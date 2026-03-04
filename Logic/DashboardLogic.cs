using BilleteraCryptoProjectAPI.Data;
using BilleteraCryptoProjectAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace BilleteraCryptoProjectAPI.Logic {
    public class DashboardLogic : IDashboardService {
        private readonly CryptoWalletApiDBContext _context;
        private readonly ICriptoyaService _criptoyaService;

        public DashboardLogic(CryptoWalletApiDBContext context, ICriptoyaService criptoyaService) {
            _context = context;
            _criptoyaService = criptoyaService;
        }

        public async Task<DashboardSummaryDTO?> GetByClienteIdAsync(int clienteId)
        {
            // Verifica si el cliente existe en la base de datos
            var clienteExiste = await _context.Clientes.AnyAsync(c => c.ClienteId == clienteId);
            if (!clienteExiste)
            {
                return null; // Retorna null si el cliente no existe
            }

            // Obtiene todas las operaciones (compras y ventas) del cliente
            var operaciones = await _context.Operaciones
                .Where(op => op.ClienteId == clienteId)
                .ToListAsync();

            var items = new List<DashboardItemDTO>();

            // Agrupa las operaciones por criptomoneda (ej: BTC, ETH, etc.)
            foreach (var grupo in operaciones.GroupBy(op => op.CriptoCode))
            {
                // Suma total de compras para esta cripto
                var compras = grupo
                    .Where(op => op.Action.ToLower() == "purchase")
                    .Sum(op => op.CriptoAmount);

                // Suma total de ventas para esta cripto
                var ventas = grupo
                    .Where(op => op.Action.ToLower() == "sale")
                    .Sum(op => op.CriptoAmount);

                // Calcula la tenencia actual (compras - ventas)
                var cantidadActual = compras - ventas;

                // Si no tiene tenencia positiva, se omite esta cripto
                if (cantidadActual <= 0)
                {
                    continue;
                }

                // Consulta el precio actual de la cripto via CriptoYa
                var precio = await _criptoyaService.GetPriceAsync(grupo.Key);

                // Calcula el valor en dinero de la tenencia actual
                var money = cantidadActual * precio;

                // Agrega el item al listado del dashboard
                items.Add(new DashboardItemDTO
                {
                    CriptoCode = grupo.Key,
                    Cantidad = cantidadActual,
                    Money = money
                });
            }

            // Calcula el valor total del portafolio sumando todos los items
            var total = items.Sum(i => i.Money);

            // Retorna el resumen completo del dashboard
            return new DashboardSummaryDTO
            {
                ClienteID = clienteId,
                TotalMoney = total,
                Items = items
            };
        }
    }
}



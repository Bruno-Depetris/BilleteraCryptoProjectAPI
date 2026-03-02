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

        public async Task<DashboardSummaryDTO?> GetByClienteIdAsync(int clienteId) {
            var clienteExiste = await _context.Clientes.AnyAsync(c => c.ClienteId == clienteId);
            if (!clienteExiste) {
                return null;
            }

            var operaciones = await _context.Operaciones
                .Where(op => op.ClienteId == clienteId)
                .ToListAsync();

            var items = new List<DashboardItemDTO>();

            foreach (var grupo in operaciones.GroupBy(op => op.CriptoCode)) {
                var compras = grupo
                    .Where(op => op.Action.ToLower() == "purchase")
                    .Sum(op => op.CriptoAmount);

                var ventas = grupo
                    .Where(op => op.Action.ToLower() == "sale")
                    .Sum(op => op.CriptoAmount);

                var cantidadActual = compras - ventas;
                if (cantidadActual <= 0) {
                    continue;
                }

                var precio = await _criptoyaService.GetPriceAsync(grupo.Key);
                var money = cantidadActual * precio;

                items.Add(new DashboardItemDTO {
                    CriptoCode = grupo.Key,
                    Cantidad = cantidadActual,
                    Money = money
                });
            }

            var total = items.Sum(i => i.Money);

            return new DashboardSummaryDTO {
                ClienteID = clienteId,
                TotalMoney = total,
                Items = items
            };
        }
    }
}



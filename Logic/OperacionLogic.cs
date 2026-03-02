using BilleteraCryptoProjectAPI.Data;
using BilleteraCryptoProjectAPI.Models;
using BilleteraCryptoProjectAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace BilleteraCryptoProjectAPI.Logic {
    public class OperacionLogic : IOperacionService {
        private readonly CryptoWalletApiDBContext _context;
        private readonly ICriptoyaService _criptoyaService;

        public OperacionLogic(CryptoWalletApiDBContext context, ICriptoyaService criptoyaService) {
            _context = context;
            _criptoyaService = criptoyaService;
        }

        public async Task<IEnumerable<OperacionReadDTO>> GetAllAsync() {
            return await _context.Operaciones
                .OrderByDescending(op => op.Datetime)
                .Select(op => new OperacionReadDTO {
                    OperacionID = op.OperacionId,
                    ClienteID = op.ClienteId,
                    CriptoCode = op.CriptoCode,
                    CriptoAmount = op.CriptoAmount,
                    Money = op.Money,
                    Action = op.Action,
                    Datetime = op.Datetime
                })
                .ToListAsync();
        }

        public async Task<OperacionReadDTO?> GetByIdAsync(int id) {
            return await _context.Operaciones
                .Where(op => op.OperacionId == id)
                .Select(op => new OperacionReadDTO {
                    OperacionID = op.OperacionId,
                    ClienteID = op.ClienteId,
                    CriptoCode = op.CriptoCode,
                    CriptoAmount = op.CriptoAmount,
                    Money = op.Money,
                    Action = op.Action,
                    Datetime = op.Datetime
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<OperacionReadDTO>> GetByClienteIdAsync(int clienteId) {
            return await _context.Operaciones
                .Where(op => op.ClienteId == clienteId)
                .OrderByDescending(op => op.Datetime)
                .Select(op => new OperacionReadDTO {
                    OperacionID = op.OperacionId,
                    ClienteID = op.ClienteId,
                    CriptoCode = op.CriptoCode,
                    CriptoAmount = op.CriptoAmount,
                    Money = op.Money,
                    Action = op.Action,
                    Datetime = op.Datetime
                })
                .ToListAsync();
        }

        public async Task<OperacionReadDTO> CreateAsync(OperacionCreateDTO dto) {
            var clienteExists = await _context.Clientes.AnyAsync(c => c.ClienteId == dto.ClienteID);
            if (!clienteExists) {
                throw new InvalidOperationException($"El cliente con ID {dto.ClienteID} no existe.");
            }

            if (dto.CriptoAmount <= 0) {
                throw new InvalidOperationException("La cantidad de criptomoneda debe ser mayor a 0.");
            }

            if (dto.Action.ToLower() == "sale") {
                var compras = await _context.Operaciones
                    .Where(op => op.ClienteId == dto.ClienteID && 
                                 op.CriptoCode == dto.CriptoCode && 
                                 op.Action.ToLower() == "purchase")
                    .SumAsync(op => op.CriptoAmount);

                var ventas = await _context.Operaciones
                    .Where(op => op.ClienteId == dto.ClienteID && 
                                 op.CriptoCode == dto.CriptoCode && 
                                 op.Action.ToLower() == "sale")
                    .SumAsync(op => op.CriptoAmount);

                var cantidadDisponible = compras - ventas;

                if (cantidadDisponible < dto.CriptoAmount) {
                    throw new InvalidOperationException($"No tienes suficiente {dto.CriptoCode}. " +
                        $"Tienes {cantidadDisponible} y estás intentando vender {dto.CriptoAmount}.");
                }
            }

            var precio = await _criptoyaService.GetPriceAsync(dto.CriptoCode);

            var montoTotal = dto.CriptoAmount * precio;

            var operacion = new Operacione {
                ClienteId = dto.ClienteID,
                CriptoCode = dto.CriptoCode,
                CriptoAmount = dto.CriptoAmount,
                Money = montoTotal,
                Action = dto.Action.ToLower(),
                Datetime = dto.Datetime
            };

            _context.Operaciones.Add(operacion);
            await _context.SaveChangesAsync();

            return new OperacionReadDTO {
                OperacionID = operacion.OperacionId,
                ClienteID = operacion.ClienteId,
                CriptoCode = operacion.CriptoCode,
                CriptoAmount = operacion.CriptoAmount,
                Money = operacion.Money,
                Action = operacion.Action,
                Datetime = operacion.Datetime
            };
        }

        public async Task<bool> UpdateAsync(int id, OperacionUpdateDTO dto) {
            var operacion = await _context.Operaciones.FindAsync(id);
            if (operacion == null) return false;

            if (dto.Money.HasValue) {
                operacion.Money = dto.Money.Value;
            }

            if (!string.IsNullOrEmpty(dto.CriptoCode)) {
                operacion.CriptoCode = dto.CriptoCode;
            }

            if (dto.CriptoAmount.HasValue) {
                operacion.CriptoAmount = dto.CriptoAmount.Value;
            }

            if (!string.IsNullOrEmpty(dto.Action)) {
                operacion.Action = dto.Action.ToLower();
            }

            if (dto.Datetime.HasValue) {
                operacion.Datetime = dto.Datetime.Value;
            }

            _context.Operaciones.Update(operacion);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id) {
            var operacion = await _context.Operaciones.FindAsync(id);
            if (operacion == null) return false;

            _context.Operaciones.Remove(operacion);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}



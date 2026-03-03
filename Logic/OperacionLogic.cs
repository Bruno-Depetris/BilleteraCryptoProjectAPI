using BilleteraCryptoProjectAPI.Data;
using BilleteraCryptoProjectAPI.Models;
using BilleteraCryptoProjectAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace BilleteraCryptoProjectAPI.Logic {
    public class OperacionLogic : IOperacionService {
        private readonly CryptoWalletApiDBContext _context;
        private readonly ICriptoyaService _criptoyaService;
        private static readonly HashSet<string> AccionesPermitidas = new(StringComparer.OrdinalIgnoreCase) { "purchase", "sale" };

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

            if (string.IsNullOrWhiteSpace(dto.CriptoCode)) {
                throw new InvalidOperationException("El código de criptomoneda es obligatorio.");
            }

            var criptoCode = dto.CriptoCode.Trim();
            var criptoExists = await _context.Criptos.AnyAsync(c => c.CriptoCode == criptoCode);
            if (!criptoExists) {
                throw new InvalidOperationException($"La criptomoneda '{criptoCode}' no existe.");
            }

            if (string.IsNullOrWhiteSpace(dto.Action)) {
                throw new InvalidOperationException("La acción es obligatoria y debe ser 'purchase' o 'sale'.");
            }

            var action = dto.Action.Trim().ToLowerInvariant();
            if (!AccionesPermitidas.Contains(action)) {
                throw new InvalidOperationException("La acción debe ser 'purchase' o 'sale'.");
            }

            if (dto.CriptoAmount <= 0) {
                throw new InvalidOperationException("La cantidad de criptomoneda debe ser mayor a 0.");
            }

            if (action == "sale") {
                var compras = await _context.Operaciones
                    .Where(op => op.ClienteId == dto.ClienteID && 
                                 op.CriptoCode == criptoCode && 
                                 op.Action.ToLower() == "purchase")
                    .SumAsync(op => op.CriptoAmount);

                var ventas = await _context.Operaciones
                    .Where(op => op.ClienteId == dto.ClienteID && 
                                 op.CriptoCode == criptoCode && 
                                 op.Action.ToLower() == "sale")
                    .SumAsync(op => op.CriptoAmount);

                var cantidadDisponible = compras - ventas;

                if (cantidadDisponible < dto.CriptoAmount) {
                    throw new InvalidOperationException($"No tienes suficiente {criptoCode}. " +
                        $"Tienes {cantidadDisponible} y estás intentando vender {dto.CriptoAmount}.");
                }
            }

            var precio = await _criptoyaService.GetPriceAsync(criptoCode);

            var montoTotal = dto.CriptoAmount * precio;

            var datetime = dto.Datetime == default ? DateTime.UtcNow : dto.Datetime;

            var operacion = new Operacione {
                ClienteId = dto.ClienteID,
                CriptoCode = criptoCode,
                CriptoAmount = dto.CriptoAmount,
                Money = montoTotal,
                Action = action,
                Datetime = datetime
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
                var newCriptoCode = dto.CriptoCode.Trim();
                var criptoExists = await _context.Criptos.AnyAsync(c => c.CriptoCode == newCriptoCode);
                if (!criptoExists) {
                    throw new InvalidOperationException($"La criptomoneda '{newCriptoCode}' no existe.");
                }
                operacion.CriptoCode = newCriptoCode;
            }

            if (dto.CriptoAmount.HasValue) {
                if (dto.CriptoAmount.Value <= 0) {
                    throw new InvalidOperationException("La cantidad de criptomoneda debe ser mayor a 0.");
                }
                operacion.CriptoAmount = dto.CriptoAmount.Value;
            }

            if (!string.IsNullOrEmpty(dto.Action)) {
                var action = dto.Action.Trim().ToLowerInvariant();
                if (!AccionesPermitidas.Contains(action)) {
                    throw new InvalidOperationException("La acción debe ser 'purchase' o 'sale'.");
                }
                operacion.Action = action;
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



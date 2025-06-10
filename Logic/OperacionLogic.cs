using BilleteraCryptoProjectAPI.Data;
using BilleteraCryptoProjectAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BilleteraCryptoProjectAPI.Logic {
    public class OperacionLogic : IOperacionService {
        private readonly CryptoWalletApiDBContext _context;

        public OperacionLogic(CryptoWalletApiDBContext context) {
            _context = context;
        }

        public async Task<IEnumerable<OperacionReadDTO>> GetAllAsync() {
            return await _context.Operaciones
                .Select(op => new OperacionReadDTO {
                    OperacionID = op.OperacionId,
                    CuentaID = op.CuentaId,
                    CriptoCode = op.CriptoCode,
                    Cantidad = op.Cantidad,
                    MontoARS = op.MontoArs,
                    AccionID = op.AccionId,
                    Fecha = op.Fecha
                })
                .ToListAsync();
        }

        public async Task<OperacionReadDTO?> GetByIdAsync(int id) {
            return await _context.Operaciones
                .Where(op => op.OperacionId == id)
                .Select(op => new OperacionReadDTO {
                    OperacionID = op.OperacionId,
                    CuentaID = op.CuentaId,
                    CriptoCode = op.CriptoCode,
                    Cantidad = op.Cantidad,
                    MontoARS = op.MontoArs,
                    AccionID = op.AccionId,
                    Fecha = op.Fecha
                })
                .FirstOrDefaultAsync();
        }

        public async Task<OperacionReadDTO> CreateAsync(OperacionCreateDTO dto) {
            var operacion = new Operacione {
                CuentaId = dto.CuentaID,
                CriptoCode = dto.CriptoCode,
                Cantidad = dto.Cantidad,
                MontoArs = dto.MontoARS,
                AccionId = dto.AccionID,
                Fecha = DateTime.UtcNow
            };
            _context.Operaciones.Add(operacion);
            await _context.SaveChangesAsync();
            return new OperacionReadDTO {
                OperacionID = operacion.OperacionId,
                CuentaID = operacion.CuentaId,
                CriptoCode = operacion.CriptoCode,
                Cantidad = operacion.Cantidad,
                MontoARS = operacion.MontoArs,
                AccionID = operacion.AccionId,
                Fecha = operacion.Fecha
            };
        }
    }
}

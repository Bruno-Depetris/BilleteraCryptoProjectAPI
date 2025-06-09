using BilleteraCryptoProjectAPI.Data;
using BilleteraCryptoProjectAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BilleteraCryptoProjectAPI.Logic {
    public class MovimientoLogic : IMovimientoService {
        private readonly CryptoWalletApiDBContext _context;

        public MovimientoLogic(CryptoWalletApiDBContext context) {
            _context = context;
        }

        public async Task<IEnumerable<MovimientoReadDTO>> GetAllAsync() {
            return await _context.Movimientos
                .Select(m => new MovimientoReadDTO {
                    MovimientoID = m.MovimientoId,
                    Fecha = m.Fecha,
                    OperacionID = m.OperacionId,
                    CantidadCripto = m.CantidadCripto,
                    CriptoCode = m.CriptoCode,
                    EstadoBilletera = m.EstadoBilletera
                })
                .ToListAsync();
        }

        public async Task<MovimientoReadDTO?> GetByIdAsync(int id) {
            return await _context.Movimientos
                .Where(m => m.MovimientoId == id)
                .Select(m => new MovimientoReadDTO {
                    MovimientoID = m.MovimientoId,
                    Fecha = m.Fecha,
                    OperacionID = m.OperacionId,
                    CantidadCripto = m.CantidadCripto,
                    CriptoCode = m.CriptoCode,
                    EstadoBilletera = m.EstadoBilletera
                })
                .FirstOrDefaultAsync();
        }

        public async Task<MovimientoReadDTO> CreateAsync(MovimientoCreateDTO dto) {
            var movimiento = new Movimiento {
                Fecha = dto.Fecha,
                OperacionId = dto.OperacionID,
                CantidadCripto = dto.CantidadCripto,
                CriptoCode = dto.CriptoCode,
                EstadoBilletera = dto.EstadoBilletera
            };
            _context.Movimientos.Add(movimiento);
            await _context.SaveChangesAsync();
            return new MovimientoReadDTO {
                MovimientoID = movimiento.MovimientoId,
                Fecha = movimiento.Fecha,
                OperacionID = movimiento.OperacionId,
                CantidadCripto = movimiento.CantidadCripto,
                CriptoCode = movimiento.CriptoCode,
                EstadoBilletera = movimiento.EstadoBilletera
            };
        }
    }
}

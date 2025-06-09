using BilleteraCryptoProjectAPI.Data;
using BilleteraCryptoProjectAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BilleteraCryptoProjectAPI.Logic {
    public class HistorialPrecioLogic : IHistorialPrecioService {

        private readonly CryptoWalletApiDBContext _context;

        public HistorialPrecioLogic(CryptoWalletApiDBContext context) {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<HistorialPrecioReadDTO>> GetAllAsync() {
            return await _context.HistorialPrecios
                .Select(h => new HistorialPrecioReadDTO {
                    HistorialID = h.HistorialId,
                    CriptoCode = h.CriptoCode,
                    Precio = h.Precio,
                    Fecha = h.Fecha
                }).ToListAsync();
        }
        public async Task<HistorialPrecioReadDTO?> GetByIdAsync(int id) {
            var historial = await _context.HistorialPrecios
                .FirstOrDefaultAsync(h => h.HistorialId == id);
            if (historial == null) {
                return null;
            }
            return new HistorialPrecioReadDTO {
                HistorialID = historial.HistorialId,
                CriptoCode = historial.CriptoCode,
                Precio = historial.Precio,
                Fecha = historial.Fecha
            };
        }

        public async Task<HistorialPrecioReadDTO> CreateAsync(HistorialPrecioCreateDTO dto) {
            if (dto == null) {
                throw new ArgumentNullException(nameof(dto));
            }
            var historial = new HistorialPrecio {
                CriptoCode = dto.CriptoCode,
                Precio = dto.Precio,
                Fecha = dto.Fecha
            };
            _context.HistorialPrecios.Add(historial);
            await _context.SaveChangesAsync();
            return new HistorialPrecioReadDTO {
                HistorialID = historial.HistorialId,
                CriptoCode = historial.CriptoCode,
                Precio = historial.Precio,
                Fecha = historial.Fecha
            };

        }

        
    }
}

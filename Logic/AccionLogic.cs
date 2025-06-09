using BilleteraCryptoProjectAPI.Data;
using BilleteraCryptoProjectAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BilleteraCryptoProjectAPI.Logic {
    public class AccionLogic : IAccionService {
        private readonly CryptoWalletApiDBContext _context;

        public AccionLogic(CryptoWalletApiDBContext context) {
            _context = context;
        }

        public async Task<IEnumerable<AccionReadDTO>> GetAllAsync() {
            return await _context.Acciones
                .Select(accion => new AccionReadDTO {
                    AccionID = accion.AccionId,
                    Accion = accion.Accion
                }).ToListAsync();
        }

        public async Task<AccionReadDTO?> GetByIdAsync(int id) {
            var accion = await _context.Acciones
                .Where(a => a.AccionId == id)
                .Select(accion => new AccionReadDTO {
                    AccionID = accion.AccionId,
                    Accion = accion.Accion
                }).FirstOrDefaultAsync();
            return accion;
        }

        public async Task<AccionReadDTO> CreateAsync(AccionCreateDTO dto) {
            var accion = new Accione {
                Accion = dto.Accion
            };
            _context.Acciones.Add(accion);
            await _context.SaveChangesAsync();
            return new AccionReadDTO {
                AccionID = accion.AccionId,
                Accion = accion.Accion
            };
        }

        public async Task<bool> UpdateAsync(AccionUpdateDTO dto) {
            var accion = await _context.Acciones.FindAsync(dto.AccionID);
            if (accion == null) {
                return false;
            }
            accion.Accion = dto.Accion;
            _context.Acciones.Update(accion);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id) {
            var accion = await _context.Acciones.FindAsync(id);
            if (accion == null) {
                return false;
            }
            _context.Acciones.Remove(accion);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

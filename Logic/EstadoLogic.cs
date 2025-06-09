using BilleteraCryptoProjectAPI.Data;
using BilleteraCryptoProjectAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BilleteraCryptoProjectAPI.Logic {
    public class EstadoLogic : IEstadoService {
        private readonly CryptoWalletApiDBContext _context;

        public EstadoLogic(CryptoWalletApiDBContext context) {
            _context = context;
        }

        public async Task<IEnumerable<EstadoReadDTO>> GetAllAsync() {
            return await _context.Estados
                .Select(e => new EstadoReadDTO {
                    EstadoID = e.EstadoId,
                    Estado = e.Estado1
                })
                .ToListAsync();
        }

        public async Task<EstadoReadDTO?> GetByIdAsync(int id) {
            var estado = await _context.Estados
                .Where(e => e.EstadoId == id)
                .Select(e => new EstadoReadDTO {
                    EstadoID = e.EstadoId,
                    Estado = e.Estado1
                })
                .FirstOrDefaultAsync();
            return estado;
        }

        public async Task<EstadoReadDTO> CreateAsync(EstadoCreateDTO dto) {
            var estado = new Estado {
                Estado1 = dto.Estado
            };
            _context.Estados.Add(estado);
            await _context.SaveChangesAsync();
            return new EstadoReadDTO {
                EstadoID = estado.EstadoId,
                Estado = estado.Estado1
            };
        }
        public async Task<bool> UpdateAsync(EstadoUpdateDTO dto) {
            var estado = await _context.Estados.FindAsync(dto.EstadoID);
            if (estado == null) {
                return false;
            }
            estado.Estado1 = dto.Estado;
            _context.Estados.Update(estado);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id) {
            var estado = await _context.Estados.FindAsync(id);
            if (estado == null) {
                return false;
            }
            _context.Estados.Remove(estado);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

using BilleteraCryptoProjectAPI.Data;
using BilleteraCryptoProjectAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BilleteraCryptoProjectAPI.Logic {
    public class CriptoLogic : ICriptoService {

        private readonly CryptoWalletApiDBContext _context;

        public CriptoLogic(CryptoWalletApiDBContext context) {
            _context = context;
        }

        public async Task<IEnumerable<CriptoReadDTO>> GetAllAsync() {
            return await _context.Criptos
                .Select(cripto => new CriptoReadDTO {
                    CriptoCode = cripto.CriptoCode,
                    Nombre = cripto.Nombre
                })
                .ToListAsync();
        }

        public async Task<CriptoReadDTO?> GetByCodeAsync(string code) {
            var cripto = await _context.Criptos.FindAsync(code);
            if (cripto == null) return null;
            return new CriptoReadDTO {
                CriptoCode = cripto.CriptoCode,
                Nombre = cripto.Nombre
            };

        }

        public async Task<CriptoReadDTO> CreateAsync(CriptoCreateDTO dto) {
            var cripto = new Cripto {
                CriptoCode = dto.CriptoCode,
                Nombre = dto.Nombre
            };
            _context.Criptos.Add(cripto);
            await _context.SaveChangesAsync();
            return new CriptoReadDTO {
                CriptoCode = cripto.CriptoCode,
                Nombre = cripto.Nombre
            };
        }

        public async Task<bool> UpdateAsync(CriptoUpdateDTO dto) {
            var cripto = await _context.Criptos.FindAsync(dto.CriptoCode);
            if (cripto == null) return false;
            cripto.Nombre = dto.Nombre;
            _context.Criptos.Update(cripto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string code) {
            var cripto = await _context.Criptos.FindAsync(code);
            if (cripto == null) return false;
            _context.Criptos.Remove(cripto);
            await _context.SaveChangesAsync();
            return true;
        }   
    }
}

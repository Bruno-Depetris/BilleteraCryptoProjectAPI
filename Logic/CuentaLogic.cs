using BilleteraCryptoProjectAPI.Data;
using BilleteraCryptoProjectAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BilleteraCryptoProjectAPI.Logic {
    public class CuentaLogic : ICuentaService {
        public readonly CryptoWalletApiDBContext _context;

        public CuentaLogic(CryptoWalletApiDBContext context) {
            _context = context;
        }

        public async Task<IEnumerable<CuentaReadDTO>> GetAllAsync() {
            return await _context.Cuentas
                .Select(cuenta => new CuentaReadDTO {
                    CuentaID = cuenta.CuentaId,

                    Cliente = new ClienteReadDTO {
                        ClienteID = cuenta.Cliente.ClienteId,
                        Nombre = cuenta.Cliente.Nombre,
                        Email = cuenta.Cliente.Email
                    },

                    Estado = new EstadoReadDTO {
                        EstadoID = cuenta.Estado.EstadoId,
                        Estado = cuenta.Estado.Estado1
                    }
                })
                .ToListAsync();
        }

        public async Task<CuentaReadDTO?> GetByIdAsync(int id) {
            var cuenta = await _context.Cuentas
                .Include(c => c.Cliente)
                .Include(c => c.Estado)
                .FirstOrDefaultAsync(c => c.CuentaId == id);
            if (cuenta == null) return null;
            return new CuentaReadDTO {
                CuentaID = cuenta.CuentaId,
                ClienteID = cuenta.Cliente.ClienteId,
                EstadoID = cuenta.Estado.EstadoId,
                Cliente = new ClienteReadDTO {
                    ClienteID = cuenta.Cliente.ClienteId,
                    Nombre = cuenta.Cliente.Nombre,
                    Email = cuenta.Cliente.Email
                },
                Estado = new EstadoReadDTO {
                    EstadoID = cuenta.Estado.EstadoId,
                    Estado = cuenta.Estado.Estado1
                }
            };
        }

        public async Task<CuentaReadDTO> CreateAsync(CuentaCreateDTO dto) {
            var cuenta = new Cuenta {
                ClienteId = dto.ClienteID,
                EstadoId = dto.EstadoID
            };
            _context.Cuentas.Add(cuenta);
            await _context.SaveChangesAsync();
            return new CuentaReadDTO {
                CuentaID = cuenta.CuentaId,
                ClienteID = cuenta.ClienteId,
                EstadoID = cuenta.EstadoId
            };
        }

        public async Task<bool> UpdateAsync(CuentaUpdateDTO dto) {
            var cuenta = await _context.Cuentas.FindAsync(dto.CuentaID);
            if (cuenta == null) return false;
            cuenta.ClienteId = dto.ClienteID;
            cuenta.EstadoId = dto.EstadoID;
            _context.Cuentas.Update(cuenta);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id) {
            var cuenta = await _context.Cuentas.FindAsync(id);
            if (cuenta == null) return false;
            _context.Cuentas.Remove(cuenta);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

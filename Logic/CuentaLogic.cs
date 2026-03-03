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
            var clienteExiste = await _context.Clientes.AnyAsync(c => c.ClienteId == dto.ClienteID);
            if (!clienteExiste) {
                throw new KeyNotFoundException($"No existe el cliente con ID {dto.ClienteID}.");
            }

            var cuentaExistente = await _context.Cuentas.FirstOrDefaultAsync(c => c.ClienteId == dto.ClienteID);
            if (cuentaExistente != null) {
                return new CuentaReadDTO {
                    CuentaID = cuentaExistente.CuentaId,
                    ClienteID = cuentaExistente.ClienteId,
                    EstadoID = cuentaExistente.EstadoId
                };
            }

            var estadoId = dto.EstadoID;
            var estadoExiste = await _context.Estados.AnyAsync(e => e.EstadoId == estadoId);
            if (!estadoExiste) {
                var estadoFallback = await _context.Estados
                    .OrderBy(e => e.EstadoId)
                    .Select(e => (int?)e.EstadoId)
                    .FirstOrDefaultAsync();

                if (!estadoFallback.HasValue) {
                    throw new InvalidOperationException("No hay estados cargados para crear la cuenta.");
                }

                estadoId = estadoFallback.Value;
            }

            var cuenta = new Cuenta {
                ClienteId = dto.ClienteID,
                EstadoId = estadoId
            };

            _context.Cuentas.Add(cuenta);

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                var cuentaCreadaPorOtroProceso = await _context.Cuentas.FirstOrDefaultAsync(c => c.ClienteId == dto.ClienteID);
                if (cuentaCreadaPorOtroProceso != null) {
                    return new CuentaReadDTO {
                        CuentaID = cuentaCreadaPorOtroProceso.CuentaId,
                        ClienteID = cuentaCreadaPorOtroProceso.ClienteId,
                        EstadoID = cuentaCreadaPorOtroProceso.EstadoId
                    };
                }

                throw;
            }

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



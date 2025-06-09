using BilleteraCryptoProjectAPI.Data;
using BilleteraCryptoProjectAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BilleteraCryptoProjectAPI.Logic {
    public class ClienteLogic : IClienteService {

        private readonly CryptoWalletApiDBContext _context;

        public ClienteLogic(CryptoWalletApiDBContext context) {
            _context = context;
        }

        public async Task<IEnumerable<ClienteReadDTO>> GetAllAsync() {
            return await _context.Clientes
                .Select(cliente => new ClienteReadDTO {
                    Nombre = cliente.Nombre,
                    Email = cliente.Email
                })
                .ToListAsync();
        }

        public async Task<ClienteReadDTO?> GetByIdAsync(int id) {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return null;
            return new ClienteReadDTO {
                Nombre = cliente.Nombre,
                Email = cliente.Email
            };

        }

        public async Task<ClienteReadDTO> CreateAsync(ClienteCreateDTO dto) {
            var cliente = new Cliente {
                Nombre = dto.Nombre,
                Email = dto.Email
            };
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return new ClienteReadDTO {
                Nombre = cliente.Nombre,
                Email = cliente.Email
            };
        }

        public async Task<bool> UpdateAsync(ClienteUpdateDTO dto) {
            var cliente = await _context.Clientes.FindAsync(dto.ClienteID);
            if (cliente == null) return false;
            cliente.Nombre = dto.Nombre;
            cliente.Email = dto.Email;
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id) {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return false;
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

public interface IClienteService
{
    Task<IEnumerable<ClienteReadDTO>> GetAllAsync();
    Task<ClienteReadDTO?> GetByIdAsync(int id);
    Task<ClienteReadDTO> CreateAsync(ClienteCreateDTO dto);
    Task<bool> UpdateAsync(ClienteUpdateDTO dto);
    Task<bool> DeleteAsync(int id);
}
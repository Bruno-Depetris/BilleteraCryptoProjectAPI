public interface IOperacionService
{
    Task<IEnumerable<OperacionReadDTO>> GetAllAsync();
    Task<OperacionReadDTO?> GetByIdAsync(int id);
    Task<OperacionReadDTO> CreateAsync(OperacionCreateDTO dto);
    Task<bool> UpdateAsync(int id, OperacionUpdateDTO dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<OperacionReadDTO>> GetByClienteIdAsync(int clienteId);
}


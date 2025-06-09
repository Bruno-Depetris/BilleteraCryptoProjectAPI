public interface IOperacionService
{
    Task<IEnumerable<OperacionReadDTO>> GetAllAsync();
    Task<OperacionReadDTO?> GetByIdAsync(int id);
    Task<OperacionReadDTO> CreateAsync(OperacionCreateDTO dto);
}
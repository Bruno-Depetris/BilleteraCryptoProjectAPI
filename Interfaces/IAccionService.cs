public interface IAccionService
{
    Task<IEnumerable<AccionReadDTO>> GetAllAsync();
    Task<AccionReadDTO?> GetByIdAsync(int id);
    Task<AccionReadDTO> CreateAsync(AccionCreateDTO dto);
    Task<bool> UpdateAsync(AccionUpdateDTO dto);
    Task<bool> DeleteAsync(int id); 
}
public interface IEstadoService
{
    Task<IEnumerable<EstadoReadDTO>> GetAllAsync();
    Task<EstadoReadDTO?> GetByIdAsync(int id);
    Task<EstadoReadDTO> CreateAsync(EstadoCreateDTO dto);
    Task<bool> UpdateAsync(EstadoUpdateDTO dto);
    Task<bool> DeleteAsync(int id);
}
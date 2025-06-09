public interface ICuentaService
{
    Task<IEnumerable<CuentaReadDTO>> GetAllAsync();
    Task<CuentaReadDTO?> GetByIdAsync(int id);
    Task<CuentaReadDTO> CreateAsync(CuentaCreateDTO dto);
    Task<bool> UpdateAsync(CuentaUpdateDTO dto);
    Task<bool> DeleteAsync(int id);
}
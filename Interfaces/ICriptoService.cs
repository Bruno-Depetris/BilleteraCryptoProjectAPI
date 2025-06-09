public interface ICriptoService
{
    Task<IEnumerable<CriptoReadDTO>> GetAllAsync();
    Task<CriptoReadDTO?> GetByCodeAsync(string code);
    Task<CriptoReadDTO> CreateAsync(CriptoCreateDTO dto);
    Task<bool> UpdateAsync(CriptoUpdateDTO dto);
    Task<bool> DeleteAsync(string code);
}
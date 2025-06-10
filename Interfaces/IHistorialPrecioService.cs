using BilleteraCryptoProjectAPI.DTO.HistorialPrecios;

public interface IHistorialPrecioService {
    Task<IEnumerable<HistorialPrecioReadDTO>> GetAllAsync();
    Task<HistorialPrecioReadDTO?> GetByIdAsync(int id);
    Task<HistorialPrecioReadDTO> CreateAsync(HistorialPrecioCreateDTO dto);
    Task<bool> UpdateAsync(HistorialPrecioUpdateDTO dto);
}

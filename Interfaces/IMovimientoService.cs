public interface IMovimientoService
{
    Task<IEnumerable<MovimientoReadDTO>> GetAllAsync();
    Task<MovimientoReadDTO?> GetByIdAsync(int id);
    Task<MovimientoReadDTO> CreateAsync(MovimientoCreateDTO dto);
}
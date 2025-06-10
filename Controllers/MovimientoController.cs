using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BilleteraCryptoProjectAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientoController : ControllerBase {

        private readonly IMovimientoService _movimientoService;
        private readonly IMapper _mapper;

        public MovimientoController(IMovimientoService movimientoService, IMapper mapper) {
            _movimientoService = movimientoService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<MovimientoReadDTO>> GetAllAsync() {
            try {
                var movimientos = await _movimientoService.GetAllAsync();
                if (movimientos == null || !movimientos.Any()) {
                    return (IEnumerable<MovimientoReadDTO>)NotFound("No se encontraron movimientos.");
                }
                return movimientos;
            } catch (Exception ex) {
                throw new Exception($"Error al obtener los movimientos: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id) {
            try {
                var movimiento = await _movimientoService.GetByIdAsync(id);
                if (movimiento == null) {
                    return NotFound($"No se encontró el movimiento con ID {id}.");
                }
                return Ok(movimiento);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener el movimiento: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] MovimientoCreateDTO movimientoCreateDTO) {
            if (movimientoCreateDTO == null) {
                return BadRequest("El objeto Movimiento no puede ser nulo.");
            }
            try {
                var movimiento = await _movimientoService.CreateAsync(movimientoCreateDTO);
                return CreatedAtAction(nameof(GetByIdAsync), new { id = movimiento.MovimientoID }, movimiento);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al crear el movimiento: {ex.Message}");
            }
        }


    }
}

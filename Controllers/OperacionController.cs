using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BilleteraCryptoProjectAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class OperacionController : ControllerBase {

        public readonly IOperacionService _operacionService;
        public readonly IMapper mapper;

        public OperacionController(IOperacionService operacionService, IMapper mapper) {
            _operacionService = operacionService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync() {
            try {
                var operaciones = await _operacionService.GetAllAsync();
                if (operaciones == null || !operaciones.Any()) {
                    return NotFound("No se encontraron operaciones.");
                }
                return Ok(operaciones);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener las operaciones: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id) {
            try {
                var operacion = await _operacionService.GetByIdAsync(id);
                if (operacion == null) {
                    return NotFound($"No se encontró la operación con ID {id}.");
                }
                return Ok(operacion);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener la operación: {ex.Message}");
            }
        }

        [HttpPost]

        public async Task<IActionResult> CreateAsync([FromBody] OperacionCreateDTO operacionCreateDTO) {
            if (operacionCreateDTO == null) {
                return BadRequest("El objeto Operacion no puede ser nulo.");
            }
            try {
                var operacion = await _operacionService.CreateAsync(operacionCreateDTO);
                return CreatedAtAction(nameof(GetByIdAsync), new { id = operacion.OperacionID }, operacion);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al crear la operación: {ex.Message}");
            }
        }
    }
}

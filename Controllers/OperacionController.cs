using AutoMapper;
using BilleteraCryptoProjectAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BilleteraCryptoProjectAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class OperacionController : ControllerBase {

        public readonly IOperacionService _operacionService;
        public readonly ICriptoyaService _criptoyaService;
        public readonly IMapper mapper;

        public OperacionController(IOperacionService operacionService, ICriptoyaService criptoyaService, IMapper mapper) {
            _operacionService = operacionService;
            _criptoyaService = criptoyaService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get() {
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
        public async Task<IActionResult> Get(int id) {
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

        [HttpGet("cliente/{clienteId:int}")]
        public async Task<IActionResult> GetByClienteId(int clienteId) {
            try {
                var operaciones = await _operacionService.GetByClienteIdAsync(clienteId);
                if (operaciones == null || !operaciones.Any()) {
                    return NotFound($"No se encontraron operaciones para el cliente {clienteId}.");
                }
                return Ok(operaciones);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener las operaciones: {ex.Message}");
            }
        }

        [HttpGet("precio/{criptoCode}")]
        public async Task<IActionResult> GetPrecioActual(string criptoCode) {
            if (string.IsNullOrWhiteSpace(criptoCode)) {
                return BadRequest("El código de criptomoneda es obligatorio.");
            }

            try {
                var code = criptoCode.Trim().ToUpperInvariant();
                var precio = await _criptoyaService.GetPriceAsync(code);
                return Ok(new {
                    CriptoCode = code,
                    Precio = precio,
                    FechaConsulta = DateTime.UtcNow
                });
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener el precio actual: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OperacionCreateDTO operacionCreateDTO) {
            if (operacionCreateDTO == null) {
                return BadRequest("El objeto Operacion no puede ser nulo.");
            }
            try {
                var operacion = await _operacionService.CreateAsync(operacionCreateDTO);
                return CreatedAtAction(nameof(Get), new { id = operacion.OperacionID }, operacion);
            } catch (InvalidOperationException ex) {
                return BadRequest(ex.Message);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al crear la operación: {ex.Message}");
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, [FromBody] OperacionUpdateDTO operacionUpdateDTO) {
            if (operacionUpdateDTO == null) {
                return BadRequest("El objeto Operacion no puede ser nulo.");
            }
            try {
                var result = await _operacionService.UpdateAsync(id, operacionUpdateDTO);
                if (!result) {
                    return NotFound($"No se encontró la operación con ID {id}.");
                }
                return NoContent();
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar la operación: {ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) {
            try {
                var result = await _operacionService.DeleteAsync(id);
                if (!result) {
                    return NotFound($"No se encontró la operación con ID {id}.");
                }
                return NoContent();
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al eliminar la operación: {ex.Message}");
            }
        }
    }
}



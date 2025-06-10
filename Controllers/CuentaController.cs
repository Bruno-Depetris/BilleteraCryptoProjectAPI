using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BilleteraCryptoProjectAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CuentaController : ControllerBase {

        public readonly ICuentaService _cuentaService;
        public readonly IMapper mapper;

        public CuentaController(ICuentaService cuentaService, IMapper mapper) {
            _cuentaService = cuentaService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync() {
            try {
                var cuentas = await _cuentaService.GetAllAsync();
                if (cuentas == null || !cuentas.Any()) {
                    return NotFound("No se encontraron cuentas.");
                }
                return Ok(cuentas);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener las cuentas: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]

        public async Task<IActionResult> GetByIdAsync(int id) {
            try {
                var cuenta = await _cuentaService.GetByIdAsync(id);
                if (cuenta == null) {
                    return NotFound($"No se encontró la cuenta con ID {id}.");
                }
                return Ok(cuenta);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener la cuenta: {ex.Message}");
            }
        }

        [HttpPost]

        public async Task<IActionResult> CreateAsync([FromBody] CuentaCreateDTO cuentaCreateDTO) {
            if (cuentaCreateDTO == null) {
                return BadRequest("El objeto Cuenta no puede ser nulo.");
            }
            try {
                var cuenta = await _cuentaService.CreateAsync(cuentaCreateDTO);
                return CreatedAtAction(nameof(GetByIdAsync), new { id = cuenta.CuentaID }, cuenta);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al crear la cuenta: {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]

        public async Task<IActionResult> UpdateAsync(int id, [FromBody] CuentaUpdateDTO cuentaUpdateDTO) {
            if (cuentaUpdateDTO == null || cuentaUpdateDTO.CuentaID != id) {
                return BadRequest("El objeto Cuenta no puede ser nulo y el ID debe coincidir.");
            }
            try {
                var result = await _cuentaService.UpdateAsync(cuentaUpdateDTO);
                if (!result) {
                    return NotFound($"No se encontró la cuenta con ID {id} para actualizar.");
                }
                return NoContent();
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar la cuenta: {ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]


        public async Task<IActionResult> DeleteAsync(int id) {
            try {
                var result = await _cuentaService.DeleteAsync(id);
                if (!result) {
                    return NotFound($"No se encontró la cuenta con ID {id} para eliminar.");
                }
                return NoContent();
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al eliminar la cuenta: {ex.Message}");
            }
        }

    }
}

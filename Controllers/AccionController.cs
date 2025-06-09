using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BilleteraCryptoProjectAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AccionController : ControllerBase {

        public readonly IAccionService _accionService;
        public readonly IMapper mapper;

        public AccionController(IAccionService accionService, IMapper mapper) {
            _accionService = accionService;
            this.mapper = mapper;

        }

        [HttpGet]
        public IActionResult Get() {

            try {
                var acciones = _accionService.GetAllAsync().Result;
                if (acciones == null || !acciones.Any()) {
                    return NotFound("No se encontraron acciones.");
                }
                return Ok(acciones);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener las acciones: {ex.Message}");
            }



        }

        [HttpGet("{id:int}")]

        public IActionResult Get(int id) {
            try {
                var accion = _accionService.GetByIdAsync(id).Result;
                if (accion == null) {
                    return NotFound($"No se encontró la acción con ID {id}.");
                }
                return Ok(accion);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener la acción: {ex.Message}");
            }
        }

        [HttpPost]

        public IActionResult Post([FromBody] AccionCreateDTO accionCreateDTO) {
            if (accionCreateDTO == null) {
                return BadRequest("El objeto Accion no puede ser nulo.");
            }
            try {
                var accion = _accionService.CreateAsync(accionCreateDTO).Result;
                return CreatedAtAction(nameof(Get), new { id = accion.AccionID }, accion);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al crear la acción: {ex.Message}");
            }

        }

        [HttpPut("{id:int}")]

        public IActionResult Put(int id, [FromBody] AccionUpdateDTO accionUpdateDTO) {
            if (accionUpdateDTO == null || id != accionUpdateDTO.AccionID) {
                return BadRequest("El objeto Accion no puede ser nulo y el ID debe coincidir.");
            }
            try {
                var result = _accionService.UpdateAsync(accionUpdateDTO).Result;
                if (!result) {
                    return NotFound($"No se encontró la acción con ID {id} para actualizar.");
                }
                return NoContent();
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar la acción: {ex.Message}");
            }
        }
    }
}

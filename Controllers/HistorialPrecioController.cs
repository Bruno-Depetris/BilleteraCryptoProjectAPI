using AutoMapper;
using BilleteraCryptoProjectAPI.DTO.HistorialPrecios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BilleteraCryptoProjectAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialPrecioController : ControllerBase {

        public readonly IHistorialPrecioService _historialPrecioService;
        public readonly IMapper mapper;

        public HistorialPrecioController(IHistorialPrecioService historialPrecioService, IMapper mapper) {
            _historialPrecioService = historialPrecioService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync() {
            try {
                var historialPrecios = await _historialPrecioService.GetAllAsync();
                if (historialPrecios == null || !historialPrecios.Any()) {
                    return NotFound("No se encontraron historiales de precios.");
                }
                return Ok(historialPrecios);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener los historiales de precios: {ex.Message}");
            }
        }



        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id) {
            try {
                var historialPrecio = await _historialPrecioService.GetByIdAsync(id);
                if (historialPrecio == null) {
                    return NotFound($"No se encontró el historial de precios con ID {id}.");
                }
                return Ok(historialPrecio);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener el historial de precios: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] HistorialPrecioCreateDTO historialPrecioCreateDTO) {
            if (historialPrecioCreateDTO == null) {
                return BadRequest("El objeto HistorialPrecio no puede ser nulo.");
            }
            try {
                var historialPrecio = await _historialPrecioService.CreateAsync(historialPrecioCreateDTO);
                return CreatedAtAction(nameof(GetByIdAsync), new { id = historialPrecio.HistorialID }, historialPrecio);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al crear el historial de precios: {ex.Message}");
            }

        }


        [HttpPut("{id:int}")]

        public async Task<IActionResult> UpdateAsync(int id, [FromBody] HistorialPrecioUpdateDTO historialPrecioUpdateDTO) {
            if (historialPrecioUpdateDTO == null) {
                return BadRequest("El objeto HistorialPrecio no puede ser nulo.");
            }
            if (id != historialPrecioUpdateDTO.HistorialPrecioID) {
                return BadRequest("El ID del historial de precios no coincide con el ID proporcionado en la URL.");
            }
            try {
                var result = await _historialPrecioService.UpdateAsync(historialPrecioUpdateDTO);
                if (!result) {
                    return NotFound($"No se encontró el historial de precios con ID {id} para actualizar.");
                }
                return NoContent();
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar el historial de precios: {ex.Message}");
            }
        }

    }
}

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BilleteraCryptoProjectAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase {

        public readonly IClienteService clienteService;
        public readonly IMapper mapper;

        public ClienteController(IClienteService clienteService, IMapper mapper) {
            this.clienteService = clienteService;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> Get() {
            try {
                var clientes = await clienteService.GetAllAsync();
                if (clientes == null || !clientes.Any()) {
                    return NotFound("No se encontraron clientes.");
                }
                return Ok(clientes);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener los clientes: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id) {
            try {
                var cliente = await clienteService.GetByIdAsync(id);
                if (cliente == null) {
                    return NotFound($"No se encontró el cliente con ID {id}.");
                }
                return Ok(cliente);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener el cliente: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClienteCreateDTO clienteCreateDTO) {
            if (clienteCreateDTO == null) {
                return BadRequest("El objeto Cliente no puede ser nulo.");
            }
            try {
                var cliente = await clienteService.CreateAsync(clienteCreateDTO);
                return CreatedAtAction(nameof(Get), new { id = cliente.ClienteID }, cliente);
            } catch (DbUpdateException dbEx) when (dbEx.InnerException?.Message.Contains("Duplicate", StringComparison.OrdinalIgnoreCase) == true || dbEx.InnerException?.Message.Contains("Email", StringComparison.OrdinalIgnoreCase) == true) {
                return Conflict("Ya existe un cliente con ese email.");
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al crear el cliente: {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] ClienteUpdateDTO clienteUpdateDTO) {
            if (clienteUpdateDTO == null || clienteUpdateDTO.ClienteID != id) {
                return BadRequest("El objeto Cliente no puede ser nulo y debe tener el ID correcto.");
            }
            try {
                var updated = await clienteService.UpdateAsync(clienteUpdateDTO);
                if (!updated) {
                    return NotFound($"No se encontró el cliente con ID {id} para actualizar.");
                }
                return NoContent();
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar el cliente: {ex.Message}");
            }
        }



        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) {

            try {
                var deleted = await clienteService.DeleteAsync(id);
                if (!deleted) {
                    return NotFound($"No se encontró el cliente con ID {id} para eliminar.");
                }
                return NoContent();
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al eliminar el cliente: {ex.Message}");
            }


        }
    }
}



using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BilleteraCryptoProjectAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService) {
            _dashboardService = dashboardService;
        }

        [HttpGet("cliente/{clienteId:int}")]
        public async Task<IActionResult> GetByClienteId(int clienteId) {
            try {
                var resumen = await _dashboardService.GetByClienteIdAsync(clienteId);
                if (resumen == null) {
                    return NotFound($"No se encontro el cliente con ID {clienteId}.");
                }
                return Ok(resumen);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener el dashboard: {ex.Message}");
            }
        }
    }
}



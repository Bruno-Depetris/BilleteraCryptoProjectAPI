using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BilleteraCryptoProjectAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CriptoController : ControllerBase {
        private readonly ICriptoService criptoService;
        private readonly IMapper mapper;
        public CriptoController(ICriptoService criptoService, IMapper mapper) {
            this.criptoService = criptoService;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IEnumerable<CriptoReadDTO>> GetAllAsync() {
            var criptos = await criptoService.GetAllAsync();
            return mapper.Map<IEnumerable<CriptoReadDTO>>(criptos);
        }

        [HttpGet("{code}")]
        public async Task<ActionResult<CriptoReadDTO>> GetByCodeAsync(string code) {
            var cripto = await criptoService.GetByCodeAsync(code);
            if (cripto == null) {
                return NotFound();
            }
            return mapper.Map<CriptoReadDTO>(cripto);
        }

        [HttpPost]
        public async Task<ActionResult<CriptoReadDTO>> CreateAsync(CriptoCreateDTO dto) {
            var cripto = await criptoService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByCodeAsync), new { code = cripto.Code }, mapper.Map<CriptoReadDTO>(cripto));
        }

        [HttpPut("{code}")]
        public ActionResult<bool> UpdateAsync(string code, CriptoUpdateDTO dto) {
            if (code != dto.CriptoCode) {
                return BadRequest("Code mismatch");
            }
            var result = criptoService.UpdateAsync(dto);
            if (!result.Result) {
                return NotFound();
            }
            return Ok(result.Result);
        }

        [HttpDelete("{code}")]
        public async Task<ActionResult<bool>> DeleteAsync(string code) {
            var result = await criptoService.DeleteAsync(code);
            if (!result) {
                return NotFound();
            }
            return Ok(result);
        }
    }
}

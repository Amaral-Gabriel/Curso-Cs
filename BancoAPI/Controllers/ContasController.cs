using System.Security.Claims;
using BancoAPI.DTOs;
using BancoAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BancoAPI.Controllers
{
    [ApiController]
    [Route("api/contas")]
    [Authorize]
    public class ContasController : ControllerBase
    {
        private readonly ContaService _contaService;

        public ContasController(ContaService contaService) => _contaService = contaService;

        private int GetClienteId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        [ProducesResponseType(typeof(List<ContaResponseDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetMinhasContas()
            => Ok(await _contaService.GetByClienteIdAsync(GetClienteId()));

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ContaResponseDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> GetById(int id)
        {
            try { return Ok(await _contaService.GetByIdAsync(id, GetClienteId())); }
            catch (KeyNotFoundException ex) { return NotFound(new { erro = ex.Message }); }
            catch (UnauthorizedAccessException) { return Forbid(); }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ContaResponseDto), 201)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> AbrirConta([FromBody] AbrirContaDto dto)
        {
            try
            {
                var conta = await _contaService.AbrirContaAsync(dto, GetClienteId());
                return CreatedAtAction(nameof(GetById), new { id = conta.Id }, conta);
            }
            catch (ArgumentException ex) { return BadRequest(new { erro = ex.Message }); }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> EncerrarConta(int id)
        {
            try { await _contaService.EncerrarContaAsync(id, GetClienteId()); return NoContent(); }
            catch (KeyNotFoundException ex) { return NotFound(new { erro = ex.Message }); }
            catch (InvalidOperationException ex) { return BadRequest(new { erro = ex.Message }); }
            catch (UnauthorizedAccessException) { return Forbid(); }
        }
    }
}

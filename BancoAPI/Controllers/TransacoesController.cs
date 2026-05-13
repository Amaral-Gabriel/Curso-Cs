using System.Security.Claims;
using BancoAPI.DTOs;
using BancoAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BancoAPI.Controllers
{
    [ApiController]
    [Route("api/transacoes")]
    [Authorize]
    public class TransacoesController : ControllerBase
    {
        private readonly TransacaoService _transacaoService;

        public TransacoesController(TransacaoService transacaoService) => _transacaoService = transacaoService;

        private int GetClienteId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost("deposito")]
        [ProducesResponseType(typeof(TransacaoResponseDto), 200)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Depositar([FromBody] DepositoDto dto)
        {
            try { return Ok(await _transacaoService.DepositarAsync(dto, GetClienteId())); }
            catch (KeyNotFoundException ex) { return NotFound(new { erro = ex.Message }); }
            catch (UnauthorizedAccessException) { return Forbid(); }
        }

        [HttpPost("saque")]
        [ProducesResponseType(typeof(TransacaoResponseDto), 200)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Sacar([FromBody] SaqueDto dto)
        {
            try { return Ok(await _transacaoService.SacarAsync(dto, GetClienteId())); }
            catch (KeyNotFoundException ex) { return NotFound(new { erro = ex.Message }); }
            catch (InvalidOperationException ex) { return BadRequest(new { erro = ex.Message }); }
            catch (UnauthorizedAccessException) { return Forbid(); }
        }

        [HttpPost("pix")]
        [ProducesResponseType(typeof(TransacaoResponseDto), 200)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Pix([FromBody] PixDto dto)
        {
            try { return Ok(await _transacaoService.PixAsync(dto, GetClienteId())); }
            catch (KeyNotFoundException ex) { return NotFound(new { erro = ex.Message }); }
            catch (InvalidOperationException ex) { return BadRequest(new { erro = ex.Message }); }
            catch (UnauthorizedAccessException) { return Forbid(); }
        }

        [HttpGet("{contaId}")]
        [ProducesResponseType(typeof(List<TransacaoResponseDto>), 200)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> GetExtrato(int contaId)
        {
            try { return Ok(await _transacaoService.GetExtratoAsync(contaId, GetClienteId())); }
            catch (KeyNotFoundException ex) { return NotFound(new { erro = ex.Message }); }
            catch (UnauthorizedAccessException) { return Forbid(); }
        }
    }
}

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

        public ContasController(ContaService contaService)
        {
            _contaService = contaService;
        }

        private int GetClienteId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        /// <summary>Lista as contas do cliente autenticado</summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<ContaResponseDto>), 200)]
        public async Task<IActionResult> GetMinhasContas()
        {
            var contas = await _contaService.GetByClienteIdAsync(GetClienteId());
            return Ok(contas);
        }

        /// <summary>Detalhes de uma conta com extrato</summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ContaResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var conta = await _contaService.GetByIdAsync(id, GetClienteId());
                return Ok(conta);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { erro = ex.Message });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        /// <summary>Abre uma nova conta (Corrente ou Poupanca)</summary>
        [HttpPost]
        [ProducesResponseType(typeof(ContaResponseDto), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AbrirConta([FromBody] AbrirContaDto dto)
        {
            try
            {
                var conta = await _contaService.AbrirContaAsync(dto, GetClienteId());
                return CreatedAtAction(nameof(GetById), new { id = conta.Id }, conta);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        /// <summary>Encerra uma conta (saldo deve ser zero)</summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> EncerrarConta(int id)
        {
            try
            {
                await _contaService.EncerrarContaAsync(id, GetClienteId());
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { erro = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }
    }
}

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

        public TransacoesController(TransacaoService transacaoService)
        {
            _transacaoService = transacaoService;
        }

        private int GetClienteId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        /// <summary>Realiza um depósito na conta</summary>
        [HttpPost("deposito")]
        [ProducesResponseType(typeof(TransacaoResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Depositar([FromBody] DepositoDto dto)
        {
            try
            {
                var resultado = await _transacaoService.DepositarAsync(dto, GetClienteId());
                return Ok(resultado);
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

        /// <summary>Realiza um saque (ContaCorrente cobra taxa de R$2,50)</summary>
        [HttpPost("saque")]
        [ProducesResponseType(typeof(TransacaoResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Sacar([FromBody] SaqueDto dto)
        {
            try
            {
                var resultado = await _transacaoService.SacarAsync(dto, GetClienteId());
                return Ok(resultado);
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

        /// <summary>Realiza uma transferência PIX pelo número da conta destino</summary>
        [HttpPost("pix")]
        [ProducesResponseType(typeof(TransacaoResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Pix([FromBody] PixDto dto)
        {
            try
            {
                var resultado = await _transacaoService.PixAsync(dto, GetClienteId());
                return Ok(resultado);
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

        /// <summary>Retorna o extrato de uma conta</summary>
        [HttpGet("{contaId}")]
        [ProducesResponseType(typeof(List<TransacaoResponseDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetExtrato(int contaId)
        {
            try
            {
                var extrato = await _transacaoService.GetExtratoAsync(contaId, GetClienteId());
                return Ok(extrato);
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
    }
}

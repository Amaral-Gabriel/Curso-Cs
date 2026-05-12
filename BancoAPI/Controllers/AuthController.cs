using BancoAPI.DTOs;
using BancoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BancoAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        /// <summary>Registra um novo cliente</summary>
        [HttpPost("register")]
        [ProducesResponseType(typeof(object), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                var cliente = await _authService.RegisterAsync(dto);
                return CreatedAtAction(nameof(Register), new { id = cliente.Id }, new { mensagem = "Cadastro realizado com sucesso.", clienteId = cliente.Id });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        /// <summary>Realiza login e retorna o token JWT</summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenResponseDto), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var resultado = await _authService.LoginAsync(dto);
                return Ok(resultado);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { erro = ex.Message });
            }
        }
    }
}

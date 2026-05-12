using BancoAPI.DTOs;
using BancoAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BancoAPI.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    [Authorize(Roles = "Admin")]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClientesController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        /// <summary>Lista todos os clientes (Admin)</summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<ClienteResponseDto>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var clientes = await _clienteService.GetAllAsync();
            return Ok(clientes);
        }

        /// <summary>Busca um cliente pelo ID (Admin)</summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClienteResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var cliente = await _clienteService.GetByIdAsync(id);
                return Ok(cliente);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { erro = ex.Message });
            }
        }

        /// <summary>Atualiza dados de um cliente (Admin)</summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ClienteResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] ClienteUpdateDto dto)
        {
            try
            {
                var cliente = await _clienteService.UpdateAsync(id, dto);
                return Ok(cliente);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { erro = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        /// <summary>Remove um cliente (Admin)</summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _clienteService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { erro = ex.Message });
            }
        }
    }
}

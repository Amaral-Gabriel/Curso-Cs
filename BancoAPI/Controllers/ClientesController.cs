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

        public ClientesController(ClienteService clienteService) => _clienteService = clienteService;

        [HttpGet]
        [ProducesResponseType(typeof(List<ClienteResponseDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> GetAll()
            => Ok(await _clienteService.GetAllAsync());

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClienteResponseDto), 200)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> GetById(int id)
        {
            try { return Ok(await _clienteService.GetByIdAsync(id)); }
            catch (KeyNotFoundException ex) { return NotFound(new { erro = ex.Message }); }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ClienteResponseDto), 200)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Update(int id, [FromBody] ClienteUpdateDto dto)
        {
            try { return Ok(await _clienteService.UpdateAsync(id, dto)); }
            catch (KeyNotFoundException ex) { return NotFound(new { erro = ex.Message }); }
            catch (InvalidOperationException ex) { return BadRequest(new { erro = ex.Message }); }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Delete(int id)
        {
            try { await _clienteService.DeleteAsync(id); return NoContent(); }
            catch (KeyNotFoundException ex) { return NotFound(new { erro = ex.Message }); }
        }
    }
}

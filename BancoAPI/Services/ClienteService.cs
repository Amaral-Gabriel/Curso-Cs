using BancoAPI.Data;
using BancoAPI.DTOs;
using BancoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BancoAPI.Services
{
    public class ClienteService
    {
        private readonly BancoContext _context;

        public ClienteService(BancoContext context) => _context = context;

        public async Task<List<ClienteResponseDto>> GetAllAsync()
        {
            var clientes = await _context.Clientes.Include(c => c.Contas).ToListAsync();
            return clientes.Select(MapToDto).ToList();
        }

        public async Task<ClienteResponseDto> GetByIdAsync(int id)
        {
            var cliente = await _context.Clientes.Include(c => c.Contas).FirstOrDefaultAsync(c => c.Id == id)
                ?? throw new KeyNotFoundException("Cliente não encontrado.");
            return MapToDto(cliente);
        }

        public async Task<ClienteResponseDto> UpdateAsync(int id, ClienteUpdateDto dto)
        {
            var cliente = await _context.Clientes.FindAsync(id)
                ?? throw new KeyNotFoundException("Cliente não encontrado.");

            if (await _context.Clientes.AnyAsync(c => c.Email == dto.Email && c.Id != id))
                throw new InvalidOperationException("Email já em uso por outro cliente.");

            cliente.Nome = dto.Nome;
            cliente.Email = dto.Email;
            await _context.SaveChangesAsync();
            return MapToDto(cliente);
        }

        public async Task DeleteAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id)
                ?? throw new KeyNotFoundException("Cliente não encontrado.");
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
        }

        private static ClienteResponseDto MapToDto(Cliente c) => new()
        {
            Id = c.Id,
            Nome = c.Nome,
            Email = c.Email,
            Perfil = c.Perfil,
            Contas = c.Contas.Select(ct => new ContaResumoDto
            {
                Id = ct.Id,
                NumeroConta = ct.NumeroConta,
                Tipo = ct.Tipo,
                Saldo = ct.Saldo
            }).ToList()
        };
    }
}

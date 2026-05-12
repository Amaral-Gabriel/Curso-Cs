using BancoAPI.DTOs;
using BancoAPI.Models;
using BancoAPI.Repositories;

namespace BancoAPI.Services
{
    public class ClienteService
    {
        private readonly IClienteRepository _clienteRepo;

        public ClienteService(IClienteRepository clienteRepo)
        {
            _clienteRepo = clienteRepo;
        }

        public async Task<List<ClienteResponseDto>> GetAllAsync()
        {
            var clientes = await _clienteRepo.GetAllAsync();
            return clientes.Select(MapToDto).ToList();
        }

        public async Task<ClienteResponseDto> GetByIdAsync(int id)
        {
            var cliente = await _clienteRepo.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Cliente não encontrado.");
            return MapToDto(cliente);
        }

        public async Task<ClienteResponseDto> UpdateAsync(int id, ClienteUpdateDto dto)
        {
            var cliente = await _clienteRepo.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Cliente não encontrado.");

            var emailEmUso = await _clienteRepo.GetByEmailAsync(dto.Email);
            if (emailEmUso != null && emailEmUso.Id != id)
                throw new InvalidOperationException("Email já em uso por outro cliente.");

            cliente.Nome = dto.Nome;
            cliente.Email = dto.Email;

            await _clienteRepo.UpdateAsync(cliente);
            return MapToDto(cliente);
        }

        public async Task DeleteAsync(int id)
        {
            var cliente = await _clienteRepo.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Cliente não encontrado.");
            await _clienteRepo.DeleteAsync(cliente);
        }

        private ClienteResponseDto MapToDto(Cliente c) => new()
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

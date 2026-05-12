using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BancoAPI.DTOs;
using BancoAPI.Models;
using BancoAPI.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace BancoAPI.Services
{
    public class AuthService
    {
        private readonly IClienteRepository _clienteRepo;
        private readonly IConfiguration _config;

        public AuthService(IClienteRepository clienteRepo, IConfiguration config)
        {
            _clienteRepo = clienteRepo;
            _config = config;
        }

        public async Task<Cliente> RegisterAsync(RegisterDto dto)
        {
            var existente = await _clienteRepo.GetByEmailAsync(dto.Email);
            if (existente != null)
                throw new InvalidOperationException("Email já cadastrado.");

            var cliente = new Cliente
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
                Perfil = "Cliente"
            };

            return await _clienteRepo.CreateAsync(cliente);
        }

        public async Task<TokenResponseDto> LoginAsync(LoginDto dto)
        {
            var cliente = await _clienteRepo.GetByEmailAsync(dto.Email);
            if (cliente == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, cliente.SenhaHash))
                throw new UnauthorizedAccessException("Email ou senha inválidos.");

            var token = GerarToken(cliente);
            return new TokenResponseDto { Token = token, Nome = cliente.Nome, Perfil = cliente.Perfil };
        }

        private string GerarToken(Cliente cliente)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, cliente.Id.ToString()),
                new Claim(ClaimTypes.Email, cliente.Email),
                new Claim(ClaimTypes.Name, cliente.Nome),
                new Claim(ClaimTypes.Role, cliente.Perfil)
            };

            var horas = int.Parse(_config["Jwt:ExpiracaoHoras"]!);
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(horas),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

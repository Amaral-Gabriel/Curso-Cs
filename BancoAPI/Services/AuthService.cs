using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BancoAPI.Data;
using BancoAPI.DTOs;
using BancoAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BancoAPI.Services
{
    public class AuthService
    {
        private readonly BancoContext _context;
        private readonly IConfiguration _config;

        public AuthService(BancoContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<Cliente> RegisterAsync(RegisterDto dto)
        {
            if (await _context.Clientes.AnyAsync(c => c.Email == dto.Email))
                throw new InvalidOperationException("Email já cadastrado.");

            if (dto.TipoConta != "Corrente" && dto.TipoConta != "Poupanca")
                throw new InvalidOperationException("Tipo de conta inválido. Use 'Corrente' ou 'Poupanca'.");

            var cliente = new Cliente
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = dto.Senha,
                Perfil = "Cliente"
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            _context.Contas.Add(new Conta
            {
                NumeroConta = GerarNumeroConta(),
                Tipo = dto.TipoConta,
                TaxaSaque = dto.TipoConta == "Corrente" ? 2.50m : 0,
                TaxaJuros = dto.TipoConta == "Poupanca" ? 0.005m : 0,
                ClienteId = cliente.Id
            });
            await _context.SaveChangesAsync();

            return cliente;
        }

        public async Task<TokenResponseDto> LoginAsync(LoginDto dto)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Email == dto.Email);
            if (cliente == null || cliente.SenhaHash != dto.Senha)
                throw new UnauthorizedAccessException("Email ou senha inválidos.");

            return new TokenResponseDto { Token = GerarToken(cliente), Nome = cliente.Nome, Perfil = cliente.Perfil };
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

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(int.Parse(_config["Jwt:ExpiracaoHoras"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string GerarNumeroConta()
            => Random.Shared.Next(10000000, 99999999) + "-" + Random.Shared.Next(0, 9);
    }
}

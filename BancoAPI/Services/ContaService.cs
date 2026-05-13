using BancoAPI.Data;
using BancoAPI.DTOs;
using BancoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BancoAPI.Services
{
    public class ContaService
    {
        private readonly BancoContext _context;

        public ContaService(BancoContext context) => _context = context;

        public async Task<List<ContaResponseDto>> GetByClienteIdAsync(int clienteId)
        {
            var contas = await _context.Contas.Where(c => c.ClienteId == clienteId).ToListAsync();
            foreach (var conta in contas)
                await AplicarJurosAsync(conta);
            return contas.Select(MapToDto).ToList();
        }

        public async Task<ContaResponseDto> GetByIdAsync(int id, int clienteId)
        {
            var conta = await _context.Contas.FindAsync(id)
                ?? throw new KeyNotFoundException("Conta não encontrada.");

            if (conta.ClienteId != clienteId)
                throw new UnauthorizedAccessException("Acesso negado.");

            await AplicarJurosAsync(conta);
            return MapToDto(conta);
        }

        public async Task<ContaResponseDto> AbrirContaAsync(AbrirContaDto dto, int clienteId)
        {
            if (dto.Tipo != "Corrente" && dto.Tipo != "Poupanca")
                throw new ArgumentException("Tipo inválido. Use 'Corrente' ou 'Poupanca'.");

            var conta = new Conta
            {
                NumeroConta = await GerarNumeroUnicoAsync(),
                Tipo = dto.Tipo,
                TaxaSaque = dto.Tipo == "Corrente" ? 2.50m : 0,
                TaxaJuros = dto.Tipo == "Poupanca" ? 0.005m : 0,
                ClienteId = clienteId
            };

            _context.Contas.Add(conta);
            await _context.SaveChangesAsync();
            return MapToDto(conta);
        }

        public async Task EncerrarContaAsync(int id, int clienteId)
        {
            var conta = await _context.Contas.FindAsync(id)
                ?? throw new KeyNotFoundException("Conta não encontrada.");

            if (conta.ClienteId != clienteId)
                throw new UnauthorizedAccessException("Acesso negado.");

            if (conta.Saldo > 0)
                throw new InvalidOperationException("Encerre o saldo antes de fechar a conta.");

            _context.Contas.Remove(conta);
            await _context.SaveChangesAsync();
        }

        private async Task AplicarJurosAsync(Conta conta)
        {
            if (conta.Tipo == "Poupanca" && conta.UltimoRendimento.Date < DateTime.Today)
            {
                var dias = (DateTime.Today - conta.UltimoRendimento.Date).Days;
                for (int i = 0; i < dias; i++)
                    conta.Saldo *= 1 + conta.TaxaJuros;

                conta.UltimoRendimento = DateTime.Today;
                await _context.SaveChangesAsync();
            }
        }

        private ContaResponseDto MapToDto(Conta conta)
        {
            var transacoes = _context.Transacoes
                .Include(t => t.ContaDestino)
                .Where(t => t.ContaOrigemId == conta.Id || t.ContaDestinoId == conta.Id)
                .OrderByDescending(t => t.DataHora)
                .ToList();

            return new ContaResponseDto
            {
                Id = conta.Id,
                NumeroConta = conta.NumeroConta,
                Tipo = conta.Tipo,
                Saldo = conta.Saldo,
                TaxaSaque = conta.TaxaSaque,
                TaxaJuros = conta.TaxaJuros,
                Transacoes = transacoes.Select(t => new TransacaoResponseDto
                {
                    Id = t.Id,
                    Tipo = t.Tipo,
                    Valor = t.Valor,
                    DataHora = t.DataHora,
                    ContaDestino = t.ContaDestino?.NumeroConta
                }).ToList()
            };
        }

        private async Task<string> GerarNumeroUnicoAsync()
        {
            string numero;
            do { numero = Random.Shared.Next(100000, 999999).ToString(); }
            while (await _context.Contas.AnyAsync(c => c.NumeroConta == numero));
            return numero;
        }
    }
}

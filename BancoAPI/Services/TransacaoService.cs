using BancoAPI.Data;
using BancoAPI.DTOs;
using BancoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BancoAPI.Services
{
    public class TransacaoService
    {
        private readonly BancoContext _context;

        public TransacaoService(BancoContext context) => _context = context;

        public async Task<TransacaoResponseDto> DepositarAsync(DepositoDto dto, int clienteId)
        {
            var conta = await _context.Contas.FindAsync(dto.ContaId)
                ?? throw new KeyNotFoundException("Conta não encontrada.");

            if (conta.ClienteId != clienteId)
                throw new UnauthorizedAccessException("Acesso negado.");

            conta.Saldo += dto.Valor;

            var transacao = new Transacao { Tipo = "Deposito", Valor = dto.Valor, ContaOrigemId = conta.Id };
            _context.Transacoes.Add(transacao);
            await _context.SaveChangesAsync();
            return MapToDto(transacao);
        }

        public async Task<TransacaoResponseDto> SacarAsync(SaqueDto dto, int clienteId)
        {
            var conta = await _context.Contas.FindAsync(dto.ContaId)
                ?? throw new KeyNotFoundException("Conta não encontrada.");

            if (conta.ClienteId != clienteId)
                throw new UnauthorizedAccessException("Acesso negado.");

            var totalDebito = dto.Valor + conta.TaxaSaque;
            if (conta.Saldo < totalDebito)
                throw new InvalidOperationException($"Saldo insuficiente. Saldo: R${conta.Saldo:F2}, necessário: R${totalDebito:F2}.");

            conta.Saldo -= totalDebito;

            var transacao = new Transacao { Tipo = "Saque", Valor = dto.Valor, ContaOrigemId = conta.Id };
            _context.Transacoes.Add(transacao);
            await _context.SaveChangesAsync();
            return MapToDto(transacao);
        }

        public async Task<TransacaoResponseDto> PixAsync(PixDto dto, int clienteId)
        {
            var origem = await _context.Contas.FindAsync(dto.ContaOrigemId)
                ?? throw new KeyNotFoundException("Conta de origem não encontrada.");

            if (origem.ClienteId != clienteId)
                throw new UnauthorizedAccessException("Acesso negado.");

            var destino = await _context.Contas.FirstOrDefaultAsync(c => c.NumeroConta == dto.NumeroContaDestino)
                ?? throw new KeyNotFoundException("Conta de destino não encontrada.");

            if (origem.Id == destino.Id)
                throw new InvalidOperationException("Não é possível fazer PIX para a mesma conta.");

            if (origem.Saldo < dto.Valor)
                throw new InvalidOperationException($"Saldo insuficiente. Saldo: R${origem.Saldo:F2}.");

            origem.Saldo -= dto.Valor;
            destino.Saldo += dto.Valor;

            var transacao = new Transacao { Tipo = "Pix", Valor = dto.Valor, ContaOrigemId = origem.Id, ContaDestinoId = destino.Id };
            _context.Transacoes.Add(transacao);
            await _context.SaveChangesAsync();
            return MapToDto(transacao, destino.NumeroConta);
        }

        public async Task<List<TransacaoResponseDto>> GetExtratoAsync(int contaId, int clienteId)
        {
            var conta = await _context.Contas.FindAsync(contaId)
                ?? throw new KeyNotFoundException("Conta não encontrada.");

            if (conta.ClienteId != clienteId)
                throw new UnauthorizedAccessException("Acesso negado.");

            return await _context.Transacoes
                .Include(t => t.ContaDestino)
                .Where(t => t.ContaOrigemId == contaId || t.ContaDestinoId == contaId)
                .OrderByDescending(t => t.DataHora)
                .Select(t => MapToDto(t, t.ContaDestino!.NumeroConta))
                .ToListAsync();
        }

        private static TransacaoResponseDto MapToDto(Transacao t, string? contaDestino = null) => new()
        {
            Id = t.Id,
            Tipo = t.Tipo,
            Valor = t.Valor,
            DataHora = t.DataHora,
            ContaDestino = contaDestino
        };
    }
}

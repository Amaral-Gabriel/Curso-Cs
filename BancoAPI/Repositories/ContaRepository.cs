using BancoAPI.Data;
using BancoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BancoAPI.Repositories
{
    public class ContaRepository : IContaRepository
    {
        private readonly BancoContext _context;

        public ContaRepository(BancoContext context)
        {
            _context = context;
        }

        public async Task<List<Conta>> GetByClienteIdAsync(int clienteId)
            => await _context.Contas.Where(c => c.ClienteId == clienteId).ToListAsync();

        public async Task<Conta?> GetByIdAsync(int id)
            => await _context.Contas.Include(c => c.Cliente).FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Conta?> GetByNumeroAsync(string numeroConta)
            => await _context.Contas.FirstOrDefaultAsync(c => c.NumeroConta == numeroConta);

        public async Task<Conta> CreateAsync(Conta conta)
        {
            _context.Contas.Add(conta);
            await _context.SaveChangesAsync();
            return conta;
        }

        public async Task<Conta> UpdateAsync(Conta conta)
        {
            _context.Contas.Update(conta);
            await _context.SaveChangesAsync();
            return conta;
        }

        public async Task DeleteAsync(Conta conta)
        {
            _context.Contas.Remove(conta);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Transacao>> GetTransacoesAsync(int contaId)
            => await _context.Transacoes
                .Include(t => t.ContaDestino)
                .Where(t => t.ContaOrigemId == contaId || t.ContaDestinoId == contaId)
                .OrderByDescending(t => t.DataHora)
                .ToListAsync();
    }
}

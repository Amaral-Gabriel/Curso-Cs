using BancoAPI.Models;

namespace BancoAPI.Repositories
{
    public interface IContaRepository
    {
        Task<List<Conta>> GetByClienteIdAsync(int clienteId);
        Task<Conta?> GetByIdAsync(int id);
        Task<Conta?> GetByNumeroAsync(string numeroConta);
        Task<Conta> CreateAsync(Conta conta);
        Task<Conta> UpdateAsync(Conta conta);
        Task DeleteAsync(Conta conta);
        Task<List<Transacao>> GetTransacoesAsync(int contaId);
    }
}

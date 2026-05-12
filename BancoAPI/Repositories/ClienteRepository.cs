using BancoAPI.Data;
using BancoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BancoAPI.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly BancoContext _context;

        public ClienteRepository(BancoContext context)
        {
            _context = context;
        }

        public async Task<List<Cliente>> GetAllAsync()
            => await _context.Clientes.Include(c => c.Contas).ToListAsync();

        public async Task<Cliente?> GetByIdAsync(int id)
            => await _context.Clientes.Include(c => c.Contas).FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Cliente?> GetByEmailAsync(string email)
            => await _context.Clientes.FirstOrDefaultAsync(c => c.Email == email);

        public async Task<Cliente> CreateAsync(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<Cliente> UpdateAsync(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task DeleteAsync(Cliente cliente)
        {
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
        }
    }
}

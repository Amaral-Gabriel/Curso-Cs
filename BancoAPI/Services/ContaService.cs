using BancoAPI.DTOs;
using BancoAPI.Models;
using BancoAPI.Repositories;

namespace BancoAPI.Services
{
    public class ContaService
    {
        private readonly IContaRepository _contaRepo;

        public ContaService(IContaRepository contaRepo)
        {
            _contaRepo = contaRepo;
        }

        public async Task<List<ContaResponseDto>> GetByClienteIdAsync(int clienteId)
        {
            var contas = await _contaRepo.GetByClienteIdAsync(clienteId);
            var resultado = new List<ContaResponseDto>();
            foreach (var conta in contas)
            {
                await AplicarJurosSePoupancaAsync(conta);
                resultado.Add(await MapToDtoAsync(conta));
            }
            return resultado;
        }

        public async Task<ContaResponseDto> GetByIdAsync(int id, int clienteId)
        {
            var conta = await _contaRepo.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Conta não encontrada.");

            if (conta.ClienteId != clienteId)
                throw new UnauthorizedAccessException("Acesso negado.");

            await AplicarJurosSePoupancaAsync(conta);
            return await MapToDtoAsync(conta);
        }

        public async Task<ContaResponseDto> AbrirContaAsync(AbrirContaDto dto, int clienteId)
        {
            if (dto.Tipo != "Corrente" && dto.Tipo != "Poupanca")
                throw new ArgumentException("Tipo de conta inválido. Use 'Corrente' ou 'Poupanca'.");

            var conta = new Conta
            {
                NumeroConta = await GerarNumeroUnicoAsync(),
                Tipo = dto.Tipo,
                Saldo = 0,
                TaxaSaque = dto.Tipo == "Corrente" ? 2.50m : 0,
                TaxaJuros = dto.Tipo == "Poupanca" ? 0.005m : 0,
                UltimoRendimento = DateTime.Today,
                ClienteId = clienteId
            };

            await _contaRepo.CreateAsync(conta);
            return await MapToDtoAsync(conta);
        }

        public async Task EncerrarContaAsync(int id, int clienteId)
        {
            var conta = await _contaRepo.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Conta não encontrada.");

            if (conta.ClienteId != clienteId)
                throw new UnauthorizedAccessException("Acesso negado.");

            if (conta.Saldo > 0)
                throw new InvalidOperationException("Encerre o saldo antes de fechar a conta.");

            await _contaRepo.DeleteAsync(conta);
        }

        private async Task AplicarJurosSePoupancaAsync(Conta conta)
        {
            if (conta.Tipo == "Poupanca" && conta.UltimoRendimento.Date < DateTime.Today)
            {
                var dias = (DateTime.Today - conta.UltimoRendimento.Date).Days;
                for (int i = 0; i < dias; i++)
                    conta.Saldo *= (1 + conta.TaxaJuros);

                conta.UltimoRendimento = DateTime.Today;
                await _contaRepo.UpdateAsync(conta);
            }
        }

        private async Task<ContaResponseDto> MapToDtoAsync(Conta conta)
        {
            var transacoes = await _contaRepo.GetTransacoesAsync(conta.Id);
            return new ContaResponseDto
            {
                Id = conta.Id,
                NumeroConta = conta.NumeroConta,
                Tipo = conta.Tipo,
                Saldo = conta.Saldo,
                TaxaSaque = conta.TaxaSaque,
                TaxaJuros = conta.TaxaJuros,
                TitularNome = conta.Cliente?.Nome ?? "",
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
            var rnd = new Random();
            string numero;
            do
            {
                numero = rnd.Next(100000, 999999).ToString();
            } while (await _contaRepo.GetByNumeroAsync(numero) != null);
            return numero;
        }
    }
}

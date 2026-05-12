using System.ComponentModel.DataAnnotations;

namespace BancoAPI.DTOs
{
    public class AbrirContaDto
    {
        [Required]
        public string Tipo { get; set; } = string.Empty; // "Corrente" ou "Poupanca"
    }

    public class ContaResponseDto
    {
        public int Id { get; set; }
        public string NumeroConta { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
        public decimal TaxaSaque { get; set; }
        public decimal TaxaJuros { get; set; }
        public string TitularNome { get; set; } = string.Empty;
        public List<TransacaoResponseDto> Transacoes { get; set; } = new();
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoAPI.Models
{
    public class Conta
    {
        public int Id { get; set; }

        [Required]
        public string NumeroConta { get; set; } = string.Empty;

        [Required]
        public string Tipo { get; set; } = string.Empty; // "Corrente" ou "Poupanca"

        [Column(TypeName = "decimal(18,2)")]
        public decimal Saldo { get; set; } = 0;

        // Apenas ContaCorrente usa TaxaSaque (R$2,50)
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxaSaque { get; set; } = 0;

        // Apenas ContaPoupanca usa TaxaJuros (0,5% ao dia = 0.005)
        [Column(TypeName = "decimal(18,4)")]
        public decimal TaxaJuros { get; set; } = 0;

        public DateTime UltimoRendimento { get; set; } = DateTime.Today;

        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        public List<Transacao> TransacoesOrigem { get; set; } = new();
        public List<Transacao> TransacoesDestino { get; set; } = new();
    }
}

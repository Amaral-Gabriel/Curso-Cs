using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoAPI.Models
{
    public class Transacao
    {
        public int Id { get; set; }

        [Required]
        public string Tipo { get; set; } = string.Empty; // "Deposito", "Saque", "Pix"

        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }

        public DateTime DataHora { get; set; } = DateTime.Now;

        public int ContaOrigemId { get; set; }
        public Conta? ContaOrigem { get; set; }

        public int? ContaDestinoId { get; set; }
        public Conta? ContaDestino { get; set; }
    }
}

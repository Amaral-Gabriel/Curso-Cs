using System.ComponentModel.DataAnnotations;

namespace BancoAPI.DTOs
{
    public class DepositoDto
    {
        [Required]
        public int ContaId { get; set; }

        [Required, Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
        public decimal Valor { get; set; }
    }

    public class SaqueDto
    {
        [Required]
        public int ContaId { get; set; }

        [Required, Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
        public decimal Valor { get; set; }
    }

    public class PixDto
    {
        [Required]
        public int ContaOrigemId { get; set; }

        [Required]
        public string NumeroContaDestino { get; set; } = string.Empty;

        [Required, Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
        public decimal Valor { get; set; }
    }

    public class TransacaoResponseDto
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataHora { get; set; }
        public string? ContaDestino { get; set; }
    }
}

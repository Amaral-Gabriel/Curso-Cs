using System.ComponentModel.DataAnnotations;

namespace BancoAPI.DTOs
{
    public class ClienteResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Perfil { get; set; } = string.Empty;
        public List<ContaResumoDto> Contas { get; set; } = new();
    }

    public class ClienteUpdateDto
    {
        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
    }

    public class ContaResumoDto
    {
        public int Id { get; set; }
        public string NumeroConta { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
    }
}

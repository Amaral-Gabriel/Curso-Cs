using System.ComponentModel.DataAnnotations;

namespace BancoAPI.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string SenhaHash { get; set; } = string.Empty;

        public string Perfil { get; set; } = "Cliente"; // "Cliente" ou "Admin"

        public List<Conta> Contas { get; set; } = new();
    }
}

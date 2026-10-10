using System.ComponentModel.DataAnnotations;

namespace sistema_gerenciamento_tarefas.DTOs.Usuarias.Requests
{
    public class UsuariaCadastroRequest
    {
        [StringLength(80, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 50 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Senha { get; set; } = string.Empty;
    }
}
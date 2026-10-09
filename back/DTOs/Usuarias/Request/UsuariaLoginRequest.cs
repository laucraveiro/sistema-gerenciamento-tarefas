namespace sistema_gerenciamento_tarefas.DTOs.Usuarias.Requests
{
    public class UsuarioLoginRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}
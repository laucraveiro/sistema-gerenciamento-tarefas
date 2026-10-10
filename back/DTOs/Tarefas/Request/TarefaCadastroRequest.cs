namespace sistema_gerenciamento_tarefas.DTOs.Tarefas.Request
{
    public class TarefaCadastroRequest
    {
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public DateTime DataVencimento { get; set; }
    }
}

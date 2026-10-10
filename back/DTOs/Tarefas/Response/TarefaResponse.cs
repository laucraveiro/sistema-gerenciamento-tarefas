using sistema_gerenciamento_tarefas.Models.Enums;

namespace sistema_gerenciamento_tarefas.DTOs.Tarefas.Responses
{
    public class TarefaResponse
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataVencimento { get; set; }
        public string Status { get; set; } = string.Empty;
        public int UsuariaId { get; set; }
    }
}
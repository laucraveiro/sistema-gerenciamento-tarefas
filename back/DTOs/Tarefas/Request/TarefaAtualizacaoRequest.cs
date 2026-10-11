using sistema_gerenciamento_tarefas.Models.Enums;

namespace sistema_gerenciamento_tarefas.DTOs.Tarefas.Requests
{
    public class TarefaAtualizacaoRequest
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataVencimento { get; set; }
    }
}
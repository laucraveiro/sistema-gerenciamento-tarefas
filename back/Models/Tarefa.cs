using sistema_gerenciamento_tarefas.Models.Enums;

namespace sistema_gerenciamento_tarefas.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataVencimento { get; set; }
        public StatusTarefa Status { get; set; } = StatusTarefa.Pendente;
        public int UsuariaId { get; set; }
    }
}

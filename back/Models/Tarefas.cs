namespace sistema_gerenciamento_tarefas.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataVencimento { get; set; }
        public string Status { get; set; } = "Pendente";
        public int UsuariaId { get; set; }
    }
}

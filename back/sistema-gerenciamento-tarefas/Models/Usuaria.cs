namespace sistema_gerenciamento_tarefas.Models
{
    public class Usuaria
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get;  } = "12345";
    }
}

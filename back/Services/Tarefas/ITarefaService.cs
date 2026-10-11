using sistema_gerenciamento_tarefas.Models;
using sistema_gerenciamento_tarefas.Models.Enums;

namespace sistema_gerenciamento_tarefas.Services.Tarefas
{
    public interface ITarefaService
    {
        Task<Tarefa> CriarTarefa(string titulo, string descricao, DateTime dataVencimento, int usuariaId);
        Task<IEnumerable<Tarefa>> ListarTarefasPorIdUsuaria(int usuariaId);
        Task<Tarefa> BuscarTarefaPorId(int usuariaId, int tarefaId);
        Task<Tarefa> AtualizarTarefa(int tarefaId, string titulo, string descricao, DateTime dataVencimento, int usuariaId);
        Task<Tarefa> AtualizarStatus(int tarefaId, StatusTarefa status, int usuariaId);
        Task DeletarTarefa(int usuariaId, int tarefaId);
    }
}
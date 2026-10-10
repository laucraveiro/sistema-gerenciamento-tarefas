using sistema_gerenciamento_tarefas.Models;

namespace sistema_gerenciamento_tarefas.Data.Repositories.Tarefas
{
    public interface ITarefaRepository
    {
        Task<List<Tarefa>> BuscarPorUsuariaAsync(int usuariaId);

        Task<Tarefa?> BuscarPorIdAsync(
            int usuariaId,
            int tarefaId
        );

        Task<Tarefa> AdicionarAsync(Tarefa tarefa);

        Task<Tarefa> AtualizarAsync(Tarefa tarefa);

        Task RemoverAsync(Tarefa tarefa);
    }
}
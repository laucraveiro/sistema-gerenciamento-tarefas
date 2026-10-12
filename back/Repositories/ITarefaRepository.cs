using sistema_gerenciamento_tarefas.Models;

namespace sistema_gerenciamento_tarefas.Repositories;

public interface ITarefaRepository
{
    List<Tarefa> BuscarPorUsuaria(int usuariaId);

    Tarefa? BuscarPorId(int usuariaId, int tarefaId);

    Tarefa Adicionar(Tarefa tarefa);

    Tarefa Atualizar(Tarefa tarefa);

    void Remover(Tarefa tarefa);
}
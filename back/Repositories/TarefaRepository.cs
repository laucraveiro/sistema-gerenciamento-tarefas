using sistema_gerenciamento_tarefas.Data;
using sistema_gerenciamento_tarefas.Models;

namespace sistema_gerenciamento_tarefas.Repositories;

public class TarefaRepository : ITarefaRepository
{
    private readonly AppDbContext _context;

    public TarefaRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<Tarefa> BuscarPorUsuaria(int usuariaId)
    {
        if (usuariaId <= 0) return new List<Tarefa>();

        return _context.Tarefas
            .Where(tarefa => tarefa.UsuariaId == usuariaId)
            .ToList();
    }

    public Tarefa? BuscarPorId(int usuariaId, int tarefaId)
    {
        if (usuariaId <= 0 || tarefaId <= 0) return null;

        return _context.Tarefas
            .FirstOrDefault(tarefa =>
                tarefa.Id == tarefaId &&
                tarefa.UsuariaId == usuariaId
            );
    }

    public Tarefa Adicionar(Tarefa tarefa)
    {
        ArgumentNullException.ThrowIfNull(tarefa);

        if (tarefa.UsuariaId <= 0)
        {
            throw new ArgumentException(
                "A tarefa precisa estar associada a uma usuária válida."
            );
        }

        var usuariaExiste = _context.Usuarias
            .Any(usuaria => usuaria.Id == tarefa.UsuariaId);

        if (!usuariaExiste)
        {
            throw new InvalidOperationException(
                "A usuária informada não existe."
            );
        }

        _context.Tarefas.Add(tarefa);
        _context.SaveChanges();

        return tarefa;
    }

    public Tarefa Atualizar(Tarefa tarefa)
    {
        ArgumentNullException.ThrowIfNull(tarefa);

        _context.Tarefas.Update(tarefa);
        _context.SaveChanges();

        return tarefa;
    }

    public void Remover(Tarefa tarefa)
    {
        ArgumentNullException.ThrowIfNull(tarefa);

        _context.Tarefas.Remove(tarefa);
        _context.SaveChanges();
    }
}
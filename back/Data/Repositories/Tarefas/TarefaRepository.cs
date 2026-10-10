using Microsoft.EntityFrameworkCore;
using sistema_gerenciamento_tarefas.Models;

namespace sistema_gerenciamento_tarefas.Data.Repositories.Tarefas
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly AppDbContext _context;

        public TarefaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tarefa>> BuscarPorUsuariaAsync(int usuariaId)
        {
            if (usuariaId <= 0) return new List<Tarefa>();

            return await _context.Tarefas
                .Where(tarefa => tarefa.UsuariaId == usuariaId)
                .ToListAsync();
        }

        public async Task<Tarefa?> BuscarPorIdAsync(
            int usuariaId,
            int tarefaId
        )
        {
            if (usuariaId <= 0 || tarefaId <= 0) return null;

            return await _context.Tarefas
                .FirstOrDefaultAsync(tarefa =>
                    tarefa.Id == tarefaId &&
                    tarefa.UsuariaId == usuariaId
                );
        }

        public async Task<Tarefa> AdicionarAsync(Tarefa tarefa)
        {
            ArgumentNullException.ThrowIfNull(tarefa);

            var usuariaExiste = await _context.Usuarias
                .AnyAsync(usuaria =>
                    usuaria.Id == tarefa.UsuariaId
                );

            if (!usuariaExiste)
            {
                throw new InvalidOperationException(
                    "A usuária informada não existe."
                );
            }

            await _context.Tarefas.AddAsync(tarefa);
            await _context.SaveChangesAsync();

            return tarefa;
        }

        public async Task<Tarefa> AtualizarAsync(Tarefa tarefa)
        {
            ArgumentNullException.ThrowIfNull(tarefa);

            _context.Tarefas.Update(tarefa);
            await _context.SaveChangesAsync();

            return tarefa;
        }

        public async Task RemoverAsync(Tarefa tarefa)
        {
            ArgumentNullException.ThrowIfNull(tarefa);

            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();
        }
    }
}
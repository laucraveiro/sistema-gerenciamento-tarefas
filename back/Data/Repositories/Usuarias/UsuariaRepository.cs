using Microsoft.EntityFrameworkCore;
using sistema_gerenciamento_tarefas.Models;

namespace sistema_gerenciamento_tarefas.Data.Repositories.Usuarias
{
    public class UsuariaRepository : IUsuariaRepository
    {
        private readonly AppDbContext _context;

        public UsuariaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuaria> CadastrarAsync(Usuaria usuaria)
        {
            ArgumentNullException.ThrowIfNull(usuaria);

            await _context.Usuarias.AddAsync(usuaria);
            await _context.SaveChangesAsync();

            return usuaria;
        }

        public async Task<Usuaria?> BuscarPorIdAsync(int id)
        {
            if (id <= 0)
            {
                return null;
            }

            return await _context.Usuarias
                .FirstOrDefaultAsync(usuaria => usuaria.Id == id);
        }

        public async Task<Usuaria?> BuscarPorNomeAsync(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                return null;
            }

            var nomeNormalizado = nome.Trim().ToLower();

            return await _context.Usuarias
                .FirstOrDefaultAsync(
                    usuaria => usuaria.Nome.ToLower() == nomeNormalizado
                );
        }

        public async Task<List<Usuaria>> ListarTodasAsync()
        {
            return await _context.Usuarias
                .ToListAsync();
        }
    }
}
using sistema_gerenciamento_tarefas.Data;
using sistema_gerenciamento_tarefas.Models;

namespace sistema_gerenciamento_tarefas.Repositories;

public class UsuariaRepository : IUsuariaRepository
{
    private readonly AppDbContext _context;

    public UsuariaRepository(AppDbContext context)
    {
        _context = context;
    }

    public Usuaria Adicionar(Usuaria usuaria)
    {
        // Impede que o método receba uma usuária nula
        ArgumentNullException.ThrowIfNull(usuaria);

        _context.Usuarias.Add(usuaria);
        _context.SaveChanges();

        return usuaria;
    }

    public Usuaria? BuscarPorId(int id)
    {
        if (id <= 0) return null;

        return _context.Usuarias
            .FirstOrDefault(usuaria => usuaria.Id == id);
    }

    public Usuaria? BuscarPorNome(string nome)
    {
        // Evita consulta com nome vazio
        if (string.IsNullOrWhiteSpace(nome)) return null;

        return _context.Usuarias
            .FirstOrDefault(usuaria => usuaria.Nome == nome);
    }
}
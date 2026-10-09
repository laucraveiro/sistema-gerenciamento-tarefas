using sistema_gerenciamento_tarefas.Models;

namespace sistema_gerenciamento_tarefas.Repositories;

public interface IUsuariaRepository
{
    Usuaria Adicionar(Usuaria usuaria);

    Usuaria? BuscarPorId(int id);

    Usuaria? BuscarPorNome(string nome);
}
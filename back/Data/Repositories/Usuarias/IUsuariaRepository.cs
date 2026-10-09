using sistema_gerenciamento_tarefas.Models;

namespace sistema_gerenciamento_tarefas.Data.Repositories.Usuarias
{
    public interface IUsuariaRepository
    {
        Task<Usuaria> CadastrarAsync(Usuaria usuaria);
        Task<Usuaria?> BuscarPorIdAsync(int id);
        Task<Usuaria?> BuscarPorNomeAsync(string nome);
        Task<List<Usuaria>> ListarTodasAsync(); // Adicionado para teste
    }
}

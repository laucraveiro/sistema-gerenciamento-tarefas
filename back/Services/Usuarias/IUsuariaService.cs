using sistema_gerenciamento_tarefas.Data.Repositories.Usuarias;
using sistema_gerenciamento_tarefas.Models;

namespace sistema_gerenciamento_tarefas.Services.Usuarias
{
    public interface IUsuariaService
    {
        Task<Usuaria> CadastrarUsuaria(string nome, string email, string senha);
        Task<Usuaria> LoginUsuaria(string email, string senha);
        Task<List<Usuaria>> ListarTodasUsuarias(); // Adicionado para teste
        Task<Usuaria?> BuscarPorIdAsync(int id);
    }
}

using sistema_gerenciamento_tarefas.Models;
using sistema_gerenciamento_tarefas.Data.Repositories.Usuarias;

namespace sistema_gerenciamento_tarefas.Data.Repositories.Usuarias
{
    // Repositório temporário para TESTES sem SQLite
    public class UsuariaRepositoryInMemory : IUsuariaRepository
    {
        private static readonly List<Usuaria> _usuarios = new();
        private static int _proximoId = 1;

        public Task<Usuaria> CadastrarAsync(Usuaria usuaria)
        {
            usuaria.Id = _proximoId++;
            _usuarios.Add(usuaria);
            return Task.FromResult(usuaria);
        }

        public Task<Usuaria?> BuscarPorNomeAsync(string nome)
        {
            var usuario = _usuarios.FirstOrDefault(u => u.Nome.ToLower() == nome.ToLower());
            return Task.FromResult(usuario);
        }

        public Task<Usuaria?> BuscarPorIdAsync(int id)
        {
            var usuario = _usuarios.FirstOrDefault(u => u.Id == id);
            return Task.FromResult(usuario);
        }
        public Task<List<Usuaria>> ListarTodasAsync()
        {
            return Task.FromResult(_usuarios);
        }
    }
}
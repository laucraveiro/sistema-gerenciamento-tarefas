using sistema_gerenciamento_tarefas.Data.Repositories.Usuarias;
using sistema_gerenciamento_tarefas.Models;

namespace sistema_gerenciamento_tarefas.Services.Usuarias
{
    public class UsuariaService : IUsuariaService
    {
        private readonly IUsuariaRepository _usuariaRepository;

        public UsuariaService(IUsuariaRepository usuariaRepository)
        {
            _usuariaRepository = usuariaRepository;
        }

        public async Task<Usuaria> CadastrarUsuaria(string nome, string email, string senha)
        {
            if(string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Nome e e-mail são campos obrigatórios");
            }

            if(senha != "12345")
            {
                throw new InvalidOperationException("A senha deve ser igual a 12345");
            }

            //tratar o nome em tolower
            var nomeTratado = nome.Trim().ToLower();

            var usuariaExistente = await _usuariaRepository.BuscarPorNomeAsync(nomeTratado);
            if (usuariaExistente != null)
            {
                throw new InvalidOperationException("Já existe uma usuária cadastrada com este nome.");
            }

            var novaUsuaria = new Usuaria
            {
                Nome = nomeTratado,
                Email = email.Trim(),
                //SENHA AUTOMATICAMENTE VIRA 12345
            };

            return await _usuariaRepository.CadastrarAsync(novaUsuaria);

        }

        public async Task<List<Usuaria>> ListarTodasUsuarias()
        {
            return await _usuariaRepository.ListarTodasAsync();
        }

        public async Task<Usuaria> LoginUsuaria(string nome, string senha)
        {
            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(senha))
            {
                throw new ArgumentException("Nome e Senha são obrigatórios.");
            }

            string nomeTratado = nome.Trim().ToLower();

            var usuaria = await _usuariaRepository.BuscarPorNomeAsync(nomeTratado);

            if (usuaria == null || usuaria.Senha != senha)
            {
                throw new UnauthorizedAccessException("Nome de usuária ou senha inválidos.");
            }

            return usuaria; // Retorna a usuária com o Id gerado pelo banco
        }

        public async Task<Usuaria?> BuscarPorIdAsync(int id)
        {
            return await _usuariaRepository.BuscarPorIdAsync(id);
        }
    }
}

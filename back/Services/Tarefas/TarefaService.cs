using sistema_gerenciamento_tarefas.Data.Repositories.Tarefas;
using sistema_gerenciamento_tarefas.Models;
using sistema_gerenciamento_tarefas.Models.Enums;

namespace sistema_gerenciamento_tarefas.Services.Tarefas
{
    public class TarefaService : ITarefaService
    {
        private readonly ITarefaRepository _tarefaRepository;

        public TarefaService(ITarefaRepository tarefaRepository)
        {
            _tarefaRepository = tarefaRepository;
        }

        public async Task<Tarefa> CriarTarefa(string titulo, string descricao, DateTime dataVencimento, int usuariaId)
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                throw new ArgumentException("O título da tarefa é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(descricao))
            {
                throw new ArgumentException("A descrição é obrigatória");
            }

            if (usuariaId <= 0)
            {
                throw new ArgumentException("ID da usuária inválido.");
            }

            var novaTarefa = new Tarefa
            {
                Titulo = titulo.Trim(),
                Descricao = descricao?.Trim() ?? string.Empty,
                DataVencimento = dataVencimento,
                UsuariaId = usuariaId,
            };

            // O repositório já valida se a usuária existe e joga exceção se não existir
            return await _tarefaRepository.AdicionarAsync(novaTarefa);
        }

        public async Task<IEnumerable<Tarefa>> ListarTarefasPorIdUsuaria(int usuariaId)
        {
            if (usuariaId <= 0)
            {
                throw new ArgumentException("ID da usuária inválido.");
            }

            return await _tarefaRepository.BuscarPorUsuariaAsync(usuariaId);
        }

        public async Task<Tarefa> BuscarTarefaPorId(int usuariaId, int tarefaId)
        {
            if (usuariaId <= 0 || tarefaId <= 0)
            {
                throw new ArgumentException("IDs inválidos.");
            }

            var tarefa = await _tarefaRepository.BuscarPorIdAsync(usuariaId, tarefaId);
            if (tarefa == null)
            {
                throw new KeyNotFoundException("Tarefa não encontrada.");
            }

            return tarefa;
        }

        public async Task<Tarefa> AtualizarTarefa(int tarefaId, string titulo, string descricao, DateTime dataVencimento, int usuariaId)
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                throw new ArgumentException("O título da tarefa é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(descricao))
            {
                throw new ArgumentException("A descrição da tarefa é obrigatório.");
            }

            var tarefaExistente = await _tarefaRepository.BuscarPorIdAsync(usuariaId, tarefaId);

            if (tarefaExistente == null)
            {
                throw new KeyNotFoundException("Tarefa não encontrada para esta usuária.");
            }

            tarefaExistente.Titulo = titulo.Trim();
            tarefaExistente.Descricao = descricao?.Trim() ?? string.Empty;
            tarefaExistente.DataVencimento = dataVencimento;

            return await _tarefaRepository.AtualizarAsync(tarefaExistente);
        }

        public async Task<Tarefa> AtualizarStatus(int tarefaId, StatusTarefa status, int usuariaId)
        {
            var tarefaExistente = await _tarefaRepository.BuscarPorIdAsync(usuariaId, tarefaId);

            if (tarefaExistente == null)
            {
                throw new KeyNotFoundException("Tarefa não encontrada para esta usuária.");
            }

            tarefaExistente.Status = status;

            return await _tarefaRepository.AtualizarAsync(tarefaExistente);
        }

        public async Task DeletarTarefa(int usuariaId, int tarefaId)
        {
            var tarefaExistente = await _tarefaRepository.BuscarPorIdAsync(usuariaId, tarefaId);

            if (tarefaExistente == null)
            {
                throw new KeyNotFoundException("Tarefa não encontrada para esta usuária.");
            }

            await _tarefaRepository.RemoverAsync(tarefaExistente);
        }
    }
}
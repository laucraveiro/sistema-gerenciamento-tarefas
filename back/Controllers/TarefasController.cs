using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sistema_gerenciamento_tarefas.DTOs.Comum;
using sistema_gerenciamento_tarefas.DTOs.Tarefas.Request;
using sistema_gerenciamento_tarefas.DTOs.Tarefas.Requests;
using sistema_gerenciamento_tarefas.DTOs.Tarefas.Responses;
using sistema_gerenciamento_tarefas.Models.Enums;
using sistema_gerenciamento_tarefas.Services.Tarefas;

namespace sistema_gerenciamento_tarefas.Controllers
{
    [Route("api/usuarios/{usuarioId}/tarefas")]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        private readonly ITarefaService _tarefaService;

        public TarefasController(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(TarefaResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CriarTarefa(int usuarioId, [FromBody] TarefaCadastroRequest request)
        {
            try
            {
                var tarefa = await _tarefaService.CriarTarefa(
                    request.Titulo,
                    request.Descricao,
                    request.DataVencimento,
                    usuarioId
                );

                var response = new TarefaResponse
                {
                    Id = tarefa.Id,
                    Titulo = tarefa.Titulo,
                    Descricao = tarefa.Descricao,
                    DataVencimento = tarefa.DataVencimento,
                    Status = tarefa.Status.ToString(),
                    UsuariaId = tarefa.UsuariaId
                };

                return CreatedAtAction(nameof(BuscarPorId), new { usuarioId = tarefa.UsuariaId, id = tarefa.Id }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TarefaResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListarTarefasPorUsuaria(int usuarioId)
        {
            try
            {
                var tarefas = await _tarefaService.ListarTarefasPorIdUsuaria(usuarioId);

                var response = tarefas.Select(t => new TarefaResponse
                {
                    Id = t.Id,
                    Titulo = t.Titulo,
                    Descricao = t.Descricao,
                    DataVencimento = t.DataVencimento,
                    Status = t.Status.ToString(),
                    UsuariaId = t.UsuariaId
                });

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(TarefaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BuscarPorId(int usuarioId, int id)
        {
            try
            {
                var tarefa = await _tarefaService.BuscarTarefaPorId(usuarioId, id);

                var response = new TarefaResponse
                {
                    Id = tarefa.Id,
                    Titulo = tarefa.Titulo,
                    Descricao = tarefa.Descricao,
                    DataVencimento = tarefa.DataVencimento,
                    Status = tarefa.Status.ToString(),
                    UsuariaId = tarefa.UsuariaId
                };

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(TarefaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AtualizarTarefa(int usuarioId, int id, [FromBody] TarefaAtualizacaoRequest request)
        {
            try
            {
                var tarefaAtualizada = await _tarefaService.AtualizarTarefa(
                    id,
                    request.Titulo,
                    request.Descricao,
                    request.DataVencimento,
                    usuarioId
                );

                var response = new TarefaResponse
                {
                    Id = tarefaAtualizada.Id,
                    Titulo = tarefaAtualizada.Titulo,
                    Descricao = tarefaAtualizada.Descricao,
                    DataVencimento = tarefaAtualizada.DataVencimento,
                    Status = tarefaAtualizada.Status.ToString(),
                    UsuariaId = tarefaAtualizada.UsuariaId
                };

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        [HttpPut("{id:int}/concluir")]
        [ProducesResponseType(typeof(TarefaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConcluirTarefa(int usuarioId, int id)
        {
            try
            {
                // Altera diretamente o status para Concluida, validando o usuário e a tarefa
                var tarefaAtualizada = await _tarefaService.AtualizarStatus(id, StatusTarefa.Concluida, usuarioId);

                var response = new TarefaResponse
                {
                    Id = tarefaAtualizada.Id,
                    Titulo = tarefaAtualizada.Titulo,
                    Descricao = tarefaAtualizada.Descricao,
                    DataVencimento = tarefaAtualizada.DataVencimento,
                    Status = tarefaAtualizada.Status.ToString(),
                    UsuariaId = tarefaAtualizada.UsuariaId
                };

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletarTarefa(int usuarioId, int id)
        {
            try
            {
                await _tarefaService.DeletarTarefa(usuarioId, id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }
    }
}
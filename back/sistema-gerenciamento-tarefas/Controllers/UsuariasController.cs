using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sistema_gerenciamento_tarefas.DTOs.Comum;
using sistema_gerenciamento_tarefas.DTOs.Usuarias.Requests;
using sistema_gerenciamento_tarefas.DTOs.Usuarios.Responses;
using sistema_gerenciamento_tarefas.Services.Usuarias;

namespace sistema_gerenciamento_tarefas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariasController : ControllerBase
    {
        private readonly IUsuariaService _usuariaService;

        public UsuariasController(IUsuariaService usuariaService)
        {
            _usuariaService = usuariaService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(UsuarioResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CadastrarUsuaria([FromBody] UsuariaCadastroRequest request)
        {
            try
            {
                var usuaria = await _usuariaService.CadastrarUsuaria(request.Nome, request.Email, request.Senha);

                // Retorna 201 Created contendo os dados da usuária criada (incluindo o Id)
                return CreatedAtAction(nameof(CadastrarUsuaria), new { id = usuaria.Id }, new
                {
                    id = usuaria.Id,
                    nome = usuaria.Nome,
                    email = usuaria.Email
                });
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


        [HttpPost("login")]
        [ProducesResponseType(typeof(UsuarioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginRequest request)
        {
            try
            {
                var usuaria = await _usuariaService.LoginUsuaria(request.Nome, request.Senha);

                var response = new UsuarioResponse
                {
                    Id = usuaria.Id,
                    Nome = usuaria.Nome,
                    Email = usuaria.Email
                };

                // Retorna 200 OK com os dados da usuária (incluindo o Id) para o Angular guardar
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { mensagem = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }


        //*****PARA TESTES
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UsuarioResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListarUsuarias()
        {
            var usuarias = await _usuariaService.ListarTodasUsuarias();

            var response = usuarias.Select(u => new UsuarioResponse
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email
            });

            return Ok(response);
        }

        //*****PARA TESTES
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(UsuarioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            try
            {
                var usuaria = await _usuariaService.BuscarPorIdAsync(id);
                if (usuaria == null)
                {
                    return NotFound(new { mensagem = "Usuária não encontrada." });
                }

                var response = new UsuarioResponse
                {
                    Id = usuaria.Id,
                    Nome = usuaria.Nome,
                    Email = usuaria.Email
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}

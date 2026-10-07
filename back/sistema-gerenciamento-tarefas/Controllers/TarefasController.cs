using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace sistema_gerenciamento_tarefas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetTeste(int id)
        {
            return Ok();
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace sistema_gerenciamento_tarefas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariasController : ControllerBase
    {
        [HttpPost]
        public IActionResult RegistrarUsuaria()
        {
            return Ok();
        }
    }
}

using CaixaAPI.Services.Interfaces;
using CaixaAPI.Services.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CaixaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaixaController(IBoxService service) : ControllerBase
    {
        [HttpPost]
        public IActionResult Calcular([FromBody] PedidoInput input)
        {
            var resposta = service.Calcular(input);
            return Ok(resposta);
        }
    }
}

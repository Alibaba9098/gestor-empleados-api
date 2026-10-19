using Microsoft.AspNetCore.Mvc;
using MiWebAPI.Data;
using MiWebAPI.Models;

namespace MiWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvedorController : ControllerBase
    {
        private readonly ProvedorData _provedorData;

        public ProvedorController(ProvedorData provedorData)
        {
            _provedorData = provedorData;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<Provedor> lista = await _provedorData.Lista();
            return StatusCode(StatusCodes.Status200OK, lista);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> Obtener(long id)
        {
            Provedor? objeto = await _provedorData.Obtener(id);

            if (objeto == null)
                return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "Proveedor no encontrado" });

            return StatusCode(StatusCodes.Status200OK, objeto);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Provedor objeto)
        {
            bool respuesta = await _provedorData.Crear(objeto);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] Provedor objeto)
        {
            bool respuesta = await _provedorData.Editar(objeto);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Eliminar(long id)
        {
            bool respuesta = await _provedorData.Eliminar(id);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using MiWebAPI.Data;
using MiWebAPI.Models;

namespace MiWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipoComputoController : ControllerBase
    {
        private readonly EquipoComputoData _equipoComputoData;

        public EquipoComputoController(EquipoComputoData equipoComputoData)
        {
            _equipoComputoData = equipoComputoData;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<EquipoComputo> lista = await _equipoComputoData.Lista();
            return StatusCode(StatusCodes.Status200OK, lista);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> Obtener(long id)
        {
            EquipoComputo? objeto = await _equipoComputoData.Obtener(id);

            if (objeto == null)
                return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "Equipo de cómputo no encontrado" });

            return StatusCode(StatusCodes.Status200OK, objeto);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] EquipoComputo objeto)
        {
            bool respuesta = await _equipoComputoData.Crear(objeto);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] EquipoComputo objeto)
        {
            bool respuesta = await _equipoComputoData.Editar(objeto);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Eliminar(long id)
        {
            bool respuesta = await _equipoComputoData.Eliminar(id);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }
    }
}
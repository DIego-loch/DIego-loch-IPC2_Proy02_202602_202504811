using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto2.Servicios;

namespace Proyecto2.Controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class SistemaController : ControllerBase
    {
        private readonly SistemaLibreria _sistema;
        private readonly CargadorXML _cargador;

        public SistemaController(SistemaLibreria sistema, CargadorXML cargador)
        {
            _sistema = sistema;
            _cargador = cargador;
        }

        // POST /api/sistema/inicializar
        [HttpPost("inicializar")]
        public IActionResult Inicializar()
        {
            _sistema.Inicializar();
            return Content("{\"ok\":true,\"msg\":\"Sistema reiniciado\"}",
                           "application/json");
        }

        // POST /api/sistema/cargar-xml  (por ruta del servidor)
        [HttpPost("cargar-xml")]
        public IActionResult CargarXml([FromBody] RutaDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Ruta))
                return Content("{\"ok\":false,\"msg\":\"Ruta vacía\"}", "application/json");

            string msg = _cargador.Cargar(dto.Ruta);

            StringBuilder sb = new StringBuilder();
            sb.Append("{\"ok\":true,\"msg\":\"");
            sb.Append(EscaparJSON(msg));
            sb.Append("\"}");

            return Content(sb.ToString(), "application/json");
        }

        // POST /api/sistema/subir-xml  (archivo subido desde el navegador)
        [HttpPost("subir-xml")]
        public IActionResult SubirXml(IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
                return Content("{\"ok\":false,\"msg\":\"No se recibió archivo\"}",
                               "application/json");

            string msg;
            using (Stream stream = archivo.OpenReadStream())
            {
                // ✅ Delega al CargadorXML (misma lógica, sin duplicar)
                msg = _cargador.CargarDesdeStream(stream);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("{\"ok\":true,\"msg\":\"");
            sb.Append(EscaparJSON(msg));
            sb.Append("\"}");

            return Content(sb.ToString(), "application/json");
        }

        private string EscaparJSON(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s.Replace("\\", "\\\\")
                    .Replace("\"", "\\\"")
                    .Replace("\n", "\\n")
                    .Replace("\r", "\\r")
                    .Replace("\t", "\\t");
        }
    }

    public class RutaDTO
    {
        public string Ruta { get; set; }
    }
}

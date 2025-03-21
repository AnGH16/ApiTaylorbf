using Microsoft.AspNetCore.Mvc;
using ApiTaylorbf.Model;

namespace ApiTaylorbf.Controllers
{
    [ApiController]
    [Route("api/novios")]
    public class NovioController : ControllerBase
    {
        private const string ApiKeyHeader = "x-api-key";
        private const string ExpectedApiKey = "XOXO-GossipGirl"; // Cámbiala por tu clave real

        private static List<Novio> novios = new List<Novio> {
            new Novio {Id=1, Nombre="Harry Styles", Profesion="Cantante", Cancion="Style"},
            new Novio {Id=2, Nombre="Tom Hiddleston", Profesion="Actor", Cancion="Getaway Car"},
            new Novio {Id=3, Nombre="Taylor Lautner", Profesion="Actor", Cancion="Back To December"}
        };

        private bool EsApiKeyValida()
        {
            if (!Request.Headers.TryGetValue(ApiKeyHeader, out var apiKey) || apiKey != ExpectedApiKey)
                return false;
            return true;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Novio>> GetAll()
        {
            if (!EsApiKeyValida()) return Unauthorized("API Key inválida");
            return Ok(novios);
        }

        [HttpGet("{id}")]
        public ActionResult<Novio> GetById(int id)
        {
            if (!EsApiKeyValida()) return Unauthorized("API Key inválida");

            var novio = novios.FirstOrDefault(n => n.Id == id);
            return novio != null ? Ok(novio) : NotFound();
        }

        [HttpPost]
        public ActionResult<Novio> Crear(Novio novio)
        {
            if (!EsApiKeyValida()) return Unauthorized("API Key inválida");

            novio.Id = novios.Any() ? novios.Max(n => n.Id) + 1 : 1;
            novios.Add(novio);
            return CreatedAtAction(nameof(GetById), new { id = novio.Id }, novio);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Novio Upnovio)
        {
            if (!EsApiKeyValida()) return Unauthorized("API Key inválida");

            var novio = novios.FirstOrDefault(n => n.Id == id);
            if (novio == null) return NotFound();

            novio.Nombre = Upnovio.Nombre;
            novio.Profesion = Upnovio.Profesion;
            novio.Cancion = Upnovio.Cancion;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!EsApiKeyValida()) return Unauthorized("API Key inválida");

            var novio = novios.FirstOrDefault(n => n.Id == id);
            if (novio == null) return NotFound();

            novios.Remove(novio);
            return NoContent();
        }
    }
}


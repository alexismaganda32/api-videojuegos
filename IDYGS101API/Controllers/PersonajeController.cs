using Domain.Modelo;
using IDYGS101API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IDYGS101API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonajeController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public PersonajeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<Response<PersonajeResponse>> CrearPersonaje([FromBody] PersonajeResponse i)
        {
            Personaje personaje = new Personaje();

            personaje.Nombre = i.Nombre;
            personaje.Poder = i.Poder;
            personaje.Color = i.Color;
            personaje.FkGenero = i.FkGenero;

            _context.personajes.Add(personaje);
            await _context.SaveChangesAsync();


            return new Response<PersonajeResponse>("Ok", i);

        }

        [HttpGet]
        public async Task<Response<List<Personaje>>> GetPersonajes()
        {
            try
            {
                var response = await _context.personajes.Include(x => x.genero).ToListAsync();

                return new Response<List<Personaje>>("correcto", response);
            }
            catch (Exception ex) {
                string message = ex.Message;
                throw new Exception("surgio un errro" +message);
            }
        }

        [HttpPut]
        public async Task<Response<PersonajeResponse>> EditarPersonaje(int? id, [FromBody] PersonajeResponse i)
        {
            try
            {
                var response = _context.personajes.Find(id);

                response.Nombre = i.Nombre;
                response.Poder = i.Poder;
                response.Color = i.Color;
                response.FkGenero = i.FkGenero;

                _context.Entry(response).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return new Response<PersonajeResponse>("se modifico correctamente", i);

            }
            catch(Exception ex)
            {
                string message = ex.Message;
                throw new Exception("surgio un errro" + message);
            }
        }

        // DELETE: api/owo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            var documents = await _context.personajes.FindAsync(id);
            if (documents == null)
            {
                return NotFound();
            }

            _context.personajes.Remove(documents);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}

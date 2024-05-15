using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Examen.Entidades;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Examen.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimeController : ControllerBase
    {
        private readonly DbContext _context;

        public AnimeController(DbContext context)
        {
            _context = context;
        }

        // GET: api/Anime
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Anime>>> GetAnime()
        {
            var anime = await _context.Anime
           .Include(a => a.Tipo)
           .ToListAsync();

            var response = anime.Select(a => new Anime
            {
                AnimeID = a.AnimeID,
                NombreAnime = a.NombreAnime,
                FechaEstreno = a.FechaEstreno,
                Tipo = a.Tipo.Select(t => new Tipo
                {
                    TipoID = t.TipoID,
                    NombreTipo = t.NombreTipo,
                    TipoAudiencia = t.TipoAudiencia
                }).ToList()
            });

            return Ok(response);
            //return await _context.Anime.ToListAsync();
        }

        // GET: api/Anime/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Anime>> GetAnime(int id)
        {
            var anime = await _context.Anime
            .Include(a => a.Tipo)
            .FirstOrDefaultAsync(a => a.AnimeID == id);

            if (anime == null)
            {
                return NotFound(); // Retorna un código 404 si el anime no se encuentra
            }

            var response = new Anime
            {
                AnimeID = anime.AnimeID,
                NombreAnime = anime.NombreAnime,
                FechaEstreno = anime.FechaEstreno,
                Tipo = anime.Tipo.Select(t => new Tipo
                {
                    TipoID = t.TipoID,
                    NombreTipo = t.NombreTipo,
                    TipoAudiencia = t.TipoAudiencia
                }).ToList()
            };

            return Ok(response);
        }

        // PUT: api/Anime/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnime(int id, Anime anime)
        {
            if (id != anime.AnimeID)
            {
                return BadRequest();
            }

            _context.Entry(anime).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnimeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Anime
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Anime>> PostAnime(Anime anime)
        {
            _context.Anime.Add(anime);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnime", new { id = anime.AnimeID }, anime);
        }

        // DELETE: api/Anime/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnime(int id)
        {
            var anime = await _context.Anime.FindAsync(id);
            if (anime == null)
            {
                return NotFound();
            }

            _context.Anime.Remove(anime);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AnimeExists(int id)
        {
            return _context.Anime.Any(e => e.AnimeID == id);
        }
    }
}

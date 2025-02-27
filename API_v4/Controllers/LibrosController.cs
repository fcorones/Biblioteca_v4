using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_v4.Context;
using API_v4.Models;
using Microsoft.AspNetCore.Authorization;


//CONTROLLER FUNCIONANDO PARA LIBROS. 06-12-2024
namespace API_v4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LibrosController : ControllerBase
    {
        private readonly BibliotecaDbContext _context;

        public LibrosController(BibliotecaDbContext context)
        {
            _context = context;
        }


        //ESTE ANDA
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<object>>> GetLibro()
        {
            var libros = await _context.Libros
                .Include(l => l.Generos)        //Carga los géneros asociados a cada libro
                .Include(l => l.Autor)
                .Include(l => l.Editorial)
                .ToListAsync();

            var result = libros.Select(libro => new
            {
                libro.Id,
                libro.Titulo,
                libro.TituloEspaniol,
                libro.AnioDePublicacion,
                libro.ISBN,
                libro.PortadaUrl,
                libro.Descripcion,
                libro.BoolPrestado,
                libro.NumeroEdicion,
                libro.Eliminado,
                // Agregamos los emojis a las propiedades mapeadas
                NombreAutor = libro.Autor == null
                    ? null
                    : $"{libro.Autor.NombreAutor}{(libro.Autor.Eliminado ? " ❌" : "")}",
                NombreEditorial = libro.Editorial == null
                    ? null
                    : $"{libro.Editorial.NombreEditorial}{(libro.Editorial.Eliminado ? " ❌" : "")}",
                NombresGeneros = libro.Generos
                    .Select(g => $"{g.NombreGenero}{(g.Eliminado ? " ❌" : "")}")
                    .ToList()
            });

            return Ok(result);
        }

        // GET: api/Libros/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> GetLibro(int id)
        {
            var libro = await _context.Libros
                .Include(l => l.Generos)
                .Include(l => l.Autor)
                .Include(l => l.Editorial)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (libro == null)
            {
                return NotFound($"No se encontró un libro con el ID {id}.");
            }

            var result = new
            {
                libro.Id,
                libro.Titulo,
                libro.TituloEspaniol,
                libro.AnioDePublicacion,
                libro.ISBN,
                libro.PortadaUrl,
                libro.Descripcion,
                libro.BoolPrestado,
                libro.NumeroEdicion,
                libro.Eliminado,
                NombreAutor = libro.Autor?.NombreAutor,
                NombreEditorial = libro.Editorial?.NombreEditorial,
                NombresGeneros = libro.Generos.Select(g => g.NombreGenero).ToList() // Devuelve los nombres de los géneros
            };

            return Ok(result);
        }


        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> PostLibro(Libro libro)
        {
            // Buscar el autor por nombre
            var autor = await _context.Autores.FirstOrDefaultAsync(a => a.NombreAutor == libro.NombreAutor);
            if (autor == null)
            {
                return BadRequest($"No se encontró un autor con el nombre '{libro.NombreAutor}'.");
            }
            libro.AutorId = autor.Id;

            // Buscar la editorial por nombre
            var editorial = await _context.Editoriales.FirstOrDefaultAsync(e => e.NombreEditorial == libro.NombreEditorial);
            if (editorial == null)
            {
                return BadRequest($"No se encontró una editorial con el nombre '{libro.NombreEditorial}'.");
            }
            libro.EditorialId = editorial.Id;

            // Buscar géneros por nombres
            if (libro.NombresGeneros == null || !libro.NombresGeneros.Any())
            {
                return BadRequest("Debe proporcionar al menos un género.");
            }

            var generos = await _context.Generos
                .Where(g => libro.NombresGeneros.Contains(g.NombreGenero))
                .ToListAsync();

            if (generos.Count != libro.NombresGeneros.Count)
            {
                return BadRequest("Algunos géneros no se encontraron en la base de datos.");
            }

            libro.Generos = generos; //EF mapea id_libro con id_genero en Libros_x_Genero

            // Guardar el libro en la base de datos
            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLibro), new { id = libro.Id }, libro);
        }

        // PUT: api/Libros/5
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> PutLibro(int id, Libro libro)
        {
            if (id != libro.Id)
            {
                return BadRequest();
            }

            var existingLibro = await _context.Libros
                .Include(l => l.Generos)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (existingLibro == null)
            {
                return NotFound();
            }

            // Buscar el autor por nombre y asignar su ID
            if (!string.IsNullOrWhiteSpace(libro.NombreAutor))
            {
                var autor = await _context.Autores.FirstOrDefaultAsync(a => a.NombreAutor == libro.NombreAutor);
                if (autor == null)
                {
                    return BadRequest($"No se encontró un autor con el nombre '{libro.NombreAutor}'.");
                }
                existingLibro.AutorId = autor.Id;
            }

            // Buscar la editorial por nombre y asignar su ID
            if (!string.IsNullOrWhiteSpace(libro.NombreEditorial))
            {
                var editorial = await _context.Editoriales.FirstOrDefaultAsync(e => e.NombreEditorial == libro.NombreEditorial);
                if (editorial == null)
                {
                    return BadRequest($"No se encontró una editorial con el nombre '{libro.NombreEditorial}'.");
                }
                existingLibro.EditorialId = editorial.Id;
            }

            // Buscar géneros por nombres y asignarlos
            if (libro.NombresGeneros != null && libro.NombresGeneros.Any())
            {
                var generos = await _context.Generos
                    .Where(g => libro.NombresGeneros.Contains(g.NombreGenero))
                    .ToListAsync();

                if (generos.Count != libro.NombresGeneros.Count)
                {
                    return BadRequest("Algunos géneros no se encontraron en la base de datos.");
                }

                existingLibro.Generos = generos;
            }

            // Actualizar las demás propiedades
            existingLibro.Titulo = libro.Titulo;
            existingLibro.TituloEspaniol = libro.TituloEspaniol;
            existingLibro.AnioDePublicacion = libro.AnioDePublicacion;
            existingLibro.ISBN = libro.ISBN;
            existingLibro.PortadaUrl = libro.PortadaUrl;
            existingLibro.Descripcion = libro.Descripcion;
            existingLibro.BoolPrestado = libro.BoolPrestado;
            existingLibro.NumeroEdicion = libro.NumeroEdicion;
            existingLibro.Eliminado = libro.Eliminado;

            _context.Entry(existingLibro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibroExists(id))
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





        // DELETE: api/Libros/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteLibro(int id)
        {
            var libro = await _context.Libros
                .Include(l => l.Generos) // Incluir los géneros relacionados
                .FirstOrDefaultAsync(l => l.Id == id);

            if (libro == null)
            {
                return NotFound();
            }

            // Limpiar la relación con géneros antes de eliminar
            libro.Generos.Clear();
            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LibroExists(int id)
        {
            return _context.Libros.Any(e => e.Id == id);
        }
    }
}

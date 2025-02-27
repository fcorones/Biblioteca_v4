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
using System.Security.Claims;

namespace API_v4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PrestamosController : ControllerBase
    {
        private readonly BibliotecaDbContext _context;

        public PrestamosController(BibliotecaDbContext context)
        {
            _context = context;
        }

       

        [HttpGet]
        [Authorize(Roles = "ADMIN,USUARIO")]
        public async Task<IActionResult> GetPrestamos()
        {
            var prestamos = await _context.Prestamos
                .Include(p => p.Usuario)
                .Include(p => p.Libro)
                .Select(p => new
                {
                    p.Id,
                    UsuarioId = p.Usuario.Id,
                    UsuarioNombre = p.Usuario.Nombre, // Campo del nombre del usuario
                    LibroId = p.Libro.Id,
                    LibroNombre = p.Libro.Titulo,    // Campo del nombre del libro
                    LibroNombreEspaniol = p.Libro.TituloEspaniol,
                    NumeroEdicion = p.Libro.NumeroEdicion,
                    LibroEditorial = p.Libro.Editorial.NombreEditorial, // Información adicional para diferenciación
                    FechaPrestamo = p.FechaPrestamo,
                    FechaDevolucion = p.FechaDevolucion,
                    Eliminado = p.Eliminado,
                    Activo = p.Activo
                })
                .ToListAsync();

            return Ok(prestamos);
        }

        

        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN,USUARIO")] // Requiere autenticación //OJO CAPAZ QUE ES ALLOWANONYMOUS
        public async Task<IActionResult> GetPrestamo(int id)
        {
            // Buscar el préstamo con sus relaciones
            var prestamo = await _context.Prestamos
                .Include(p => p.Usuario) // Incluir información del Usuario
                .Include(p => p.Libro)   // Incluir información del Libro
                .ThenInclude(l => l.Editorial) // Incluir la Editorial del Libro
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prestamo == null)
            {
                return NotFound("El préstamo no existe.");
            }

            // Obtener el rol y el ID del usuario autenticado desde el token JWT
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            // Validar acceso basado en el rol
            if (userRole == "ADMIN")
            {
                // Si es ADMIN, devolver la información completa del préstamo
                return Ok(new
                {
                    prestamo.Id,
                    prestamo.FechaPrestamo,
                    prestamo.FechaDevolucion,
                    prestamo.Activo,
                    Eliminado = prestamo.Eliminado,
                    UsuarioId = prestamo.UsuarioId,
                    UsuarioNombre = prestamo.Usuario?.Nombre, // Nombre del usuario (si existe)
                    LibroId = prestamo.LibroId,
                    LibroTitulo = prestamo.Libro?.Titulo,     // Título del libro (si existe)
                    LibroEditorial = prestamo.Libro?.Editorial?.NombreEditorial // Nombre de la editorial
                });
            }
            else if (userRole == "USUARIO")
            {
                // Si es USUARIO, verificar que el préstamo pertenece al usuario autenticado
                if (prestamo.UsuarioId.ToString() == userId)
                {
                    return Ok(new
                    {
                        prestamo.Id,
                        prestamo.FechaPrestamo,
                        prestamo.FechaDevolucion,
                        prestamo.Activo,
                        UsuarioId = prestamo.UsuarioId,
                        UsuarioNombre = prestamo.Usuario?.Nombre, // Nombre del usuario (si existe)
                        LibroId = prestamo.LibroId,
                        LibroTitulo = prestamo.Libro?.Titulo,     // Título del libro (si existe)
                        LibroEditorial = prestamo.Libro?.Editorial?.NombreEditorial // Nombre de la editorial
                    });
                }
                else
                {
                    return Forbid("No tienes permiso para consultar este préstamo.");
                }
            }

            // Si el rol no es válido, negar acceso
            return Forbid("Rol no autorizado.");
        }




        // PUT: api/Prestamos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrestamo(int id, Prestamo prestamo)
        {
            if (id != prestamo.Id)
            {
                return BadRequest();
            }

            _context.Entry(prestamo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrestamoExists(id))
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
       

        [HttpPost]
        [Authorize(Roles = "ADMIN,USUARIO")]
        public async Task<ActionResult<Prestamo>> PostPrestamo(Prestamo prestamo)
        {
            // Validar que el préstamo no sea por más de 60 días
            if ((prestamo.FechaDevolucion - prestamo.FechaPrestamo).TotalDays > 60)
            {
                return BadRequest("No se puede solicitar un préstamo por más de 60 días.");
            }

            _context.Prestamos.Add(prestamo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPrestamo", new { id = prestamo.Id }, prestamo);
        }


        // DELETE: api/Prestamos/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeletePrestamo(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null)
            {
                return NotFound();
            }

            _context.Prestamos.Remove(prestamo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PrestamoExists(int id)
        {
            return _context.Prestamos.Any(e => e.Id == id);
        }
    }
}

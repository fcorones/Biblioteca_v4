using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_v4.Context;
using API_v4.Models;
using API_v4.Services;
using Microsoft.AspNetCore.Authorization;

namespace API_v4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly BibliotecaDbContext _context;
        private readonly TokenService _tokenService;
        private readonly PasswordService _passwordService;

        // Constructor único que inyecta todas las dependencias necesarias
        public UsuariosController(BibliotecaDbContext context, TokenService tokenService, PasswordService passwordService)
        {
            _context = context;
            _tokenService = tokenService;
            _passwordService = passwordService;
        }

        public class LoginRequest
        {
            public string Email { get; set; }
            public string Contraseña { get; set; }
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.Rol) // Carga la información del rol
                .Select(u => new Usuario
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Email = u.Email,
                    Direccion = u.Direccion,
                    Telefono = u.Telefono,
                  //  ContraseñaHasheada = u.ContraseñaHasheada,
                    RolId = u.RolId,
                    RolNombre = u.Rol.Nombre, // Mapea el nombre del rol
                    Eliminado = u.Eliminado
                })
                .ToListAsync();

            return usuarios;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                usuario.Id,
                usuario.Nombre,
                usuario.Email,
                usuario.Telefono,
                usuario.Direccion,
                RolNombre = usuario.Rol.Nombre,
                usuario.Eliminado
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest("El ID del usuario no coincide con el ID proporcionado en la URL.");
            }

            if (!string.IsNullOrWhiteSpace(usuario.RolNombre))
            {
                var rol = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == usuario.RolNombre);
                if (rol == null)
                {
                    return BadRequest("El rol especificado no existe.");
                }

                usuario.RolId = rol.Id;
            }

            // Si la contraseña está vacía, conserva la contraseña existente
            if (string.IsNullOrWhiteSpace(usuario.ContraseñaHasheada))
            {
                var usuarioExistente = await _context.Usuarios.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (usuarioExistente == null)
                {
                    return NotFound("El usuario especificado no existe.");
                }

                usuario.ContraseñaHasheada = usuarioExistente.ContraseñaHasheada;
            }
            else
            {
                // Si se proporciona una contraseña, hashearla antes de guardar
                usuario.ContraseñaHasheada = _passwordService.HashPassword(usuario.ContraseñaHasheada);
            }

            usuario.Rol = null; // Evitar conflictos con EF Core

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound("El usuario especificado no existe.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            bool usuarioExiste = await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email);
            if (usuarioExiste)
            {
                return Conflict("Ya existe un usuario con este email.");
            }

            var rol = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == usuario.RolNombre);
            if (rol == null)
            {
                return BadRequest("El rol especificado no existe.");
            }

            usuario.RolId = rol.Id;
            usuario.Rol = null;
            usuario.ContraseñaHasheada = _passwordService.HashPassword(usuario.ContraseñaHasheada);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var usuarioGuardado = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == usuario.Id);

            return CreatedAtAction("GetUsuario", new { id = usuarioGuardado.Id }, usuarioGuardado);
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .Include(u => u.Rol) // Incluye el rol
                    .FirstOrDefaultAsync(u => u.Email == loginRequest.Email);

                if (usuario == null)
                {
                    return Unauthorized("El usuario no existe.");
                }

                if (!_passwordService.VerifyPassword(loginRequest.Contraseña, usuario.ContraseñaHasheada))
                {
                    return Unauthorized("La contraseña es incorrecta.");
                }

                if (usuario.Rol == null)
                {
                    return Unauthorized("El usuario no tiene un rol asignado.");
                }

                if (usuario.Eliminado)
                {
                    return Forbid("Este usuario ha sido eliminado y no puede acceder al sistema.");
                }

                // Generar el token JWT
                var token = _tokenService.GenerateToken(
                    usuario.Id.ToString(),
                    usuario.Nombre,
                    usuario.Rol.Nombre // El rol se incluye en el token
                );

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}\n{ex.StackTrace}");
            }
        }






        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}

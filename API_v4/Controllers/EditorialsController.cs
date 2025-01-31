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

namespace API_v4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EditorialsController : ControllerBase
    {
        private readonly BibliotecaDbContext _context;

        public EditorialsController(BibliotecaDbContext context)
        {
            _context = context;
        }

        // GET: api/Editorials
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Editorial>>> GetEditoriales()
        {
            return await _context.Editoriales.ToListAsync();
        }

        // GET: api/Editorials/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Editorial>> GetEditorial(int id)
        {
            var editorial = await _context.Editoriales.FindAsync(id);

            if (editorial == null)
            {
                return NotFound();
            }

            return editorial;
        }

        // PUT: api/Editorials/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> PutEditorial(int id, Editorial editorial)
        {
            if (id != editorial.Id)
            {
                return BadRequest();
            }

            _context.Entry(editorial).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EditorialExists(id))
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

        // POST: api/Editorials
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Editorial>> PostEditorial(Editorial editorial)
        {
            _context.Editoriales.Add(editorial);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEditorial", new { id = editorial.Id }, editorial);
        }

        // DELETE: api/Editorials/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteEditorial(int id)
        {
            var editorial = await _context.Editoriales.FindAsync(id);
            if (editorial == null)
            {
                return NotFound();
            }

            _context.Editoriales.Remove(editorial);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EditorialExists(int id)
        {
            return _context.Editoriales.Any(e => e.Id == id);
        }
    }
}

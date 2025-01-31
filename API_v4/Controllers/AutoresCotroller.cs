//using API_v3.Context;
//using API_v4.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//[Route("api/[controller]")]
//[ApiController]
//public class AutoresController : ControllerBase
//{
//    private readonly BibliotecaDbContext _context;

//    public AutoresController(BibliotecaDbContext context)
//    {
//        _context = context;
//    }

//    // GET: api/Autores
//    [HttpGet]
//    public async Task<ActionResult<IEnumerable<Autor>>> GetAutores()
//    {
//        return await _context.Autores.ToListAsync();
//    }

//    // GET: api/Autores/5
//    [HttpGet("{id}")]
//    public async Task<ActionResult<Autor>> GetAutor(int id)
//    {
//        var autor = await _context.Autores.FindAsync(id);

//        if (autor == null)
//        {
//            return NotFound();
//        }

//        return autor;
//    }

//    // POST: api/Autores
//    [HttpPost]
//    public async Task<ActionResult<Autor>> PostAutor(Autor autor)
//    {
//        _context.Autores.Add(autor);
//        await _context.SaveChangesAsync();

//        // Devuelve el autor recién creado con su URL de acceso.
//        return CreatedAtAction(nameof(GetAutor), new { id = autor.Id }, autor);
//    }
//}

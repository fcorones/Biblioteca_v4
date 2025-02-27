using API_v4.Models;
using Microsoft.EntityFrameworkCore;

namespace API_v4.Context
{
    public class BibliotecaDbContext : DbContext
    {
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Editorial> Editoriales { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relación N-M: Libros x Géneros (tabla intermedia)
            modelBuilder.Entity<Libro>()
                .HasMany(l => l.Generos)
                .WithMany(g => g.Libros)
                .UsingEntity(j => j.ToTable("Libros_x_Genero"));


            // Relación 1-N: Autores - Libros
            modelBuilder.Entity<Libro>()
                .HasOne(l => l.Autor)
                .WithMany(a => a.Libros)
                .HasForeignKey(l => l.AutorId);

            // Relación 1-N: Editoriales - Libros
            modelBuilder.Entity<Libro>()
                .HasOne(l => l.Editorial)
                .WithMany(e => e.Libros)
                .HasForeignKey(l => l.EditorialId);

            // Relación 1-N: Roles - Usuarios
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.RolId);

            // Relación 1-N: Usuarios - Préstamos
            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Prestamos)
                .HasForeignKey(p => p.UsuarioId);

            /*  // Relación 1-1: Libros - Préstamos
              modelBuilder.Entity<Prestamo>()
                  .HasOne(p => p.Libro)
                  .WithOne(l => l.Prestamo)
                  .HasForeignKey<Prestamo>(p => p.LibroId);
            esto es con [JsonIgnore public Prestamo? Prestamo { get; set; }
            */
            // RELACIÓN 1-N LIBROS - PRESTAMOS
            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Libro)
                .WithMany(l => l.Prestamos)
                .HasForeignKey(p => p.LibroId);

            base.OnModelCreating(modelBuilder);
        }

        
        public BibliotecaDbContext(DbContextOptions<BibliotecaDbContext> options)
            : base(options)
        {
        }
    }
}

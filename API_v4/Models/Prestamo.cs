using Humanizer;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API_v4.Models
{
    public class Prestamo
    {
        public int Id { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaDevolucion { get; set; }
        public bool Eliminado { get; set; } = false;

        public bool Activo { get; set; } = false;

       
        public int UsuarioId { get; set; }
        [JsonIgnore]
        public Usuario? Usuario { get; set; }


        
        public int LibroId { get; set; }
        [JsonIgnore]
        public Libro? Libro { get; set; }

        /*
        [NotMapped]                     los saco porque no quiero insertar por nombre, que pasa si tiene mismo nombre?
        public string? NombreUsuario { get; set; }  lo mismo para el usuario, que pasa si existen 2 raul garcía?

        [NotMapped]
        public string? NombreLibro { get; set; }

        [NotMapped]
        public string? NombreLibroEspaniol { get; set; }
        */

    }
}

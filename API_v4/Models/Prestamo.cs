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

        public EstadoPrestamo Estado { get; set; }

       
        public int UsuarioId { get; set; }
        [JsonIgnore]
        public Usuario? Usuario { get; set; }


        
        public int LibroId { get; set; }
        [JsonIgnore]
        public Libro? Libro { get; set; }

    }
}

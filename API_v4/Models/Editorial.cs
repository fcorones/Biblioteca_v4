using System.Text.Json.Serialization;

namespace API_v4.Models
{
    public class Editorial
    {
        public int Id { get; set; }
        public string NombreEditorial { get; set; } = string.Empty;
        public bool Eliminado { get; set; } = false;

        // Relación 1-N: Una editorial tiene muchos libros
        [JsonIgnore]
        public ICollection<Libro>? Libros { get; set; }
    }
}

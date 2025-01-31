using System.Text.Json.Serialization;

namespace API_v4.Models
{
    public class Genero
    {
        public int Id { get; set; }
        public string NombreGenero { get; set; } = string.Empty;
        public bool Eliminado { get; set; } = false;

        // Relación N-M: Un género puede estar asociado a muchos libros
        [JsonIgnore]
        public ICollection<Libro>? Libros { get; set; }

    }
}

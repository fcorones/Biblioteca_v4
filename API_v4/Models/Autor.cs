using System.Text.Json.Serialization;

namespace API_v4.Models
{
    public class Autor
    {
        public int Id { get; set; }
        public string NombreAutor { get; set; } = string.Empty;
        public bool Eliminado { get; set; } = false;


        [JsonIgnore]
        public ICollection<Libro>? Libros { get; set; }

    }
}

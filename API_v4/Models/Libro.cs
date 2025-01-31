using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API_v4.Models
{
    public class Libro
    {
        //cuidado. gran cambio: 
        /* 
            - eliminado public Editorial Editorial
            - AutorId ahora es JsonIgnore
            - EditorialId ahora es JsonIgnore
           esto permite no colocar los ids y postear con los nombres
         */
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string TituloEspaniol { get; set; } = string.Empty;
        public int? AnioDePublicacion { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string PortadaUrl { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool BoolPrestado { get; set; }
        public int NumeroEdicion { get; set; } = 1;
        public bool Eliminado { get; set; } = false;


        // Solo para relaciones internas
        [JsonIgnore]
        public int AutorId { get; set; }

        [JsonIgnore]  // Ignorar esta propiedad en el JSON
        public Autor? Autor { get; set; }

        [JsonIgnore]
        public int EditorialId { get; set; }

        [JsonIgnore]  // Ignorar esta propiedad en el JSON
        public Editorial? Editorial { get; set; }

        [JsonIgnore]
        public ICollection<Prestamo>? Prestamos { get; set; }


        [JsonIgnore]
        [NotMapped]
        public ICollection<int>? GeneroIds { get; set; }


        [JsonIgnore]
        public ICollection<Genero>? Generos { get; set; }

        // Campos usados solo en la solicitud del cliente
        [NotMapped]
        public string? NombreAutor { get; set; }
        [NotMapped]
        public string? NombreEditorial { get; set; }
        [NotMapped]
        public ICollection<string>? NombresGeneros { get; set; }
    }

}

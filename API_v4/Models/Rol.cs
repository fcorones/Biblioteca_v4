using System.Text.Json.Serialization;

namespace API_v4.Models
{
    public class Rol
    {
        
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Usuario>? Usuarios { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API_v4.Models
{
    public class Usuario
    {
        public int Id { get; set; }         //QUIZAS EL ID DEBERÍA SER EL DNI...
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
       
        public string ContraseñaHasheada {  get; set; } = string.Empty;
        public bool Eliminado { get; set; } = false;

        [NotMapped]
        public string RolNombre { get; set; } = string.Empty;
        [JsonIgnore]
        public int RolId { get; set; }
        
        [JsonIgnore]
        public Rol Rol { get; set; } = new Rol();
        


        [JsonIgnore]
        public ICollection<Prestamo>? Prestamos { get; set; }
    }
}

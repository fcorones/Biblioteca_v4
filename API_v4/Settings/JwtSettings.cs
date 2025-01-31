namespace API_v4.Settings
{
    public class JwtSettings
    {
        public string Secret { get; set; }        // Clave secreta para firmar los tokens
        public int ExpirationMinutes { get; set; } // Tiempo de expiración en minutos
        public string Issuer { get; set; }        // Emisor del token
        public string Audience { get; set; }

    }
}

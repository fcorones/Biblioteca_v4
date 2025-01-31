using System;
using System.Security.Cryptography;
using System.Text;

namespace API_v4.Services
{
    public class PasswordService
    {
        // Hashear la contraseña usando PBKDF2 con sal
        public string HashPassword(string password)
        {
            // Crear un 'sal' único para cada contraseña
            var salt = GenerateSalt();

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
            {
                var hash = pbkdf2.GetBytes(32); // Longitud del hash (32 bytes)

                // Combinar sal y hash
                var combined = new byte[salt.Length + hash.Length];
                Buffer.BlockCopy(salt, 0, combined, 0, salt.Length);
                Buffer.BlockCopy(hash, 0, combined, salt.Length, hash.Length);

                return Convert.ToBase64String(combined); // Devuelve el resultado como Base64
            }
        }

        // Verificar la contraseña comparando el hash almacenado
        public bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            // Convertir el hash almacenado (Base64) en bytes
            var combined = Convert.FromBase64String(hashedPassword);

            // Separar el 'sal' del hash
            var salt = new byte[16]; // Tamaño del 'sal' es 16 bytes
            Buffer.BlockCopy(combined, 0, salt, 0, salt.Length);

            var storedHash = new byte[32]; // Tamaño del hash es 32 bytes
            Buffer.BlockCopy(combined, salt.Length, storedHash, 0, storedHash.Length);

            // Generar el hash de la contraseña proporcionada con el mismo 'sal'
            using (var pbkdf2 = new Rfc2898DeriveBytes(plainPassword, salt, 10000, HashAlgorithmName.SHA256))
            {
                var computedHash = pbkdf2.GetBytes(32);

                // Comparar el hash generado con el almacenado
                return computedHash.SequenceEqual(storedHash);
            }
        }

        // Generar un 'sal' aleatorio de 16 bytes
        private byte[] GenerateSalt()
        {
            var salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}

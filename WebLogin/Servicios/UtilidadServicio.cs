using System.Security.Cryptography;
using System.Text;

namespace WebLogin.Servicios
{
    public static class UtilidadServicio
    {
        public static string ConvertirSHA256(string texto)
        {
            string hash = string.Empty;

            using (SHA256 crypt = SHA256.Create())
            {
                byte[] hashValue = crypt.ComputeHash(Encoding.UTF8.GetBytes(texto));

                // Convertir el array en cadena
                foreach (byte b in hashValue)
                {
                    hash += $"{b:X2}";
                }
            }
            return hash;
        }

        public static string GenerarToken()
        {
            string token = Guid.NewGuid().ToString();
            return token;
        }
    }
}

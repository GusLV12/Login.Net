using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using System.IO;

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

    public class PathService
    {
        private readonly IWebHostEnvironment _env;

        public PathService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string ObtenerRutaArchivo(string nombreArchivo)
        {
            return Path.Combine(_env.ContentRootPath, "Plantilla", nombreArchivo);
        }
    }
}

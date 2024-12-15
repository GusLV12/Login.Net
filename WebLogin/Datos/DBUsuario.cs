using System.Data;
using Microsoft.Data.SqlClient;
using WebLogin.Models;

namespace WebLogin.Datos
{
    public class DBUsuario
    {
        private static string CadenaSQL = "Server=(local); DataBase=DBPrueba_test; Trusted_connection=True; TrustServerCertificate=True;";

        public static bool Registrar(UsuarioDTO usuario)
        {
            bool res = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(CadenaSQL))
                {
                    string query = "INSERT INTO USUARIO(Nombre,Correo,Clave,Confirmado,Token)";
                    query += "values(@nombre,@correo,@clave,@confirmado,@token)";


                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {

                        cmd.Parameters.AddWithValue("@nombre", usuario.Nombre);
                        cmd.Parameters.AddWithValue("@correo", usuario.Correo);
                        cmd.Parameters.AddWithValue("@clave", usuario.Clave);
                        cmd.Parameters.AddWithValue("@confirmado", usuario.Confirmado);
                        cmd.Parameters.AddWithValue("@token", usuario.Token);

                        connection.Open();
                        Console.WriteLine("Conexión exitosa a la base de datos.");

                        // Ejecutar la consulta
                        int rowsAffected = cmd.ExecuteNonQuery();
                        res = rowsAffected > 0; // Retorna true si se insertó al menos un registro
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al ejecutar la BD: {ex.Message}");
                throw;
            }
    }
}

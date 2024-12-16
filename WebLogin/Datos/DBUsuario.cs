using System.Data;
using Microsoft.Data.SqlClient;
using WebLogin.Models;

namespace WebLogin.Datos
{
    public class DBUsuario
    {
        private static string CadenaSQL = "Server=(local); DataBase=DBPrueba_test; Trusted_connection=True; TrustServerCertificate=True;";

        // Método para registrar un usuario
        public static bool Registrar(UsuarioDTO usuario)
        {
            bool res = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(CadenaSQL))
                {
                    string query = "INSERT INTO USUARIO (Nombre, Correo, Clave, Confirmado, Token) " +
                                   "VALUES (@nombre, @correo, @clave, @confirmado, @token);";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // Asignación de parámetros
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
                throw; // Propagar el error para manejo externo
            }

            return res;
        }

        // Método para validar un usuario por correo y clave
        public static UsuarioDTO Validar(string correo, string clave)
        {
            UsuarioDTO usuario = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(CadenaSQL))
                {
                    string query = "SELECT Nombre, Clave, Restablecer, Confirmado FROM USUARIO " +
                                   "WHERE Correo = @correo AND Clave = @clave;";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // Asignación de parámetros
                        cmd.Parameters.AddWithValue("@correo", correo);
                        cmd.Parameters.AddWithValue("@clave", clave);

                        connection.Open();
                        Console.WriteLine("Conexión exitosa a la base de datos.");

                        // Ejecutar la consulta
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read()) // Si encuentra una fila
                            {
                                usuario = new UsuarioDTO()
                                {
                                    Nombre = dr["Nombre"].ToString(),
                                    Restablecer = dr["Restablecer"] != DBNull.Value && Convert.ToBoolean(dr["Restablecer"]),
                                    Confirmado = dr["Confirmado"] != DBNull.Value && Convert.ToBoolean(dr["Confirmado"])
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al ejecutar la BD: {ex.Message}");
                throw; // Propagar el error para manejo externo
            }

            return usuario; // Retorna null si no se encontró el usuario
        }

        public static UsuarioDTO Obtener(string correo)
        {
            UsuarioDTO usuario = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(CadenaSQL))
                {
                    string query = "SELECT Nombre, Clave, Restablecer, Confirmado, Token FROM USUARIO " +
                                   "WHERE Correo = @correo;";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // Asignación de parámetros
                        cmd.Parameters.AddWithValue("@correo", correo);

                        connection.Open();
                        Console.WriteLine("Conexión exitosa a la base de datos.");

                        // Ejecutar la consulta
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read()) // Si encuentra una fila
                            {
                                usuario = new UsuarioDTO()
                                {
                                    Nombre = dr["Nombre"].ToString(),
                                    Clave = dr["Clave"].ToString(),
                                    Token = dr["Token"].ToString(),
                                    Restablecer = dr["Restablecer"] != DBNull.Value && Convert.ToBoolean(dr["Restablecer"]),
                                    Confirmado = dr["Confirmado"] != DBNull.Value && Convert.ToBoolean(dr["Confirmado"])
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al ejecutar la BD: {ex.Message}");
                throw; // Propagar el error para manejo externo
            }

            return usuario; // Retorna null si no se encontró el usuario
        }

        public static bool ResTAct(int restablecer, string clave, string token)
        {
            bool res = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(CadenaSQL))
                {
                    string query = "UPDATE USUARIO SET " +
                                   "Restablecer=@restablecer, Clave=@clave WHERE Token=@token";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // Asignación de parámetros

                        cmd.Parameters.AddWithValue("@restablecer", restablecer);
                        cmd.Parameters.AddWithValue("@clave", clave);
                        cmd.Parameters.AddWithValue("@token", token);

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
                throw; // Propagar el error para manejo externo
            }

            return res;
        }

        public static bool Confirmar(string token)
        {
            bool res = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(CadenaSQL))
                {
                    string query = "UPDATE USUARIO SET " +
                                   "Confirmado=1 WHERE Token=@token";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // Asignación de parámetros
                        cmd.Parameters.AddWithValue("@token", token);

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
                throw; // Propagar el error para manejo externo
            }

            return res;
        }
    }
}
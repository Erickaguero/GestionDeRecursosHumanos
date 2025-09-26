using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using PrototipoFuncionalRecursosHumanos.Models;

public class UsuarioHandler
{

    private readonly string connectionString = "";

    public UsuarioHandler()
    {
        var builder = WebApplication.CreateBuilder();
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    }

    public Usuario ObtenerUsuario(string correo)
    {
        Usuario usuario = null;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM mydb.usuario WHERE correo = @Correo";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Correo", correo);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuario = new Usuario
                            {
                                IdUsuario = reader.GetInt32(reader.GetOrdinal("idusuario")),
                                IdPersona = reader.GetInt32(reader.GetOrdinal("idpersona")),
                                RolDeUsuario = new RolDeUsuario
                                {
                                    IdRolDeUsuario = reader.GetInt32(reader.GetOrdinal("idrolDeUsuario")),
                                },
                                Correo = reader.GetString(reader.GetOrdinal("correo")),
                                Contrasena = reader.GetString(reader.GetOrdinal("contrasena"))
                            };
                        }
                    }
                }
                connection.Close();
            }
        } catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return usuario;
    }

    public bool ModificarContrasena(string correo, string contrasena)
    {
        bool exito = false;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE mydb.usuario SET contrasena = @Contrasena WHERE correo = @Correo";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Correo", correo);
                    command.Parameters.AddWithValue("@Contrasena", contrasena);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    exito = rowsAffected > 0;
                }
                connection.Close();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return exito;
    }
}
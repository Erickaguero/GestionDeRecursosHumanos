using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using PrototipoFuncionalRecursosHumanos.Models;

public class RolDeUsuarioHandler
{

    private readonly string connectionString = "";

    public RolDeUsuarioHandler()
    {
        var builder = WebApplication.CreateBuilder();
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    }

    public List<RolDeUsuario> ObtenerRolesDeUsuario()
    {
        List<RolDeUsuario> rolesDeUsuario = new List<RolDeUsuario>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM mydb.roldeusuario";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RolDeUsuario rolDeUsuario = new RolDeUsuario
                            {
                                IdRolDeUsuario = reader.GetInt32(reader.GetOrdinal("idrolDeUsuario")),
                                Descripcion = reader.GetString(reader.GetOrdinal("descripcion"))
                            };
                            rolesDeUsuario.Add(rolDeUsuario);
                        }
                    }
                }
                connection.Close();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return rolesDeUsuario;
    }
}
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using PrototipoFuncionalRecursosHumanos.Models;

public class TipoPermisosHandler
{

    private readonly string connectionString = "";

    public TipoPermisosHandler()
    {
        var builder = WebApplication.CreateBuilder();
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    }

    public List<TipoPermiso> ObtenerTipoPermisos()
    {
        List<TipoPermiso> tipoPermisos = new List<TipoPermiso>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM mydb.tipopermiso";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TipoPermiso tipoPermiso = new TipoPermiso
                            {
                                IdTipoPermiso = reader.GetInt32(reader.GetOrdinal("idtipoPermiso")),
                                Descripcion = reader.GetString(reader.GetOrdinal("descripcion"))
                            };
                            tipoPermisos.Add(tipoPermiso);
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
        return tipoPermisos;
    }

    public TipoPermiso ObtenerTipoPermiso(int idTipoPermiso)
    {
        TipoPermiso tipoPermiso = null;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM mydb.tipopermiso WHERE idtipoPermiso = @IdTipoPermiso";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdTipoPermiso", idTipoPermiso);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tipoPermiso = new TipoPermiso
                            {
                                IdTipoPermiso = reader.GetInt32(reader.GetOrdinal("idtipoPermiso")),
                                Descripcion = reader.GetString(reader.GetOrdinal("descripcion"))
                            };
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
        return tipoPermiso;
    }
}
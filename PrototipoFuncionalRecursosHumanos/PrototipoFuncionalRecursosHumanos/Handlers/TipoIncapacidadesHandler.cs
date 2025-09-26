using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using PrototipoFuncionalRecursosHumanos.Models;

public class TipoIncapacidadesHandler
{

    private readonly string connectionString = "";

    public TipoIncapacidadesHandler()
    {
        var builder = WebApplication.CreateBuilder();
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    }

    public List<TipoIncapacidad> ObtenerTipoIncapacidades()
    {
        List<TipoIncapacidad> tipoIncapacidades = new List<TipoIncapacidad>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM mydb.tipoincapacidad";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TipoIncapacidad tipoIncapacidad = new TipoIncapacidad
                            {
                                IdTipoIncapacidad = reader.GetInt32(reader.GetOrdinal("idtipoincapacidad")),
                                Descripcion = reader.GetString(reader.GetOrdinal("descripcion"))
                            };
                            tipoIncapacidades.Add(tipoIncapacidad);
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
        return tipoIncapacidades;
    }

    public TipoIncapacidad ObtenerTipoIncapacidad(int idTipoIncapacidad)
    {
        TipoIncapacidad tipoIncapacidad = null;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM mydb.tipoincapacidad WHERE idtipoincapacidad = @IdTipoIncapacidad";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdTipoIncapacidad", idTipoIncapacidad);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tipoIncapacidad = new TipoIncapacidad
                            {
                                IdTipoIncapacidad = reader.GetInt32(reader.GetOrdinal("idtipoincapacidad")),
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
        return tipoIncapacidad;
    }
}
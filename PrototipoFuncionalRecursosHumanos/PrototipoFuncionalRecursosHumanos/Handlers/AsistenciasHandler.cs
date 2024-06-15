using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using PrototipoFuncionalRecursosHumanos.Models;

public class AsistenciasHandler
{

    private readonly string connectionString = "";

    public AsistenciasHandler()
    {
        var builder = WebApplication.CreateBuilder();
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    }

    public List<Asistencia> ObtenerAsistencias()
    {
        List<Asistencia> asistencias = new List<Asistencia>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM mydb.asistencia";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Asistencia asistencia = new Asistencia
                            {
                                IdAsistencia = reader.GetInt32(reader.GetOrdinal("idasistencia")),
                                FechaIngreso = reader.GetDateTime(reader.GetOrdinal("fechaIngreso")),
                                FechaSalida = reader.GetDateTime(reader.GetOrdinal("fechaSalida")),
                                Colaborador = new Colaborador
                                {
                                    IdColaborador = reader.GetInt32(reader.GetOrdinal("id_colaborador")),
                                }
                            };
                            asistencias.Add(asistencia);
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
        return asistencias;
    }
}
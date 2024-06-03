using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using PrototipoFuncionalRecursosHumanos.Models;

public class EvaluacionHandler
{

    private readonly string connectionString = "";

    public EvaluacionHandler()
    {
        var builder = WebApplication.CreateBuilder();
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    }

    public bool AgregarEvaluacion(Evaluacion evaluacion)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO mydb.evaluacion (id_colaborador, promedioEvaluacion) " +
                    "VALUES(@IdColaborador, @PromedioEvaluacion)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdColaborador", evaluacion.Colaborador.IdColaborador);
                    command.Parameters.AddWithValue("@PromedioEvaluacion", evaluacion.PromedioEvaluacion);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            exito = false;
        }

        return exito;
    }

    public List<Evaluacion> ObtenerEvaluaciones(int idColaborador)
    {
        List<Evaluacion> evaluaciones = new List<Evaluacion>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM mydb.evaluacion WHERE id_colaborador = @IdColaborador";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdColaborador", idColaborador);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Evaluacion evaluacion = new Evaluacion
                            {
                                IdEvaluacion = reader.GetInt32(reader.GetOrdinal("idevaluacion")),
                                FechaEvaluacion = reader.GetDateTime(reader.GetOrdinal("fechaEvaluacion")),
                                Colaborador = new ColaboradorHandler().ObtenerColaborador(reader.GetInt32(reader.GetOrdinal("id_colaborador"))),
                                PromedioEvaluacion = reader.GetDouble(reader.GetOrdinal("promedioEvaluacion"))
                            };
                            evaluaciones.Add(evaluacion);
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
        return evaluaciones;
    }

    public bool EliminarEvaluacion(int idEvaluacion)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM mydb.evaluacion WHERE idevaluacion = @IdEvaluacion";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdEvaluacion", idEvaluacion);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            exito = false;
        }

        return exito;
    }
}
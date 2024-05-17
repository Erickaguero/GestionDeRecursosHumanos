using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using PrototipoFuncionalRecursosHumanos.Models;

public class VacacionesHandler
{

    private readonly string connectionString = "";

    public VacacionesHandler()
    {
        var builder = WebApplication.CreateBuilder();
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    }

    public bool AgregarVacacion(Vacaciones vacacion)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO mydb.vacaciones (id_colaborador,fechaInicio,fechaFin,Estado) " +
                    "VALUES(@IdColaborador, @FechaInicio, @FechaFin, @Estado)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdColaborador", vacacion.IdColaborador);
                    command.Parameters.AddWithValue("@FechaInicio", vacacion.FechaInicio);
                    command.Parameters.AddWithValue("@FechaFin", vacacion.FechaFin);
                    command.Parameters.AddWithValue("@Estado", vacacion.Estado);
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

    public int ObtenerDiasDisponibles(int? idColaborador)
    {
        int diasVacacionesDisponibles = 0;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("CalcularDiasVacaciones", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@IdColaborador", idColaborador));
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            diasVacacionesDisponibles = Convert.ToInt32(reader["DiasVacaciones"]);
                        }
                    }
                    connection.Close();
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Ocurrió un error al obtener la cantidad de dias de vacaciones disponible " + ex.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return diasVacacionesDisponibles;
    }

    public bool VacacionesExistentes(Vacaciones vacacion)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM mydb.vacaciones WHERE id_colaborador = @IdColaborador AND (fechaInicio <= @FechaFin AND fechaFin >= @FechaInicio)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdColaborador", vacacion.IdColaborador);
                    command.Parameters.AddWithValue("@FechaInicio", vacacion.FechaInicio);
                    command.Parameters.AddWithValue("@FechaFin", vacacion.FechaFin);

                    int count = (int)command.ExecuteScalar();

                    connection.Close();
                    return count > 0;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return true;
        }
    }

    public bool EliminarVacacion(int idVacacion)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM mydb.vacaciones WHERE idvacaciones = @IdVacacion";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdVacacion", idVacacion);
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

    public List<Vacaciones> ObtenerVacaciones(int? idColaborador)
    {
        List<Vacaciones> vacaciones = new List<Vacaciones>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM mydb.vacaciones WHERE id_colaborador = @IdColaborador";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdColaborador", idColaborador);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Vacaciones vacacion = new Vacaciones
                            {
                                IdVacaciones = reader.GetInt32(reader.GetOrdinal("idvacaciones")),
                                IdColaborador = reader.GetInt32(reader.GetOrdinal("id_colaborador")),
                                FechaInicio = reader.GetDateTime(reader.GetOrdinal("fechaInicio")),
                                FechaFin    = reader.GetDateTime(reader.GetOrdinal("fechaFin")),
                                Estado = reader.GetString(reader.GetOrdinal("estado"))
                            };
                            vacaciones.Add(vacacion);
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

        return vacaciones;
    }
}
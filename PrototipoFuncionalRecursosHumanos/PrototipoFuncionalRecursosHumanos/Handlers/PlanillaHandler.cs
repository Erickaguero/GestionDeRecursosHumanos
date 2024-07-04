using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using PrototipoFuncionalRecursosHumanos.Models;

public class PlanillaHandler
{

    private readonly string connectionString = "";

    public PlanillaHandler()
    {
        var builder = WebApplication.CreateBuilder();
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    }

    public bool GenerarPlanillaColaboradores()
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("CrearPlanillaColaboradores", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Ocurrió un error al generar la planilla para los colaboradores: " + ex.Message);
            exito = false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            exito = false;
        }

        return exito;
    }

    public bool PlanillaExistente(DateTime fecha)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM mydb.planilla WHERE CAST(fechaGeneracion AS DATE) = @Fecha";
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Añadir el parámetro @Fecha a la consulta
                    command.Parameters.AddWithValue("@Fecha", fecha.Date);
                    int count = (int)command.ExecuteScalar();
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

    public List<Planilla> ObtenerPlanillas()
    {
        List<Planilla> planillas = new List<Planilla>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM mydb.planilla";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Planilla planilla = new Planilla
                            {
                                IdPlanilla = reader.GetInt32(reader.GetOrdinal("idplanilla")),
                                Colaborador = new Colaborador
                                {
                                    IdColaborador = reader.GetInt32(reader.GetOrdinal("idcolaborador"))
                                },
                                FechaGeneracion = reader.GetDateTime(reader.GetOrdinal("fechaGeneracion")),
                                Monto = reader.GetDouble(reader.GetOrdinal("monto")),
                                HorasExtra = reader.GetDouble(reader.GetOrdinal("horasExtra")),
                                HorasIncapacidades = reader.GetDouble(reader.GetOrdinal("horasIncapacidades")),
                                HorasPermiso = reader.GetDouble(reader.GetOrdinal("horasPermiso")),
                                HorasTrabajadas = reader.GetDouble(reader.GetOrdinal("horasTrabajadas")),
                                HorasVacaciones = reader.GetDouble(reader.GetOrdinal("horasVacaciones")),
                                DeduccionCCSS = reader.GetDouble(reader.GetOrdinal("deduccionCCSS")),
                                DeduccionRenta = reader.GetDouble(reader.GetOrdinal("deduccionRenta")),
                                SalarioBruto = reader.GetDouble(reader.GetOrdinal("salarioBruto"))
                            };
                            planillas.Add(planilla);
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
        return planillas;
    }
}
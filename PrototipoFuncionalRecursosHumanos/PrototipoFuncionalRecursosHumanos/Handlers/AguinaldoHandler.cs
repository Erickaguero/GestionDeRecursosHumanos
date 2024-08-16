using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using PrototipoFuncionalRecursosHumanos.Models;

public class AguinaldoHandler
{

    private readonly string connectionString = "";

    public AguinaldoHandler()
    {
        var builder = WebApplication.CreateBuilder();
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    }

    public bool GenerarAguinaldoColaboradores()
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("CrearAguinaldoColaboradores", connection))
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
            Console.WriteLine("Ocurrió un error al generar el alguinaldo para los colaboradores: " + ex.Message);
            exito = false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            exito = false;
        }

        return exito;
    }

    public bool AguinaldoExistente(DateTime fecha)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM mydb.aguinaldo WHERE CAST(fechaGeneracion AS DATE) = @Fecha";
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
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


    public List<Aguinaldo> ObtenerAguinaldos()
    {
        List<Aguinaldo> aguinaldos = new List<Aguinaldo>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM mydb.aguinaldo";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Aguinaldo aguinaldo = new Aguinaldo
                            {
                                IdAguinaldo = reader.GetInt32(reader.GetOrdinal("idaguinaldo")),
                                Colaborador = new Colaborador
                                {
                                    IdColaborador = reader.GetInt32(reader.GetOrdinal("id_colaborador"))
                                },
                                FechaGeneracion = reader.GetDateTime(reader.GetOrdinal("fechaGeneracion")),
                                Monto = reader.GetDouble(reader.GetOrdinal("montoAguinaldo")),
                            };
                            aguinaldos.Add(aguinaldo);
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
        return aguinaldos;
    }

    public List<Aguinaldo> ObtenerAguinaldosColaborador(int idColaborador)
    {
        List<Aguinaldo> aguinaldos = new List<Aguinaldo>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM mydb.aguinaldo WHERE id_colaborador = @IdColaborador";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdColaborador", idColaborador);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Aguinaldo aguinaldo = new Aguinaldo
                            {
                                IdAguinaldo = reader.GetInt32(reader.GetOrdinal("idaguinaldo")),
                                Colaborador = new Colaborador
                                {
                                    IdColaborador = reader.GetInt32(reader.GetOrdinal("id_colaborador"))
                                },
                                FechaGeneracion = reader.GetDateTime(reader.GetOrdinal("fechaGeneracion")),
                                Monto = reader.GetDouble(reader.GetOrdinal("montoAguinaldo")),
                            };
                            aguinaldos.Add(aguinaldo);
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
        return aguinaldos;
    }
}
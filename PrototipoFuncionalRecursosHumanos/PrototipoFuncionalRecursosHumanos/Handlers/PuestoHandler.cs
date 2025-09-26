using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using PrototipoFuncionalRecursosHumanos.Models;

public class PuestoHandler
{

    private readonly string connectionString = "";

    public PuestoHandler()
    {
        var builder = WebApplication.CreateBuilder();
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    }

    public bool AgregarPuesto(Puesto puesto)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO mydb.puesto (nombrePuesto, costoPorHora) " +
                    "VALUES(@NombrePuesto, @CostoPorHora)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NombrePuesto", puesto.NombrePuesto);
                    command.Parameters.AddWithValue("@CostoPorHora", puesto.CostoPorHora);

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

    public bool EditarPuesto(Puesto puesto)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE mydb.puesto SET nombrePuesto = @NombrePuesto, costoPorHora = @CostoPorHora WHERE idpuesto = @IdPuesto";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombrePuesto", puesto.NombrePuesto);
                    command.Parameters.AddWithValue("@CostoPorHora", puesto.CostoPorHora);
                    command.Parameters.AddWithValue("@IdPuesto", puesto.IdPuesto);
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

    public bool EliminarPuesto(int idPuesto)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM mydb.puesto WHERE idpuesto = @IdPuesto";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdPuesto", idPuesto);
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


    public Puesto ObtenerPuesto(int idPuesto)
    {
        Puesto puesto = null;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM mydb.puesto WHERE idpuesto = @IdPuesto";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdPuesto", idPuesto);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            puesto = new Puesto
                            {
                                IdPuesto = reader.GetInt32(reader.GetOrdinal("idpuesto")),
                                NombrePuesto = reader.GetString(reader.GetOrdinal("nombrePuesto")),
                                CostoPorHora = reader.GetDouble(reader.GetOrdinal("costoPorHora"))
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
        return puesto;
    }

    public List<Puesto> ObtenerPuestos()
    {
        List<Puesto> puestos = new List<Puesto>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM mydb.puesto";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Puesto puesto = new Puesto
                            {
                                IdPuesto = reader.GetInt32(reader.GetOrdinal("idpuesto")),
                                NombrePuesto = reader.GetString(reader.GetOrdinal("nombrePuesto")),
                                CostoPorHora = reader.GetDouble(reader.GetOrdinal("costoPorHora"))
                            };
                            puestos.Add(puesto);
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
        return puestos;
    }
}
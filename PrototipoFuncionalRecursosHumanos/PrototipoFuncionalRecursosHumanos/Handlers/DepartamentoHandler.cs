using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using PrototipoFuncionalRecursosHumanos.Models;

public class DepartamentoHandler
{

    private readonly string connectionString = "";

    public DepartamentoHandler()
    {
        var builder = WebApplication.CreateBuilder();
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    }

    public bool AgregarDepartamento(string nombreDepartamento)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO mydb.departamento (nombre) " +
                    "VALUES(@Nombre)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", nombreDepartamento);
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

    public bool EditarDepartamento(Departamento departamento)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE mydb.departamento SET nombre = @Nombre WHERE iddepartamento = @IdDepartamento" ;
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", departamento.Nombre);
                    command.Parameters.AddWithValue("@IdDepartamento", departamento.IdDepartamento);
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

    public bool EliminarDepartamento(int idDepartamento)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM mydb.departamento WHERE iddepartamento = @IdDepartamento";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdDepartamento", idDepartamento);
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


    public Departamento ObtenerDepartamento(int idDepartamento)
    {
        Departamento departamento = null;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM mydb.departamento WHERE iddepartamento = @IdDepartamento";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdDepartamento", idDepartamento);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            departamento = new Departamento
                            {
                                IdDepartamento = reader.GetInt32(reader.GetOrdinal("iddepartamento")),
                                Nombre = reader.GetString(reader.GetOrdinal("nombre"))
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
        return departamento;
    }

    public List<Departamento> ObtenerDepartamentos()
    {
        List<Departamento> departamentos = new List<Departamento>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM mydb.departamento";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Departamento departamento = new Departamento
                            {
                                IdDepartamento = reader.GetInt32(reader.GetOrdinal("iddepartamento")),
                                Nombre = reader.GetString(reader.GetOrdinal("nombre"))
                            };
                            departamentos.Add(departamento);
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
        return departamentos;
    }
}
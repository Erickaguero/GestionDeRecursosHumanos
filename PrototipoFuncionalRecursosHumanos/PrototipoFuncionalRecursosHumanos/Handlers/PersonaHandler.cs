using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using PrototipoFuncionalRecursosHumanos.Models;

public class PersonaHandler
{

    private readonly string connectionString = "";

    public PersonaHandler()
    {
        var builder = WebApplication.CreateBuilder();
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    }

    public bool AgregarPersona(Persona persona)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO mydb.persona (identificacion,nombre,apellido1,apellido2,fecha_nacimiento) " +
                    "VALUES(@Identificacion, @Nombre, @Apellido1, @Apellido2, @FechaNacimiento)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Identificacion", persona.Identificacion);
                    command.Parameters.AddWithValue("@Nombre", persona.Nombre);
                    command.Parameters.AddWithValue("@Apellido1", persona.Apellido1);
                    command.Parameters.AddWithValue("@Apellido2", persona.Apellido2);
                    command.Parameters.AddWithValue("@FechaNacimiento", persona.FechaDeNacimiento);
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

    public Persona ObtenerPersona(string identificacion)
    {
        Persona persona = null;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM mydb.persona WHERE identificacion = @Identificacion";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Identificacion", identificacion);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            persona = new Persona
                            {
                                IdPersona = reader.GetInt32(reader.GetOrdinal("idpersona")),
                                Identificacion = reader.GetString(reader.GetOrdinal("identificacion")),
                                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                                Apellido1 = reader.GetString(reader.GetOrdinal("apellido1")),
                                Apellido2 = reader.GetString(reader.GetOrdinal("apellido2")),
                                FechaDeNacimiento = reader.GetDateTime(reader.GetOrdinal("fecha_nacimiento"))
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

        return persona;
    }
}
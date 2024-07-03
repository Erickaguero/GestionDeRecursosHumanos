using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using PrototipoFuncionalRecursosHumanos.Models;

public class IncapacidadesHandler
{

    private readonly string connectionString = "";

    public IncapacidadesHandler()
    {
        var builder = WebApplication.CreateBuilder();
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    }

    public bool AgregarIncapacidad(Incapacidades incapacidad)
    {
        bool exito = true;
        // Cambiar esto por un proceso almacenado a futuro
        int idTipoPermisoNoDefinido = 3;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO mydb.incapacidades (fechaFin, id_colaborador, idtipoincapacidad, estado, justificacion) " +
                    "VALUES(@FechaFin, @IdColaborador, @IdTipoIncapacidad, @Estado, @Justificacion)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FechaFin", incapacidad.FechaFin);
                    command.Parameters.AddWithValue("@IdColaborador", incapacidad.Colaborador.IdColaborador);
                    command.Parameters.AddWithValue("@IdTipoIncapacidad", idTipoPermisoNoDefinido);
                    command.Parameters.AddWithValue("@Estado", incapacidad.Estado);
                    command.Parameters.AddWithValue("@Justificacion", incapacidad.Justificacion);
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

    public bool IncapacidadExistente(Incapacidades incapacidad)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM mydb.incapacidades WHERE id_colaborador = @IdColaborador AND (estado = 'Pendiente' OR estado = 'Aprobado') AND (CONVERT(date, fechaInicio) <= @FechaFin AND CONVERT(date, fechaFin) >= @FechaInicio)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdColaborador", incapacidad.Colaborador.IdColaborador);
                    command.Parameters.AddWithValue("@FechaInicio", DateTime.Now.Date);
                    command.Parameters.AddWithValue("@FechaFin", incapacidad.FechaFin);

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

    public bool EditarTipoIncapacidad(int idIncapacidad, int idTipoIncapacidad)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE mydb.incapacidades SET idtipoincapacidad = @IdTipoIncapacidad WHERE idincapacidades = @IdIncapacidad";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdIncapacidad", idIncapacidad);
                    command.Parameters.AddWithValue("@IdTipoIncapacidad", idTipoIncapacidad);
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

    public bool AprobarIncapacidadAdministrador(int idIncapacidad)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE mydb.incapacidades SET estado = @Estado WHERE idincapacidades = @IdIncapacidad";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdIncapacidad", idIncapacidad);
                    command.Parameters.AddWithValue("@Estado", "Aprobado");
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

    public bool AprobarIncapacidadJefatura(int idIncapacidad)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE mydb.incapacidades SET estado = @Estado WHERE idincapacidades = @IdIncapacidad";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdIncapacidad", idIncapacidad);
                    command.Parameters.AddWithValue("@Estado", "Aprobado por jefatura");
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

    public bool RechazarIncapacidad(int idIncapacidad)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE mydb.incapacidades SET estado = @Estado WHERE idincapacidades = @IdIncapacidad";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdIncapacidad", idIncapacidad);
                    command.Parameters.AddWithValue("@Estado", "Rechazado");
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

    public bool EliminarIncapacidad(int idIncapacidad)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM mydb.incapacidades WHERE idincapacidades = @IdIncapacidad";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdIncapacidad", idIncapacidad);
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

    public List<Incapacidades> ObtenerIncapacidades(int? idColaborador)
    {
        List<Incapacidades> incapacidades = new List<Incapacidades>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM mydb.incapacidades WHERE id_colaborador = @IdColaborador";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdColaborador", idColaborador);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Incapacidades incapacidad = new Incapacidades
                            {
                                IdIncapacidad = reader.GetInt32(reader.GetOrdinal("idincapacidades")),
                                FechaInicio = reader.GetDateTime(reader.GetOrdinal("fechaInicio")),
                                FechaFin = reader.GetDateTime(reader.GetOrdinal("fechaFin")),
                                Colaborador = new Colaborador
                                {
                                    IdColaborador = reader.GetInt32(reader.GetOrdinal("id_colaborador"))
                                },
                                TipoIncapacidad = new TipoIncapacidad
                                {
                                    IdTipoIncapacidad = reader.GetInt32(reader.GetOrdinal("idtipoincapacidad"))
                                },
                                Estado = reader.GetString(reader.GetOrdinal("estado")),
                                Justificacion = reader.GetString(reader.GetOrdinal("justificacion"))
                            };
                            incapacidades.Add(incapacidad);
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

        return incapacidades;
    }

    public List<Incapacidades> ObtenerIncapacidadesParaAprobarPorAdministrador(int? idAdministrador)
    {
        List<Incapacidades> incapacidades = new List<Incapacidades>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("ObtenerIncapacidadesParaAprobarPorAdministrador", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@IdAdministrador", idAdministrador));
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Incapacidades incapacidad = new Incapacidades
                            {
                                IdIncapacidad = reader.GetInt32(reader.GetOrdinal("idincapacidades")),
                                FechaInicio = reader.GetDateTime(reader.GetOrdinal("fechaInicio")),
                                FechaFin = reader.GetDateTime(reader.GetOrdinal("fechaFin")),
                                Colaborador = new Colaborador
                                {
                                    IdColaborador = reader.GetInt32(reader.GetOrdinal("id_colaborador"))
                                },
                                TipoIncapacidad = new TipoIncapacidad
                                {
                                    IdTipoIncapacidad = reader.GetInt32(reader.GetOrdinal("idtipoincapacidad"))
                                },
                                Estado = reader.GetString(reader.GetOrdinal("estado")),
                                Justificacion = reader.GetString(reader.GetOrdinal("justificacion"))
                            };
                            incapacidades.Add(incapacidad);
                        }
                        connection.Close();
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Ocurrió un error al obtener las incapacidades para aprobar " + ex.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return incapacidades;
    }

    public List<Incapacidades> ObtenerIncapacidadesParaAprobarPorJefatura(int? idJefatura)
    {
        List<Incapacidades> incapacidades = new List<Incapacidades>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("ObtenerIncapacidadesParaAprobarPorJefatura", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@IdJefatura", idJefatura));
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Incapacidades incapacidad = new Incapacidades
                            {
                                IdIncapacidad = reader.GetInt32(reader.GetOrdinal("idincapacidades")),
                                FechaInicio = reader.GetDateTime(reader.GetOrdinal("fechaInicio")),
                                FechaFin = reader.GetDateTime(reader.GetOrdinal("fechaFin")),
                                Colaborador = new Colaborador
                                {
                                    IdColaborador = reader.GetInt32(reader.GetOrdinal("id_colaborador"))
                                },
                                TipoIncapacidad = new TipoIncapacidad
                                {
                                    IdTipoIncapacidad = reader.GetInt32(reader.GetOrdinal("idtipoincapacidad"))
                                },
                                Estado = reader.GetString(reader.GetOrdinal("estado")),
                                Justificacion = reader.GetString(reader.GetOrdinal("justificacion"))
                            };
                            incapacidades.Add(incapacidad);
                        }
                        connection.Close();
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Ocurrió un error al obtener las incapacidades para aprobar " + ex.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return incapacidades;
    }
}
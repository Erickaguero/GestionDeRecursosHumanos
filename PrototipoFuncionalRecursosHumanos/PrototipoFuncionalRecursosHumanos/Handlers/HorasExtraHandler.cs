using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using PrototipoFuncionalRecursosHumanos.Models;

public class HorasExtraHandler
{

    private readonly string connectionString = "";

    public HorasExtraHandler()
    {
        var builder = WebApplication.CreateBuilder();
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    }

    public bool AgregarHorasExtra(HorasExtra horasExtra)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO mydb.horasextra (fechaHoraExtra, id_colaborador, horas, estado, justificacion) " +
                    "VALUES(@FechaHoraExtra, @IdColaborador, @Horas, @Estado, @Justificacion)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FechaHoraExtra", horasExtra.FechaHorasExtra);
                    command.Parameters.AddWithValue("@IdColaborador", horasExtra.Colaborador.IdColaborador);
                    command.Parameters.AddWithValue("@Horas", horasExtra.Horas);
                    command.Parameters.AddWithValue("@Estado", horasExtra.Estado);
                    command.Parameters.AddWithValue("@Justificacion", horasExtra.Justificacion);
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

    public bool HorasExtraExistentes(HorasExtra horasExtra)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM mydb.horasextra WHERE id_colaborador = @IdColaborador AND (estado = 'Pendiente' OR estado = 'Aprobado') AND (fechaHoraExtra = @FechaHoraExtra)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdColaborador", horasExtra.Colaborador.IdColaborador);
                    command.Parameters.AddWithValue("@FechaHoraExtra", horasExtra.FechaHorasExtra);

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

    public bool AprobarHorasExtraAdministrador(int idHorasExtra)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE mydb.horasextra SET estado = @Estado WHERE idhorasextra = @IdHorasExtra";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdHorasExtra", idHorasExtra);
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

    public bool AprobarHorasExtraJefatura(int idHorasExtra)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE mydb.horasextra SET estado = @Estado WHERE idhorasextra = @IdHorasExtra";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdHorasExtra", idHorasExtra);
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

    public bool RechazarHorasExtra(int idHorasExtra)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE mydb.horasextra SET estado = @Estado WHERE idhorasextra = @IdHorasExtra";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdHorasExtra", idHorasExtra);
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

    public bool EliminarHorasExtra(int idHorasExtra)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM mydb.horasextra WHERE idhorasextra = @IdHorasExtra";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdHorasExtra", idHorasExtra);
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

    public List<HorasExtra> ObtenerHorasExtra(int? idColaborador)
    {
        List<HorasExtra> horasExtra = new List<HorasExtra>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM mydb.horasextra WHERE id_colaborador = @IdColaborador";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdColaborador", idColaborador);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            HorasExtra horaExtra = new HorasExtra
                            {
                                IdHorasExtra = reader.GetInt32(reader.GetOrdinal("idhorasextra")),
                                FechaHorasExtra = reader.GetDateTime(reader.GetOrdinal("fechaHoraExtra")),
                                Colaborador = new Colaborador
                                {
                                    IdColaborador = reader.GetInt32(reader.GetOrdinal("id_colaborador"))
                                },
                                Horas = reader.GetInt32(reader.GetOrdinal("horas")),
                                Estado = reader.GetString(reader.GetOrdinal("estado")),
                                Justificacion = reader.GetString(reader.GetOrdinal("justificacion"))
                            };
                            horasExtra.Add(horaExtra);
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

        return horasExtra;
    }

    public List<HorasExtra> ObtenerHorasExtraParaAprobarPorAdministrador(int? idAdministrador)
    {
        List<HorasExtra> horasExtra = new List<HorasExtra>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("ObtenerHorasExtraParaAprobarPorAdministrador", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@IdAdministrador", idAdministrador));
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            HorasExtra horaExtra = new HorasExtra
                            {
                                IdHorasExtra = reader.GetInt32(reader.GetOrdinal("idhorasextra")),
                                FechaHorasExtra = reader.GetDateTime(reader.GetOrdinal("fechaHoraExtra")),
                                Colaborador = new Colaborador
                                {
                                    IdColaborador = reader.GetInt32(reader.GetOrdinal("id_colaborador"))
                                },
                                Horas = reader.GetInt32(reader.GetOrdinal("horas")),
                                Estado = reader.GetString(reader.GetOrdinal("estado")),
                                Justificacion = reader.GetString(reader.GetOrdinal("justificacion"))
                            };
                            horasExtra.Add(horaExtra);
                        }
                        connection.Close();
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Ocurrió un error al obtener los permisos a aprobar " + ex.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return horasExtra;
    }

    public List<HorasExtra> ObtenerHorasExtraParaAprobarPorJefatura(int? idJefatura)
    {
        List<HorasExtra> horasExtra = new List<HorasExtra>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("ObtenerHorasExtraParaAprobarPorJefatura", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@IdJefatura", idJefatura));
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            HorasExtra horaExtra = new HorasExtra
                            {
                                IdHorasExtra = reader.GetInt32(reader.GetOrdinal("idhorasextra")),
                                FechaHorasExtra = reader.GetDateTime(reader.GetOrdinal("fechaHoraExtra")),
                                Colaborador = new Colaborador
                                {
                                    IdColaborador = reader.GetInt32(reader.GetOrdinal("id_colaborador"))
                                },
                                Horas = reader.GetInt32(reader.GetOrdinal("horas")),
                                Estado = reader.GetString(reader.GetOrdinal("estado")),
                                Justificacion = reader.GetString(reader.GetOrdinal("justificacion"))
                            };
                            horasExtra.Add(horaExtra);
                        }
                        connection.Close();
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Ocurrió un error al obtener los permisos a aprobar " + ex.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return horasExtra;
    }
}
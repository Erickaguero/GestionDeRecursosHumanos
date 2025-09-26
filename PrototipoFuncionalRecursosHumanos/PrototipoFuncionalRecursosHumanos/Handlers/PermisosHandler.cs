using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using PrototipoFuncionalRecursosHumanos.Models;

public class PermisosHandler
{

    private readonly string connectionString = "";

    public PermisosHandler()
    {
        var builder = WebApplication.CreateBuilder();
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    }

    public bool AgregarPermiso(Permisos permiso)
    {
        bool exito = true;
        // Cambiar esto por un proceso almacenado a futuro
        int idTipoPermisoNoDefinido = 3;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO mydb.permisos (fechaPermiso, id_colaborador, idtipoPermiso, horas, estado, justificacion) " +
                    "VALUES(@FechaPermiso, @IdColaborador, @IdTipoPermiso, @Horas, @Estado, @Justificacion)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FechaPermiso", permiso.FechaPermiso);
                    command.Parameters.AddWithValue("@IdColaborador", permiso.Colaborador.IdColaborador);
                    command.Parameters.AddWithValue("@IdTipoPermiso", idTipoPermisoNoDefinido);
                    command.Parameters.AddWithValue("@Horas", permiso.Horas);
                    command.Parameters.AddWithValue("@Estado",permiso.Estado);
                    command.Parameters.AddWithValue("@Justificacion", permiso.Justificacion);
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

    public bool PermisoExistente(Permisos permiso)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM mydb.permisos WHERE id_colaborador = @IdColaborador AND (estado = 'Pendiente' OR estado = 'Aprobado') AND (fechaPermiso = @FechaPermiso)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdColaborador", permiso.Colaborador.IdColaborador);
                    command.Parameters.AddWithValue("@FechaPermiso", permiso.FechaPermiso);

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

    public bool EditarTipoPermiso(int idPermiso, int idTipoPermiso)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE mydb.permisos SET idtipoPermiso = @IdTipoPermiso WHERE idpermisos = @IdPermiso";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdPermiso", idPermiso);
                    command.Parameters.AddWithValue("@IdTipoPermiso", idTipoPermiso);
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

    public bool AprobarPermisoAdministrador(int idPermiso)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE mydb.permisos SET estado = @Estado WHERE idpermisos = @IdPermiso";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdPermiso", idPermiso);
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

    public bool AprobarPermisoJefatura(int idPermiso)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE mydb.permisos SET estado = @Estado WHERE idpermisos = @IdPermiso";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdPermiso", idPermiso);
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

    public bool RechazarPermiso(int idPermiso)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE mydb.permisos SET estado = @Estado WHERE idpermisos = @IdPermiso";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdPermiso", idPermiso);
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

    public bool EliminarPermiso(int idPermiso)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM mydb.permisos WHERE idpermisos = @IdPermiso";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdPermiso", idPermiso);
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

    public List<Permisos> ObtenerPermisos()
    {
        List<Permisos> permisos = new List<Permisos>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM mydb.permisos";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Permisos permiso = new Permisos
                            {
                                IdPermiso = reader.GetInt32(reader.GetOrdinal("idpermisos")),
                                FechaPermiso = reader.GetDateTime(reader.GetOrdinal("fechaPermiso")),
                                Colaborador = new Colaborador
                                {
                                    IdColaborador = reader.GetInt32(reader.GetOrdinal("id_colaborador"))
                                },
                                TipoPermiso = new TipoPermiso
                                {
                                    IdTipoPermiso = reader.GetInt32(reader.GetOrdinal("idtipoPermiso"))
                                },
                                Horas = reader.GetInt32(reader.GetOrdinal("horas")),
                                FechaGeneracion = reader.GetDateTime(reader.GetOrdinal("fechaGeneracion")),
                                Estado = reader.GetString(reader.GetOrdinal("estado")),
                                Justificacion = reader.GetString(reader.GetOrdinal("justificacion"))
                            };
                            permisos.Add(permiso);
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

        return permisos;
    }

    public List<Permisos> ObtenerPermisos(int? idColaborador)
    {
        List<Permisos> permisos = new List<Permisos>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM mydb.permisos WHERE id_colaborador = @IdColaborador";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdColaborador", idColaborador);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Permisos permiso = new Permisos
                            {
                                IdPermiso = reader.GetInt32(reader.GetOrdinal("idpermisos")),
                                FechaPermiso = reader.GetDateTime(reader.GetOrdinal("fechaPermiso")),
                                Colaborador = new Colaborador
                                {
                                    IdColaborador = reader.GetInt32(reader.GetOrdinal("id_colaborador"))
                                },
                                TipoPermiso = new TipoPermiso
                                {
                                    IdTipoPermiso = reader.GetInt32(reader.GetOrdinal("idtipoPermiso"))
                                },
                                Horas = reader.GetInt32(reader.GetOrdinal("horas")),
                                FechaGeneracion = reader.GetDateTime(reader.GetOrdinal("fechaGeneracion")),
                                Estado = reader.GetString(reader.GetOrdinal("estado")),
                                Justificacion = reader.GetString(reader.GetOrdinal("justificacion"))
                            };
                            permisos.Add(permiso);
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

        return permisos;
    }

    public List<Permisos> ObtenerPermisosParaAprobarPorAdministrador(int? idAdministrador)
    {
        List<Permisos> permisos = new List<Permisos>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("ObtenerPermisosParaAprobarPorAdministrador", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@IdAdministrador", idAdministrador));
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Permisos permiso = new Permisos
                            {
                                IdPermiso = reader.GetInt32(reader.GetOrdinal("idpermisos")),
                                FechaPermiso = reader.GetDateTime(reader.GetOrdinal("fechaPermiso")),
                                Colaborador = new Colaborador
                                {
                                    IdColaborador = reader.GetInt32(reader.GetOrdinal("id_colaborador"))
                                },
                                TipoPermiso = new TipoPermiso
                                {
                                    IdTipoPermiso = reader.GetInt32(reader.GetOrdinal("idtipoPermiso"))
                                },
                                Horas = reader.GetInt32(reader.GetOrdinal("horas")),
                                FechaGeneracion = reader.GetDateTime(reader.GetOrdinal("fechaGeneracion")),
                                Estado = reader.GetString(reader.GetOrdinal("estado")),
                                Justificacion = reader.GetString(reader.GetOrdinal("justificacion"))
                            };
                            permisos.Add(permiso);
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

        return permisos;
    }

    public List<Permisos> ObtenerPermisosParaAprobarPorJefatura(int? idJefatura)
    {
        List<Permisos> permisos = new List<Permisos>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("ObtenerPermisosParaAprobarPorJefatura", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@IdJefatura", idJefatura));
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Permisos permiso = new Permisos
                            {
                                IdPermiso = reader.GetInt32(reader.GetOrdinal("idpermisos")),
                                FechaPermiso = reader.GetDateTime(reader.GetOrdinal("fechaPermiso")),
                                Colaborador = new Colaborador
                                {
                                    IdColaborador = reader.GetInt32(reader.GetOrdinal("id_colaborador"))
                                },
                                TipoPermiso = new TipoPermiso
                                {
                                    IdTipoPermiso = reader.GetInt32(reader.GetOrdinal("idtipoPermiso"))
                                },
                                Horas = reader.GetInt32(reader.GetOrdinal("horas")),
                                FechaGeneracion = reader.GetDateTime(reader.GetOrdinal("fechaGeneracion")),
                                Estado = reader.GetString(reader.GetOrdinal("estado")),
                                Justificacion = reader.GetString(reader.GetOrdinal("justificacion"))
                            };
                            permisos.Add(permiso);
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
        return permisos;
    }
}
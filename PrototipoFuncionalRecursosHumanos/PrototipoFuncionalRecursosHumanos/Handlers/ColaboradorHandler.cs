using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using PrototipoFuncionalRecursosHumanos.Models;

public class ColaboradorHandler
{

    private readonly string connectionString = "";

    public ColaboradorHandler()
    {
        var builder = WebApplication.CreateBuilder();
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    }

    public bool AgregarColaborador(Colaborador colaborador)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("CrearColaborador", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Identificacion", colaborador.Persona.Identificacion));
                    command.Parameters.Add(new SqlParameter("@Nombre", colaborador.Persona.Nombre));
                    command.Parameters.Add(new SqlParameter("@Apellido1", colaborador.Persona.Apellido1));
                    command.Parameters.Add(new SqlParameter("@Apellido2", colaborador.Persona.Apellido2));
                    command.Parameters.Add(new SqlParameter("@FechaNacimiento", colaborador.Persona.FechaDeNacimiento));
                    command.Parameters.Add(new SqlParameter("@Correo", colaborador.Usuario.Correo));
                    command.Parameters.Add(new SqlParameter("@Contrasena", colaborador.Usuario.Contrasena));
                    command.Parameters.Add(new SqlParameter("@IdRolDeUsuario", colaborador.Usuario.RolDeUsuario.IdRolDeUsuario));
                    command.Parameters.Add(new SqlParameter("@IdDepartamento", colaborador.Departamento.IdDepartamento));
                    command.Parameters.Add(new SqlParameter("@IdPuesto", colaborador.Puesto.IdPuesto));
                    command.Parameters.Add(new SqlParameter("@TipoIdentificacion", colaborador.Persona.TipoIdentificacion));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Ocurrió un error al agregar un colaborador: " + ex.Message);
            exito = false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            exito = false;
        }

        return exito;
    }

    public bool EditarColaborador(Colaborador colaborador)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("EditarColaborador", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@IdColaborador", colaborador.IdColaborador));
                    command.Parameters.Add(new SqlParameter("@Identificacion", colaborador.Persona.Identificacion));
                    command.Parameters.Add(new SqlParameter("@Nombre", colaborador.Persona.Nombre));
                    command.Parameters.Add(new SqlParameter("@Apellido1", colaborador.Persona.Apellido1));
                    command.Parameters.Add(new SqlParameter("@Apellido2", colaborador.Persona.Apellido2));
                    command.Parameters.Add(new SqlParameter("@FechaNacimiento", colaborador.Persona.FechaDeNacimiento));
                    command.Parameters.Add(new SqlParameter("@Correo", colaborador.Usuario.Correo));
                    command.Parameters.Add(new SqlParameter("@Contrasena", colaborador.Usuario.Contrasena));
                    command.Parameters.Add(new SqlParameter("@IdRolDeUsuario", colaborador.Usuario.RolDeUsuario.IdRolDeUsuario));
                    command.Parameters.Add(new SqlParameter("@IdDepartamento", colaborador.Departamento.IdDepartamento));
                    command.Parameters.Add(new SqlParameter("@IdPuesto", colaborador.Puesto.IdPuesto));
                    command.Parameters.Add(new SqlParameter("@TipoIdentificacion", colaborador.Persona.TipoIdentificacion));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Ocurrió un error al editar un colaborador: " + ex.Message);
            exito = false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            exito = false;
        }

        return exito;
    }

    public bool CambiarEstadoColaborador(string nuevoEstado, int idColaborador)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE mydb.colaborador " +
                    "SET estado = @Estado WHERE id_colaborador = @IdColaborador";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Estado", nuevoEstado);
                    command.Parameters.AddWithValue("@IdColaborador", idColaborador);
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

    public List<Colaborador> ObtenerColaboradores()
    {
        List<Colaborador> listaColaboradores = new List<Colaborador>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("ObtenerColaboradores", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Colaborador colaborador = new Colaborador
                            {
                                IdColaborador = Convert.ToInt32(reader["id_colaborador"]),
                                FechaContratacion = Convert.ToDateTime(reader["fechaContratacion"]),
                                Persona = new Persona
                                {
                                    Identificacion = reader["identificacion"].ToString(),
                                    TipoIdentificacion = reader["tipoIdentificacion"].ToString(),
                                    Nombre = reader["nombre"].ToString(),
                                    Apellido1 = reader["apellido1"].ToString(),
                                    Apellido2 = reader["apellido2"].ToString(),
                                    FechaDeNacimiento = Convert.ToDateTime(reader["fecha_nacimiento"])
                                },
                                Usuario = new Usuario
                                {
                                    Correo = reader["correo"].ToString(),
                                    Contrasena = reader["contrasena"].ToString(),
                                    RolDeUsuario = new RolDeUsuario
                                    {
                                        IdRolDeUsuario = Convert.ToInt32(reader["idrolDeUsuario"]),
                                        Descripcion = reader["descripcion"].ToString()
                                    }
                                },
                                Departamento = new Departamento
                                {
                                    IdDepartamento = Convert.ToInt32(reader["iddepartamento"]),
                                    Nombre = reader["nombreDepartamento"].ToString(),
                                },
                                Puesto = new Puesto
                                {
                                    IdPuesto = Convert.ToInt32(reader["idpuesto"]),
                                    NombrePuesto = reader["nombrePuesto"].ToString(),
                                },
                                Estado = reader["estado"].ToString(),
                            };

                            listaColaboradores.Add(colaborador);
                        }
                    }
                    connection.Close();
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return listaColaboradores;
    }

    public List<Colaborador> ObtenerColaboradoresInactivos()
    {
        List<Colaborador> listaColaboradores = new List<Colaborador>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("ObtenerColaboradoresInactivos", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Colaborador colaborador = new Colaborador
                            {
                                IdColaborador = Convert.ToInt32(reader["id_colaborador"]),
                                FechaContratacion = Convert.ToDateTime(reader["fechaContratacion"]),
                                Persona = new Persona
                                {
                                    Identificacion = reader["identificacion"].ToString(),
                                    TipoIdentificacion = reader["tipoIdentificacion"].ToString(),
                                    Nombre = reader["nombre"].ToString(),
                                    Apellido1 = reader["apellido1"].ToString(),
                                    Apellido2 = reader["apellido2"].ToString(),
                                    FechaDeNacimiento = Convert.ToDateTime(reader["fecha_nacimiento"])
                                },
                                Usuario = new Usuario
                                {
                                    Correo = reader["correo"].ToString(),
                                    Contrasena = reader["contrasena"].ToString(),
                                    RolDeUsuario = new RolDeUsuario
                                    {
                                        IdRolDeUsuario = Convert.ToInt32(reader["idrolDeUsuario"]),
                                        Descripcion = reader["descripcion"].ToString()
                                    }
                                },
                                Departamento = new Departamento
                                {
                                    IdDepartamento = Convert.ToInt32(reader["iddepartamento"]),
                                    Nombre = reader["nombreDepartamento"].ToString(),
                                },
                                Puesto = new Puesto
                                {
                                    IdPuesto = Convert.ToInt32(reader["idpuesto"]),
                                    NombrePuesto = reader["nombrePuesto"].ToString(),
                                },
                                Estado = reader["estado"].ToString(),
                            };

                            listaColaboradores.Add(colaborador);
                        }
                    }
                    connection.Close();
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return listaColaboradores;
    }

    public Colaborador ObtenerColaborador(int idColaborador)
    {
        Colaborador colaborador = null;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("ObtenerColaboradorPorId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@IdColaborador", idColaborador));
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            colaborador = new Colaborador
                            {
                                IdColaborador = Convert.ToInt32(reader["id_colaborador"]),
                                FechaContratacion = Convert.ToDateTime(reader["fechaContratacion"]),
                                Persona = new Persona
                                {
                                    Identificacion = reader["identificacion"].ToString(),
                                    TipoIdentificacion = reader["tipoIdentificacion"].ToString(),
                                    Nombre = reader["nombre"].ToString(),
                                    Apellido1 = reader["apellido1"].ToString(),
                                    Apellido2 = reader["apellido2"].ToString(),
                                    FechaDeNacimiento = Convert.ToDateTime(reader["fecha_nacimiento"])
                                },
                                Usuario = new Usuario
                                {
                                    Correo = reader["correo"].ToString(),
                                    Contrasena = reader["contrasena"].ToString(),
                                    RolDeUsuario = new RolDeUsuario
                                    {
                                        IdRolDeUsuario = Convert.ToInt32(reader["idrolDeUsuario"]),
                                        Descripcion = reader["descripcion"].ToString()
                                    }
                                },
                                Departamento = new Departamento
                                {
                                    IdDepartamento = Convert.ToInt32(reader["iddepartamento"]),
                                    Nombre = reader["nombreDepartamento"].ToString(),
                                },
                                Puesto = new Puesto
                                {
                                    IdPuesto = Convert.ToInt32(reader["idpuesto"]),
                                    NombrePuesto = reader["nombrePuesto"].ToString(),
                                },
                                Estado = reader["estado"].ToString(),
                            };
                        }
                    }
                    connection.Close();
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Ocurrió un error al obtener el colaborador " + ex.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return colaborador;
    }

    public Colaborador ObtenerColaborador(string correo)
    {
        Colaborador colaborador = null;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("ObtenerColaboradorPorCorreo", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@Correo", correo));
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            colaborador = new Colaborador
                            {
                                IdColaborador = Convert.ToInt32(reader["id_colaborador"]),
                                FechaContratacion = Convert.ToDateTime(reader["fechaContratacion"]),
                                Persona = new Persona
                                {
                                    Identificacion = reader["identificacion"].ToString(),
                                    TipoIdentificacion = reader["tipoIdentificacion"].ToString(),
                                    Nombre = reader["nombre"].ToString(),
                                    Apellido1 = reader["apellido1"].ToString(),
                                    Apellido2 = reader["apellido2"].ToString(),
                                    FechaDeNacimiento = Convert.ToDateTime(reader["fecha_nacimiento"])
                                },
                                Usuario = new Usuario
                                {
                                    Correo = reader["correo"].ToString(),
                                    Contrasena = reader["contrasena"].ToString(),
                                    RolDeUsuario = new RolDeUsuario
                                    {
                                        IdRolDeUsuario = Convert.ToInt32(reader["idrolDeUsuario"]),
                                        Descripcion = reader["descripcion"].ToString()
                                    }
                                },
                                Departamento = new Departamento
                                {
                                    IdDepartamento = Convert.ToInt32(reader["iddepartamento"]),
                                    Nombre = reader["nombreDepartamento"].ToString(),
                                },
                                Puesto = new Puesto
                                {
                                    IdPuesto = Convert.ToInt32(reader["idpuesto"]),
                                    NombrePuesto = reader["nombrePuesto"].ToString(),
                                },
                                Estado = reader["estado"].ToString(),
                            };
                        }
                    }
                    connection.Close();
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Ocurrió un error al obtener el colaborador " + ex.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return colaborador;
    }

    public bool EliminarColaborador(int idColaborador)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("EliminarColaborador", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@IdColaborador", idColaborador));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Ocurrió un error al eliminar el colaborador: " + ex.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return exito;
    }
}
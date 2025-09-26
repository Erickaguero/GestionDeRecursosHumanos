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
                using (SqlCommand command = new SqlCommand("AsignarEstadoColaborador", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
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

    public bool ExisteColaboradorPorCorreo(string correo)
    {
        bool existe = false;
        try
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                using (SqlCommand comando = new SqlCommand("ObtenerColaboradorPorCorreo", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(new SqlParameter("@Correo", correo));
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            existe = true;
                        }
                    }
                    conexion.Close();
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Ocurrió un error al obtener el colaborador: " + e.Message);
        }

        return existe;
    }

    public bool ExisteJefeParaDepartamento(int idRolDeUsuario, int idDepartamento)
    {
        bool existe = false;
        try
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                string query = @"IF @IdRolDeUsuario = (SELECT idrolDeUsuario FROM mydb.roldeusuario WHERE descripcion = 'jefatura') 
                             BEGIN
                                SELECT 1 
                                FROM mydb.colaborador C
                                INNER JOIN mydb.usuario U ON C.idusuario = U.idusuario
                                INNER JOIN mydb.roldeusuario R ON U.idrolDeUsuario = R.idrolDeUsuario
                                WHERE C.iddepartamento = @IdDepartamento AND R.descripcion = 'jefatura' AND C.estado != 'inactivo'
                             END";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.Add(new SqlParameter("@IdDepartamento", idDepartamento));
                    comando.Parameters.Add(new SqlParameter("@IdRolDeUsuario", idRolDeUsuario));
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            existe = true;
                        }
                    }
                    conexion.Close();
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Ocurrió un error al verificar si existe un jefe para el departamento: " + e.Message);
        }

        return existe;
    }

    public bool ExisteColaboradorPorIdentificacion(string identificacion)
    {
        bool existe = false;
        try
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                using (SqlCommand comando = new SqlCommand("ObtenerColaboradorPorIdentificacion", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(new SqlParameter("@Identificacion", identificacion));
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            existe = true;
                        }
                    }
                    conexion.Close();
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Ocurrió un error al obtener el colaborador: " + e.Message);
        }

        return existe;
    }

    public float CalcularHorasTrabajadasPorColaboradorId(int colaboradorId)
    {
        float totalHorasTrabajadas = 0;

        using (SqlConnection conexion = new SqlConnection(connectionString))
        {
            using (SqlCommand comando = new SqlCommand("CalcularHorasTrabajadas", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add(new SqlParameter("@IdColaborador", colaboradorId));

                SqlParameter returnParameter = new SqlParameter();
                returnParameter.Direction = ParameterDirection.ReturnValue;
                comando.Parameters.Add(returnParameter);

                conexion.Open();
                comando.ExecuteNonQuery();

                if (returnParameter.Value != null)
                {
                    totalHorasTrabajadas = (float)Convert.ToDecimal(returnParameter.Value);
                }

                conexion.Close();
            }
        }

        return totalHorasTrabajadas;
    }

    public float CalcularHorasExtraPorColaboradorId(int colaboradorId)
    {
        float totalHorasExtra = 0;

        using (SqlConnection conexion = new SqlConnection(connectionString))
        {
            using (SqlCommand comando = new SqlCommand("CalcularHorasExtra", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add(new SqlParameter("@IdColaborador", colaboradorId));

                SqlParameter returnParameter = new SqlParameter();
                returnParameter.Direction = ParameterDirection.ReturnValue;
                comando.Parameters.Add(returnParameter);

                conexion.Open();
                comando.ExecuteNonQuery();

                if (returnParameter.Value != null)
                {
                    totalHorasExtra = (float)Convert.ToDecimal(returnParameter.Value);
                }

                conexion.Close();
            }
        }

        return totalHorasExtra;
    }

    public float CalcularHorasIncapacidadesPorColaboradorId(int colaboradorId)
    {
        float totalHorasIncapacidades = 0;

        using (SqlConnection conexion = new SqlConnection(connectionString))
        {
            using (SqlCommand comando = new SqlCommand("CalcularHorasIncapacidades", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add(new SqlParameter("@IdColaborador", colaboradorId));

                SqlParameter returnParameter = new SqlParameter();
                returnParameter.Direction = ParameterDirection.ReturnValue;
                comando.Parameters.Add(returnParameter);

                conexion.Open();
                comando.ExecuteNonQuery();

                if (returnParameter.Value != null)
                {
                    totalHorasIncapacidades = (float)Convert.ToDecimal(returnParameter.Value);
                }

                conexion.Close();
            }
        }

        return totalHorasIncapacidades;
    }

    public float CalcularHorasPermisoPorColaboradorId(int colaboradorId)
    {
        float totalHorasPermiso = 0;

        using (SqlConnection conexion = new SqlConnection(connectionString))
        {
            using (SqlCommand comando = new SqlCommand("CalcularHorasPermiso", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add(new SqlParameter("@IdColaborador", colaboradorId));

                SqlParameter returnParameter = new SqlParameter();
                returnParameter.Direction = ParameterDirection.ReturnValue;
                comando.Parameters.Add(returnParameter);

                conexion.Open();
                comando.ExecuteNonQuery();

                if (returnParameter.Value != null)
                {
                    totalHorasPermiso = (float)Convert.ToDecimal(returnParameter.Value);
                }

                conexion.Close();
            }
        }

        return totalHorasPermiso;
    }
}

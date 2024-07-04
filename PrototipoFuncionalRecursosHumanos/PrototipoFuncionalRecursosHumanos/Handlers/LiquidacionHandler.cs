using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using PrototipoFuncionalRecursosHumanos.Models;

public class LiquidacionHandler
{

    private readonly string connectionString = "";

    public LiquidacionHandler()
    {
        var builder = WebApplication.CreateBuilder();
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    }

    public bool GenerarLiquidacionConResponsabilidadColaborador(int idColaborador)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("CrearLiquidacionConResponsabilidadColaborador", connection))
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
            Console.WriteLine("Ocurrió un error al generar la liquidacion para el colaborador: " + ex.Message);
            exito = false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            exito = false;
        }

        return exito;
    }

    public bool GenerarLiquidacionSinResponsabilidadColaborador(int idColaborador)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("CrearLiquidacionSinResponsabilidadColaborador", connection))
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
            Console.WriteLine("Ocurrió un error al generar la liquidacion para el colaborador: " + ex.Message);
            exito = false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            exito = false;
        }

        return exito;
    }

    public bool GenerarLiquidacionRenunciaColaborador(int idColaborador)
    {
        bool exito = true;
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("CrearLiquidacionRenunciaColaborador", connection))
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
            Console.WriteLine("Ocurrió un error al generar la liquidacion para el colaborador: " + ex.Message);
            exito = false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            exito = false;
        }

        return exito;
    }

    public List<Liquidacion> ObtenerLiquidaciones()
    {
        List<Liquidacion> liquidaciones = new List<Liquidacion>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM mydb.liquidacion";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Liquidacion liquidacion = new Liquidacion
                            {
                                IdLiquidacion = reader.GetInt32(reader.GetOrdinal("idliquidacion")),
                                Colaborador = new Colaborador
                                {
                                    IdColaborador = reader.GetInt32(reader.GetOrdinal("id_colaborador"))
                                },
                                FechaGeneracion = reader.GetDateTime(reader.GetOrdinal("fechaGeneracion")),
                                Monto = reader.GetDouble(reader.GetOrdinal("monto")),
                                Preaviso = reader.GetDouble(reader.GetOrdinal("preaviso")),
                                Cesantia = reader.GetDouble(reader.GetOrdinal("cesantia")),
                                Aguinaldo = reader.GetDouble(reader.GetOrdinal("aguinaldo")),
                                VacacionesNoUsadas = reader.GetDouble(reader.GetOrdinal("horasVacacionesNoUsadas")),
                            };
                            liquidaciones.Add(liquidacion);
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
        return liquidaciones;
    }
}
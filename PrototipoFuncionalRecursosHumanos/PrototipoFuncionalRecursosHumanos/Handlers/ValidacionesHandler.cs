using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using PrototipoFuncionalRecursosHumanos.Models;
using Microsoft.AspNetCore.Components.Forms;

public class ValidacionesHandler
{

    private readonly string connectionString = "";

    public ValidacionesHandler()
    {
        var builder = WebApplication.CreateBuilder();
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    }

    public bool ValidarSiEsFeriado(DateTime Fecha)
    {
        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT COUNT(*) 
                    FROM mydb.feriado f
                    INNER JOIN mydb.tipoferiado tf ON f.idtipoferiado = tf.idtipoferiado
                    WHERE MONTH(f.fechaFeriado) = @Mes AND DAY(f.fechaFeriado) = @Dia
                    AND tf.descripcion = 'obligatorio'";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Mes", Fecha.Month);
                    command.Parameters.AddWithValue("@Dia", Fecha.Day);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public bool ValidarSiContieneFeriado(DateTime FechaInicio, DateTime FechaFin)
    {
        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT COUNT(*) 
                    FROM mydb.feriado f
                    INNER JOIN mydb.tipoferiado tf ON f.idtipoferiado = tf.idtipoferiado
                    WHERE (MONTH(f.fechaFeriado) > @MesInicio OR (MONTH(f.fechaFeriado) = @MesInicio AND DAY(f.fechaFeriado) >= @DiaInicio))
                    AND (MONTH(f.fechaFeriado) < @MesFin OR (MONTH(f.fechaFeriado) = @MesFin AND DAY(f.fechaFeriado) <= @DiaFin))
                    AND tf.descripcion = 'obligatorio'";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MesInicio", FechaInicio.Month);
                    command.Parameters.AddWithValue("@DiaInicio", FechaInicio.Day);
                    command.Parameters.AddWithValue("@MesFin", FechaFin.Month);
                    command.Parameters.AddWithValue("@DiaFin", FechaFin.Day);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public bool ValidarFechaUnica(DateTime Fecha, int idColaborador)
    {
        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Verificar en mydb.horasextra
                if (ExisteRegistroEnHorasExtra(Fecha, idColaborador, connection)) return false;

                // Verificar en mydb.permisos
                if (ExisteRegistroEnPermisos(Fecha, idColaborador, connection)) return false;

                // Verificar en mydb.incapacidades y mydb.vacaciones (rango de fechas)
                if (ExisteRegistroEnRango(Fecha, Fecha, idColaborador, connection)) return false;

                return true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public bool ValidarFechasUnicas(DateTime FechaInicio, DateTime FechaFin, int idColaborador)
    {
        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Verificar en mydb.horasextra para cada día en el rango
                DateTime fecha = FechaInicio;
                while (fecha <= FechaFin)
                {
                    if (ExisteRegistroEnHorasExtra(fecha, idColaborador, connection)) return false;
                    fecha = fecha.AddDays(1);
                }

                // Verificar en mydb.permisos para cada día en el rango
                fecha = FechaInicio;
                while (fecha <= FechaFin)
                {
                    if (ExisteRegistroEnPermisos(fecha, idColaborador, connection)) return false;
                    fecha = fecha.AddDays(1);
                }

                // Verificar en mydb.incapacidades y mydb.vacaciones (rango de fechas)
                if (ExisteRegistroEnRango(FechaInicio, FechaFin, idColaborador, connection)) return false;

                return true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    private bool ExisteRegistroEnHorasExtra(DateTime Fecha, int idColaborador, SqlConnection connection)
    {
        string query = @"
            SELECT COUNT(*) 
            FROM mydb.horasextra 
            WHERE CONVERT(DATE, fechaHoraExtra) = @Fecha AND id_colaborador = @idColaborador";
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Fecha", Fecha.Date);
            command.Parameters.AddWithValue("@idColaborador", idColaborador);
            int count = (int)command.ExecuteScalar();
            return count > 0;
        }
    }

    private bool ExisteRegistroEnPermisos(DateTime Fecha, int idColaborador, SqlConnection connection)
    {
        string query = @"
            SELECT COUNT(*) 
            FROM mydb.permisos 
            WHERE CONVERT(DATE, fechaPermiso) = @Fecha AND id_colaborador = @idColaborador";
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Fecha", Fecha.Date);
            command.Parameters.AddWithValue("@idColaborador", idColaborador);
            int count = (int)command.ExecuteScalar();
            return count > 0;
        }
    }

    private bool ExisteRegistroEnRango(DateTime FechaInicio, DateTime FechaFin, int idColaborador, SqlConnection connection)
    {
        string query = @"
            SELECT COUNT(*) 
            FROM (
                SELECT CONVERT(DATE, fechaInicio) AS fechaInicio, CONVERT(DATE, fechaFin) AS fechaFin 
                FROM mydb.incapacidades 
                WHERE id_colaborador = @idColaborador
                UNION ALL
                SELECT CONVERT(DATE, fechaInicio), CONVERT(DATE, fechaFin) 
                FROM mydb.vacaciones 
                WHERE id_colaborador = @idColaborador
            ) AS fechas
            WHERE fechaInicio <= CONVERT(DATE, @FechaFin) AND fechaFin >= CONVERT(DATE, @FechaInicio);";
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@FechaInicio", FechaInicio.Date);
            command.Parameters.AddWithValue("@FechaFin", FechaFin.Date);
            command.Parameters.AddWithValue("@idColaborador", idColaborador);
            int count = (int)command.ExecuteScalar();
            return count > 0;
        }
    }
}
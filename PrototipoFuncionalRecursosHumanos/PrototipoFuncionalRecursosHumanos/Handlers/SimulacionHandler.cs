using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using PrototipoFuncionalRecursosHumanos.Models;

namespace PrototipoFuncionalRecursosHumanos.Handlers
{
    public class SimulacionHandler
    {

        private readonly string connectionString = "";

        public SimulacionHandler()
        {
            var builder = WebApplication.CreateBuilder();
            connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        }

        public bool GenerarAsistencia(int idColaborador)
        {
            bool exito = true;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "EXEC dbo.GenerarAsistenciasColaborador @IdColaborador";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
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

        public bool ValidarFuncionamientoHorasExtra(int idColaborador)
        {
            bool exito = true;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "EXEC dbo.ValidarFuncionamientoHorasExtra @IdColaborador";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
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

        public bool GenerarFeriados()
        {
            bool exito = true;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "EXEC dbo.ValidarFuncionamientoFeriados";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
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

        public bool ReiniciarFeriados()
        {
            bool exito = true;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "EXEC dbo.ReiniciarFeriadosConValoresPorDefecto";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
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

        public decimal ValidarFuncionamientoAguinaldo(int idColaborador)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "EXEC dbo.ValidarFuncionamientoAguinaldo @IdColaborador";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdColaborador", idColaborador);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetDecimal(0);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return 0;
        }

        public decimal CalcularImpuestoRentaQuincenal(double salarioBrutoQuincenal)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "EXEC dbo.CalcularImpuestoRentaQuincenal @salarioBrutoQuincenal, @DeduccionRentaCalculada OUTPUT";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@salarioBrutoQuincenal", salarioBrutoQuincenal);

                        SqlParameter outputParam = new SqlParameter("@DeduccionRentaCalculada", SqlDbType.Float);
                        outputParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outputParam);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return (decimal)reader.GetDouble(0);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return 0;
        }

        public bool ModificarFechaContratacionPorAnio(int idColaborador)
        {
            bool exito = true;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE mydb.colaborador SET fechaContratacion = DATEADD(YEAR,-1,fechaContratacion) WHERE id_colaborador = @idColaborador";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idColaborador", idColaborador);

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

        public bool ModificarFechaContratacionPorMes(int idColaborador)
        {
            bool exito = true;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE mydb.colaborador SET fechaContratacion = DATEADD(MONTH,-1,fechaContratacion) WHERE id_colaborador = @idColaborador";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idColaborador", idColaborador);

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

        public bool SimularPlanillaColaborador(int idColaborador, float horasExtra, float horasIncapacidades, float horasPermiso, float horasTrabajadas, float horasVacaciones)
        {
            bool exito = true;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("dbo.SimularPlanillaColaborador", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@IdColaborador", idColaborador));
                        command.Parameters.Add(new SqlParameter("@HorasExtra", horasExtra));
                        command.Parameters.Add(new SqlParameter("@HorasIncapacidades", horasIncapacidades));
                        command.Parameters.Add(new SqlParameter("@HorasPermiso", horasPermiso));
                        command.Parameters.Add(new SqlParameter("@HorasTrabajadas", horasTrabajadas));
                        command.Parameters.Add(new SqlParameter("@HorasVacaciones", horasVacaciones));

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

        public bool EliminarPlanillas()
        {
            bool exito = true;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM mydb.planilla";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
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

        public bool EliminarAguinaldos()
        {
            bool exito = true;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM mydb.aguinaldo";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
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

        public bool EliminarLiquidaciones()
        {
            bool exito = true;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM mydb.liquidacion";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
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
    }
}

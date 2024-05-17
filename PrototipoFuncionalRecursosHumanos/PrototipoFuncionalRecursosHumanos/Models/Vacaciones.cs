namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class Vacaciones
    {
        public int? IdVacaciones { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? Estado { get; set; }
        public int? IdColaborador { get; set; }

        public Vacaciones()
        {
        }

        public Vacaciones(int idVacaciones, DateTime fechaInicio, DateTime fechaFin, string estado, int idColaborador)
        {
            IdVacaciones = idVacaciones;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            Estado = estado;
            IdColaborador = idColaborador;
        }
    }
}

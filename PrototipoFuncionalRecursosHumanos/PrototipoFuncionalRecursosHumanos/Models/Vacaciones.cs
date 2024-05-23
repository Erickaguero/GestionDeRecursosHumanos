namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class Vacaciones
    {
        public int? IdVacaciones { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? Estado { get; set; }
        public Colaborador? Colaborador { get; set; }

        public Vacaciones()
        {
        }

        public Vacaciones(int idVacaciones, DateTime fechaInicio, DateTime fechaFin, string estado, Colaborador colaborador)
        {
            IdVacaciones = idVacaciones;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            Estado = estado;
            Colaborador = colaborador;
        }
    }
}

namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class Asistencia
    {
        public int? IdAsistencia { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public DateTime? FechaSalida { get; set; }
        public Colaborador? Colaborador { get; set; }

        public Asistencia()
        {
        }

        public Asistencia(int? idAsistencia, DateTime? fechaIngreso, DateTime? fechaSalida, Colaborador? colaborador)
        {
            IdAsistencia = idAsistencia;
            FechaIngreso = fechaIngreso;
            FechaSalida = fechaSalida;
            Colaborador = colaborador;
        }
    }
}

namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class HorasExtra
    {
        public int ?IdHorasExtra { get; set; }
        public DateTime? FechaHorasExtra { get; set; }
        public Colaborador? Colaborador { get; set; }
        public int? Horas { get; set; }
        public string? Estado { get; set; }
        public string? Justificacion { get; set; }

        public HorasExtra()
        {
        }

        public HorasExtra(int? idHorasExtra, DateTime? fechaHorasExtra, Colaborador? colaborador, int? horas, string? estado, string? justificacion)
        {
            IdHorasExtra = idHorasExtra;
            FechaHorasExtra = fechaHorasExtra;
            Colaborador = colaborador;
            Horas = horas;
            Estado = estado;
            Justificacion = justificacion;
        }
    }
}

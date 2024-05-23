namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class Permisos
    {
        public int? IdPermiso { get; set; }
        public DateTime? FechaPermiso { get; set; }
        public Colaborador? Colaborador { get; set; }
        public TipoPermiso? TipoPermiso { get; set; }
        public int? Horas { get; set; }
        public DateTime? FechaGeneracion { get; set;}
        public string? Estado { get; set; }
        public string? Justificacion { get; set; }

        public Permisos()
        {
        }

        public Permisos(int? idPermiso, DateTime? fechaPermiso, Colaborador? colaborador, TipoPermiso? tipoPermiso, int? horas, DateTime? fechaGeneracion, string? estado, string? justificacion)
        {
            IdPermiso = idPermiso;
            FechaPermiso = fechaPermiso;
            Colaborador = colaborador;
            TipoPermiso = tipoPermiso;
            Horas = horas;
            FechaGeneracion = fechaGeneracion;
            Estado = estado;
            Justificacion = justificacion;
        }
    }
}

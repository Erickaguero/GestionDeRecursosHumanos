using System.ComponentModel.DataAnnotations;

namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class Permisos
    {
        public int? IdPermiso { get; set; }
        public DateTime? FechaPermiso { get; set; }
        public Colaborador? Colaborador { get; set; }
        public TipoPermiso? TipoPermiso { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "La cantidad de horas solo debe contener números.")]
        public int? Horas { get; set; }
        public DateTime? FechaGeneracion { get; set;}
        public string? Estado { get; set; }

        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9\s]*$", ErrorMessage = "La justificación ingresada es invalida.")]
        [MaxLength(125, ErrorMessage = "La justificación es demasiado larga.")]
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

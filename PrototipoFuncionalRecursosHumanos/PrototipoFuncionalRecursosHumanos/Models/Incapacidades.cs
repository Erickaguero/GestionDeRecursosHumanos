using System.ComponentModel.DataAnnotations;

namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class Incapacidades
    {
        public int? IdIncapacidad { get; set; }
        public Colaborador? Colaborador { get; set; }
        public TipoIncapacidad? TipoIncapacidad { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? Estado { get; set; }

        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9\s]*$", ErrorMessage = "La justificación ingresada es invalida.")]
        [MaxLength(125, ErrorMessage = "La justificación es demasiado larga.")]
        public string? Justificacion { get; set; }

        public Incapacidades()
        {
        }

        public Incapacidades(int? idIncapacidad, Colaborador? colaborador, TipoIncapacidad? tipoIncapacidad, DateTime? fechaInicio, DateTime? fechaFin, string? estado, string? justificacion)
        {
            IdIncapacidad = idIncapacidad;
            Colaborador = colaborador;
            TipoIncapacidad = tipoIncapacidad;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            Estado = estado;
            Justificacion = justificacion;
        }
    }
}

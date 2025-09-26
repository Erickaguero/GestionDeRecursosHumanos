using System.ComponentModel.DataAnnotations;

namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class HorasExtra
    {
        public int ?IdHorasExtra { get; set; }
        public DateTime? FechaHorasExtra { get; set; }
        public Colaborador? Colaborador { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "La cantidad de horas solo debe contener números.")]
        public int? Horas { get; set; }
        public string? Estado { get; set; }

        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9\s]*$", ErrorMessage = "La justificación ingresada es invalida.")]
        [MaxLength(125, ErrorMessage = "La justificación es demasiado larga.")]
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

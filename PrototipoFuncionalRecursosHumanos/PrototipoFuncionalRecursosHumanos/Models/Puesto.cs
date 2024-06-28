using System.ComponentModel.DataAnnotations;

namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class Puesto
    {
        public int? IdPuesto { get; set; }

        [MaxLength(80, ErrorMessage = "El nombre del puesto es demasiado largo.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "El nombre del puesto solo debe contener letras.")]
        public string? NombrePuesto { get; set; }

        public double? CostoPorHora { get; set; }
        public Puesto()
        {
        }
        public Puesto(int? idPuesto, string nombrePuesto, double costoPorHora)
        {
            IdPuesto = idPuesto;
            NombrePuesto = nombrePuesto;
            CostoPorHora = costoPorHora;
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class Persona
    {
        public int? IdPersona { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "El número de identificación solo debe contener números.")]
        [MaxLength(20, ErrorMessage = "El número de identificación es demasiado largo.")]
        public string? Identificacion { get; set; }

        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]*$", ErrorMessage = "El nombre solo debe contener letras.")]
        [MaxLength(45, ErrorMessage = "El nombre es demasiado largo.")]
        public string? Nombre { get; set; }

        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]*$", ErrorMessage = "El apellido solo debe contener letras.")]
        [MaxLength(45, ErrorMessage = "El apellido es demasiado largo.")]
        public string? Apellido1 { get; set; }

        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]*$", ErrorMessage = "El apellido solo debe contener letras.")]
        [MaxLength(45, ErrorMessage = "El apellido es demasiado largo.")]
        public string? Apellido2 { get; set; }

        public DateTime? FechaDeNacimiento { get; set; }

        public Persona()
        {
        }

        public Persona(int idPersona, string identificacion, string nombre, string apellido1, string apellido2, DateTime fechaDeNacimiento)
        {
            IdPersona = idPersona;
            Identificacion = identificacion;
            Nombre = nombre;
            Apellido1 = apellido1;
            Apellido2 = apellido2;
            FechaDeNacimiento = fechaDeNacimiento;
        }
    }
}
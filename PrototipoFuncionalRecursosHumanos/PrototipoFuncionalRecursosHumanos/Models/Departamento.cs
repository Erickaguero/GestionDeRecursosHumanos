using System.ComponentModel.DataAnnotations;

namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class Departamento
    {
        public int? IdDepartamento { get; set; }
        [MaxLength(45, ErrorMessage = "El nombre del departamento es demasiado largo.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "El nombre del departamento solo debe contener letras.")]
        public string? Nombre { get; set; }

        public Departamento()
        {
        }

        public Departamento(int idDepartamento, string nombre)
        {
            IdDepartamento = idDepartamento;
            Nombre = nombre;
        }
    }
}

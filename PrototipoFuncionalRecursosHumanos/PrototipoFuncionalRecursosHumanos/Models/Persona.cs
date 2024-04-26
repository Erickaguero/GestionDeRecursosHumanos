namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class Persona
    {
        public int? IdPersona { get; set; }
        public string? Identificacion { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido1 { get; set; }
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

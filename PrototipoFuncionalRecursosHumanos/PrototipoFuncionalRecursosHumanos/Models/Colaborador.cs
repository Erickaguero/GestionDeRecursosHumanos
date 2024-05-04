namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class Colaborador
    {
        public int? IdColaborador { get; set; }
        public Persona? Persona { get; set; }
        public Usuario? Usuario { get; set; }

        public Departamento? Departamento { get; set; }
        
        public DateTime? FechaContratacion { get; set; }

        public Colaborador()
        {
            Persona = new Persona();
            Usuario = new Usuario();
            Departamento = new Departamento();
        }

        public Colaborador(int idColaborador, Persona persona, Usuario usuario, Departamento departamento, DateTime fechaContratacion)
        {
            IdColaborador = idColaborador;
            Persona = persona;
            Usuario = usuario;
            FechaContratacion = fechaContratacion;
            Departamento = departamento;
        }
    }
}

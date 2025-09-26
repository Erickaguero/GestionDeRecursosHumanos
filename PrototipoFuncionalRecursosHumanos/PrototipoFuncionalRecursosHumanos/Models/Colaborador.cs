namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class Colaborador
    {
        public int? IdColaborador { get; set; }
        public Persona? Persona { get; set; }
        public Usuario? Usuario { get; set; }

        public Departamento? Departamento { get; set; }
        
        public Puesto? Puesto { get; set; }

        public DateTime? FechaContratacion { get; set; }

        public string? Estado { get; set; }

        public Colaborador()
        {
            Persona = new Persona();
            Usuario = new Usuario();
            Departamento = new Departamento();
        }

        public Colaborador(int? idColaborador, Persona persona, Usuario usuario, Departamento departamento, Puesto puesto, DateTime fechaContratacion, string estado)
        {
            IdColaborador = idColaborador;
            Persona = persona;
            Usuario = usuario;
            Departamento = departamento;
            Puesto = puesto;
            FechaContratacion = fechaContratacion;
            Estado = estado;
        }
    }
}

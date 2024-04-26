namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class Colaborador
    {
        public int? IdColaborador { get; set; }
        public Persona? Persona { get; set; }
        public Usuario? Usuario { get; set; }
        public string? RolDeUsuario { get; set; }
        
        public DateTime? FechaContratacion { get; set; }

        public Colaborador()
        {
            Persona = new Persona();
            Usuario = new Usuario();
        }

        public Colaborador(int idColaborador, Persona persona, Usuario usuario, string rolDeUsuario, DateTime fechaContratacion)
        {
            IdColaborador = idColaborador;
            Persona = persona;
            Usuario = usuario;
            RolDeUsuario = rolDeUsuario;
            FechaContratacion = fechaContratacion;
        }
    }
}

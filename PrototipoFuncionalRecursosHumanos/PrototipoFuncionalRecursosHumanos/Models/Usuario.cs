namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class Usuario
    {
        public int? IdUsuario { get; set; }
        public int? IdPersona { get; set; }
        public RolDeUsuario? RolDeUsuario { get; set; }
        public string? Correo { get; set; }
        public string? Contrasena { get; set; }

        public Usuario()
        {
        }
        public Usuario(int idUsuario, int idPersona, RolDeUsuario rolDeUsuario, string correo, string contrasena)
        {
            IdUsuario = idUsuario;
            IdPersona = idPersona;
            RolDeUsuario = rolDeUsuario;
            this.Correo = correo;
            this.Contrasena = contrasena;
        }
    }
}

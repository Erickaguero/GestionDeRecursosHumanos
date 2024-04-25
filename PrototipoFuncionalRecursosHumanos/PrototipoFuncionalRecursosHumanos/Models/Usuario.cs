namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public int IdPersona { get; set; }
        public int IdRolDeUsuario { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }

        public Usuario()
        {
        }
        public Usuario(int idUsuario, int idPersona, int idRolDeUsuario, string correo, string contrasena)
        {
            IdUsuario = idUsuario;
            IdPersona = idPersona;
            IdRolDeUsuario = idRolDeUsuario;
            this.Correo = correo;
            this.Contrasena = contrasena;
        }
    }
}

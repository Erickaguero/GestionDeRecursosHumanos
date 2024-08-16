using System.ComponentModel.DataAnnotations;

namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class Usuario
    {
        public int? IdUsuario { get; set; }
        public int? IdPersona { get; set; }
        public RolDeUsuario? RolDeUsuario { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9._%+ñÑáéíóúÁÉÍÓÚ-]+@[a-zA-Z0-9.-ñÑáéíóúÁÉÍÓÚ-]+\.[a-zA-Z]{2,}$", ErrorMessage = "El formato del correo es inválido")]
        [MaxLength(45, ErrorMessage = "El correo es demasiado largo.")]
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

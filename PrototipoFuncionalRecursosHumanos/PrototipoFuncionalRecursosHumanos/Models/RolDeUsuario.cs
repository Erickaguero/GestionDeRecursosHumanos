namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class RolDeUsuario
    {
        public int? IdRolDeUsuario { get; set; }
        public string? Descripcion { get; set; }

        public RolDeUsuario()
        {
        }

        public RolDeUsuario(int idRolDeUsuario)
        {
            IdRolDeUsuario = idRolDeUsuario;
            Descripcion = string.Empty;
        }

        public RolDeUsuario(int idRolDeUsuario, string descripcion)
        {
            IdRolDeUsuario = idRolDeUsuario;
            Descripcion = descripcion;
        }
    }
}

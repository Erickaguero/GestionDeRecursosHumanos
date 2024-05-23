namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class TipoPermiso
    {
        public int? IdTipoPermiso { get; set; }
        public string? Descripcion { get; set; }

        public TipoPermiso()
        {
        }

        public TipoPermiso(int idTipoPermiso, string descripcion)
        {
            IdTipoPermiso = idTipoPermiso;
            Descripcion = descripcion;
        }
    }
}

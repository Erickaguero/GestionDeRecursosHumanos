namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class TipoIncapacidad
    {
        public int? IdTipoIncapacidad { get; set; }
        public string? Descripcion { get; set; }

        public TipoIncapacidad()
        {
        }

        public TipoIncapacidad(int? idTipoIncapacidad, string? descripcion)
        {
            IdTipoIncapacidad = idTipoIncapacidad;
            Descripcion = descripcion;
        }
    }
}

namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class MenuPrincipal
    {
        public string? NombreCompletoColaborador { get; set; }
        public float? HorasTrabajadasPeriodo { get; set; }
        public float? HorasExtrasPeriodo { get; set; }
        public float? HorasIncapacidadPeriodo { get; set; }
        public float? HorasPermisoPeriodo { get; set; }
        public List<string> ?UbicacionesImagenesColaboradores { get; set; }

        public MenuPrincipal()
        {
        }

        public MenuPrincipal(string? nombreCompletoColaborador, float? horasTrabajadasPeriodo, float? horasExtrasPeriodo, float? horasIncapacidadPeriodo, float? horasPermisoPeriodo, List<string>? ubicacionesImagenesColaboradores)
        {
            NombreCompletoColaborador = nombreCompletoColaborador;
            HorasTrabajadasPeriodo = horasTrabajadasPeriodo;
            HorasExtrasPeriodo = horasExtrasPeriodo;
            HorasIncapacidadPeriodo = horasIncapacidadPeriodo;
            HorasPermisoPeriodo = horasPermisoPeriodo;
            UbicacionesImagenesColaboradores = ubicacionesImagenesColaboradores;
        }
    }
}

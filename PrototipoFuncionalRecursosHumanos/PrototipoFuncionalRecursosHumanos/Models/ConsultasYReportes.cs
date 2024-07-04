namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class ConsultasYReportes
    {
        public List<Asistencia>? Asistencias {get; set;}
        public List<Planilla>? Planillas { get; set; }
        public List<Aguinaldo>? Aguinaldos { get; set; }
        public List<Liquidacion>? Liquidaciones { get; set; }
        public List<Colaborador>? ColaboradoresActivos { get; set; }
        public List<Colaborador>? ColaboradoresInactivos { get; set; }

        public List<HorasExtra>? HorasExtras { get; set; }
        public List<Permisos>? Permisos { get; set; }
        public List<Incapacidades>? Incapacidades { get; set; }
        public List<Vacaciones>? Vacaciones { get; set; }

        public string? ElementoADesplegar { get; set; }

        public bool DesplegarBotones { get; set; }
        public ConsultasYReportes() {
        }
        
        public ConsultasYReportes (List<Asistencia> asistencias, List<Planilla> planillas, List<Aguinaldo> aguinaldos, List<Liquidacion> liquidaciones, List<Colaborador> colaboradoresActivos, List<Colaborador> colaboradoresInactivos, List<HorasExtra> horasExtras, List<Permisos> permisos, List<Incapacidades> incapacidades, List<Vacaciones> vacaciones, string elementoADesplegar, bool desplegarBotones)
        {
            Asistencias = asistencias;
            Planillas = planillas;
            Aguinaldos = aguinaldos;
            Liquidaciones = liquidaciones;
            ColaboradoresActivos = colaboradoresActivos;
            ColaboradoresInactivos = colaboradoresInactivos;
            HorasExtras = horasExtras;
            Permisos = permisos;
            Incapacidades = incapacidades;
            Vacaciones = vacaciones;
            ElementoADesplegar = elementoADesplegar;
            DesplegarBotones = desplegarBotones;
        }
    }
}

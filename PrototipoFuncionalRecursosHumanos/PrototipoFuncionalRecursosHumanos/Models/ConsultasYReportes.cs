namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class ConsultasYReportes
    {
        public List<Asistencia>? Asistencias {get; set;}
        public List<Planilla>? Planillas { get; set; }
        public List<Aguinaldo>? Aguinaldos { get; set; }
        public List<Liquidacion>? Liquidaciones { get; set; }

        public List<Colaborador>? Colaboradores { get; set; }

        public string? ElementoADesplegar { get; set; }

        public bool DesplegarBotones { get; set; }
        public ConsultasYReportes() {
        }

        public ConsultasYReportes(List<Asistencia>? asistencias, List<Planilla>? planillas, List<Aguinaldo>? aguinaldos, List<Liquidacion>? liquidaciones, List<Colaborador>? colaboradores, string? elementoADesplegar, bool desplegarBotones)
        {
            Asistencias = asistencias;
            Planillas = planillas;
            Aguinaldos = aguinaldos;
            Liquidaciones = liquidaciones;
            Colaboradores = colaboradores;
            ElementoADesplegar = elementoADesplegar;
            DesplegarBotones = desplegarBotones;
        }
    }
}

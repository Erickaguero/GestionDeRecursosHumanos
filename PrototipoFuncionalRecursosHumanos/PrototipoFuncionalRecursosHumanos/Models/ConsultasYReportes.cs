using System.ComponentModel.DataAnnotations;

namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class ConsultasYReportes
    {
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9\s]*$", ErrorMessage = "El filtro ingresado es invalido.")]
        [MaxLength(45, ErrorMessage = "El filtro ingresado es demasiado largo.")]
        public string? Filtro { get; set; }
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
        
        public ConsultasYReportes (string Filtro, List<Asistencia> asistencias, List<Planilla> planillas, List<Aguinaldo> aguinaldos, List<Liquidacion> liquidaciones, List<Colaborador> colaboradoresActivos, List<Colaborador> colaboradoresInactivos, List<HorasExtra> horasExtras, List<Permisos> permisos, List<Incapacidades> incapacidades, List<Vacaciones> vacaciones, string elementoADesplegar, bool desplegarBotones)
        {
            this.Filtro = Filtro;
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

namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class Planilla
    {
        public int? IdPlanilla { get; set; }
        public Colaborador? Colaborador { get; set; }
        public DateTime? FechaGeneracion { get; set; }
        public double? Monto { get; set; }

        public double? HorasExtra { get; set; }
        public double? HorasIncapacidades { get; set; }
        public double? HorasPermiso { get; set; }
        public double? HorasTrabajadas { get; set; }
        public double? HorasVacaciones { get; set; }
        public double? DeduccionCCSS { get; set; }
        public double? DeduccionRenta { get; set; }
        public double? SalarioBruto { get; set; }

        public Planilla()
        {
        }

        public Planilla(int? idPlanilla, Colaborador? colaborador, DateTime? fechaGeneracion, double? monto, double? deduccionCCSS, double? deduccionRenta, double? salarioBruto)
        {
            IdPlanilla = idPlanilla;
            Colaborador = colaborador;
            FechaGeneracion = fechaGeneracion;
            Monto = monto;
            DeduccionCCSS = deduccionCCSS;
            DeduccionRenta = deduccionRenta;
            SalarioBruto = salarioBruto;
        }
    }
}

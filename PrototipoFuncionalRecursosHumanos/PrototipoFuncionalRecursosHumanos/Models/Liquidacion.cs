namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class Liquidacion
    {
        public int? IdLiquidacion { get; set; }
        public Colaborador? Colaborador { get; set; }
        public DateTime? FechaGeneracion { get; set; }
        public double? Monto { get; set; }
        public double? Preaviso { get; set; }
        public double? Cesantia { get; set; }
        public double? VacacionesNoUsadas { get; set; }
        public double? Aguinaldo { get; set; }

        public Liquidacion()
        {
        }
        public Liquidacion(int? idLiquidacion, Colaborador colaborador, DateTime fechaGeneracion, double monto, double preaviso, double cesantia, double vacacionesNoUsadas, double aguinaldo)
        {
            IdLiquidacion = idLiquidacion;
            Colaborador = colaborador;
            FechaGeneracion = fechaGeneracion;
            Monto = monto;
            Preaviso = preaviso;
            Cesantia = cesantia;
            VacacionesNoUsadas = vacacionesNoUsadas;
            Aguinaldo = aguinaldo;
        }
    }
}

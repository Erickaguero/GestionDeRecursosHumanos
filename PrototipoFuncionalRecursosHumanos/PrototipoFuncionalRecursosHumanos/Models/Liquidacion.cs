namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class Liquidacion
    {
        public int? IdLiquidacion { get; set; }
        public Colaborador? Colaborador { get; set; }
        public DateTime? FechaGeneracion { get; set; }
        public double? Monto { get; set; }

        public Liquidacion()
        {
        }
        public Liquidacion(int? idLiquidacion, Colaborador? colaborador, DateTime? fechaGeneracion, double? monto)
        {
            IdLiquidacion = idLiquidacion;
            Colaborador = colaborador;
            FechaGeneracion = fechaGeneracion;
            Monto = monto;
        }
    }
}

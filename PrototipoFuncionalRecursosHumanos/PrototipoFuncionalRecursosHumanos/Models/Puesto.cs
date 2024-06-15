namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class Puesto
    {
        public int? IdPuesto { get; set; }
        public string? NombrePuesto { get; set; }
        public double? CostoPorHora { get; set; }
        public Puesto()
        {
        }
        public Puesto(int? idPuesto, string nombrePuesto, double costoPorHora)
        {
            IdPuesto = idPuesto;
            NombrePuesto = nombrePuesto;
            CostoPorHora = costoPorHora;
        }
    }
}

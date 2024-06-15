namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class Aguinaldo
    {
        public int? IdAguinaldo { get; set; }
        public Colaborador? Colaborador { get; set; }
        public DateTime? FechaGeneracion { get; set; }
        public double? Monto { get; set; }

        public Aguinaldo()
        {
        }

        public Aguinaldo(int? idAguinaldo, Colaborador? colaborador, DateTime? fechaGeneracion, double? monto)
        {
            IdAguinaldo = idAguinaldo;
            Colaborador = colaborador;
            FechaGeneracion = fechaGeneracion;
            Monto = monto;
        }
    }
}

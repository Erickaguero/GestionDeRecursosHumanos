namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class Evaluacion
    {
        public int? IdEvaluacion { get; set; }
        public DateTime? FechaEvaluacion { get; set; }
        public Colaborador? Colaborador { get; set; }
        
        public double? NotaEvaluacion { get; set; }

        public List<Pregunta>? Preguntas { get; set; }

        public Evaluacion()
        {
            NotaEvaluacion = 0;
            Preguntas = new List<Pregunta>();
        }

        public Evaluacion(int? idEvaluacion, DateTime? fechaEvaluacion, Colaborador? colaborador, double? promedioEvaluacion)
        {
            IdEvaluacion = idEvaluacion;
            FechaEvaluacion = fechaEvaluacion;
            Colaborador = colaborador;
            NotaEvaluacion = promedioEvaluacion;
            Preguntas = new List<Pregunta>();
        }
    }
}

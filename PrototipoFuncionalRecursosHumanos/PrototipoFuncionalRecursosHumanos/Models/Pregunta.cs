namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class Pregunta
    {
        public string? Texto { get; set; }
        public int? Respuesta { get; set; }

        public Pregunta()
        {
        }
        
        public Pregunta(string texto, int respuesta = 0)
        {
            Texto = texto;
            Respuesta = respuesta;
        }
    }
}

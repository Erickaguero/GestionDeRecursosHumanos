namespace PrototipoFuncionalRecursosHumanos.Services
{
    public class Alertas
    {
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public string TipoAlerta { get; set; } // success, error, warning, info, question

        public Alertas(string titulo, string mensaje, string tipoAlerta)
        {
            Titulo = titulo;
            Mensaje = mensaje;
            TipoAlerta = tipoAlerta;
        }

        public static Alertas Exito(string mensaje) => new Alertas("Éxito", mensaje, "success");
        public static Alertas Error(string mensaje) => new Alertas("Error", mensaje, "error");
    }
}
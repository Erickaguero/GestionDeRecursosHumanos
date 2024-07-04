using PrototipoFuncionalRecursosHumanos.Services;

public static class Autorizador
{
    private static Autenticador autenticador = new Autenticador();
    private static ColaboradorHandler colaboradorHandler = new ColaboradorHandler();
    public static string ObtenerRolColaborador(HttpRequest request) {
        var correo = autenticador.ValidarToken(request);
        if (correo == null) return "";
        return colaboradorHandler.ObtenerColaborador(correo).Usuario.RolDeUsuario.Descripcion;
    }
    public static string ObtenerEstadoColaborador(HttpRequest request)
    {
        var correo = autenticador.ValidarToken(request);
        if (correo == null) return "";
        return colaboradorHandler.ObtenerColaborador(correo).Estado;
    }
}


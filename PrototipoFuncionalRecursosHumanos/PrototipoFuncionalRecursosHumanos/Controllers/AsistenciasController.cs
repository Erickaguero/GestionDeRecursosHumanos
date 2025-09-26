using Microsoft.AspNetCore.Mvc;
using PrototipoFuncionalRecursosHumanos.Models;
using PrototipoFuncionalRecursosHumanos.Services;

namespace PrototipoFuncionalRecursosHumanos.Controllers
{
    public class AsistenciasController : Controller
    {
        private Autenticador authenticator = new Autenticador();
        private AsistenciasHandler asistenciasHandler = new AsistenciasHandler();
        private ColaboradorHandler colaboradorHandler = new ColaboradorHandler();

        public IActionResult ListaAsistencias()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            List<Asistencia> asistencias = asistenciasHandler.ObtenerAsistencias((int)colaborador.IdColaborador);
            var nombreColaborador = colaborador.Persona.Nombre + " " + colaborador.Persona.Apellido1 + " " + colaborador.Persona.Apellido2;
            ViewBag.NombreColaborador = nombreColaborador;
            return View(asistencias);
        }
    }
}

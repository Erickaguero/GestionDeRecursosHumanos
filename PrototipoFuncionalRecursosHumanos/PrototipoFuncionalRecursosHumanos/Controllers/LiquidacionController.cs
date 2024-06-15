using Microsoft.AspNetCore.Mvc;
using PrototipoFuncionalRecursosHumanos.Models;
using PrototipoFuncionalRecursosHumanos.Services;

namespace PrototipoFuncionalRecursosHumanos.Controllers
{
    public class LiquidacionController : Controller
    {
        private Autenticador authenticator = new Autenticador();
        LiquidacionHandler liquidacionHandler = new LiquidacionHandler();
        ColaboradorHandler colaboradorHandler = new ColaboradorHandler();

        public IActionResult Index()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador") return RedirectToAction("Index", "Home");
            List<Liquidacion> liquidaciones = liquidacionHandler.ObtenerLiquidaciones();
            ObtenerInformacionColaboradores(liquidaciones);
            return View(liquidaciones);
        }

        public IActionResult CrearLiquidacion()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador") return RedirectToAction("Index", "Home");
            List<Colaborador> colaboradores = colaboradorHandler.ObtenerColaboradores();
            return View(colaboradores);
        }

        public IActionResult GenerarLiquidacionColaborador(int idColaborador)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador") return RedirectToAction("Index", "Home");
            if (liquidacionHandler.GenerarLiquidacionColaborador(idColaborador))
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("CrearLiquidacion");
        }

        public void ObtenerInformacionColaboradores(List<Liquidacion> liquidaciones)
        {
            foreach (var liquidacion in liquidaciones)
            {
                liquidacion.Colaborador = colaboradorHandler.ObtenerColaborador((int)liquidacion.Colaborador.IdColaborador);
            }
        }
    }
}

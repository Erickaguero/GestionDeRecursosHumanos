using Microsoft.AspNetCore.Mvc;
using PrototipoFuncionalRecursosHumanos.Models;
using PrototipoFuncionalRecursosHumanos.Services;

namespace PrototipoFuncionalRecursosHumanos.Controllers
{
    public class PlanillaController : Controller
    {
        private Autenticador authenticator = new Autenticador();
        PlanillaHandler planillaHandler = new PlanillaHandler();
        ColaboradorHandler colaboradorHandler = new ColaboradorHandler();

        public IActionResult Index()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador") return RedirectToAction("Index", "Home");
            List<Planilla> planillas = planillaHandler.ObtenerPlanillas();
            ObtenerInformacionColaboradores(planillas);
            return View(planillas);
        }

        public IActionResult CrearPlanilla()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador") return RedirectToAction("Index", "Home");
            List<Colaborador> colaboradores = colaboradorHandler.ObtenerColaboradores();
            return View(colaboradores);
        }

        public IActionResult GenerarPlanillaColaboradores()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador") return RedirectToAction("Index", "Home");
            if (planillaHandler.GenerarPlanillaColaboradores())
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("CrearPlanilla");
        }

        public void ObtenerInformacionColaboradores(List<Planilla> planillas)
        {
            foreach (var planilla in planillas)
            {
                planilla.Colaborador = colaboradorHandler.ObtenerColaborador((int)planilla.Colaborador.IdColaborador);
            }
        }
    }
}

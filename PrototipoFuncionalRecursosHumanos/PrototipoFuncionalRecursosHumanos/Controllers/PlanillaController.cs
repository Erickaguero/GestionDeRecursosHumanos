using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador" || Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");
            List<Planilla> planillas = planillaHandler.ObtenerPlanillas();
            ObtenerInformacionColaboradores(planillas);
            return View(planillas);
        }

        public IActionResult CrearPlanilla()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador" || Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");
            List<Colaborador> colaboradores = colaboradorHandler.ObtenerColaboradores();
            return View(colaboradores);
        }

        public IActionResult GenerarPlanillaColaboradores()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador" || Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");
            if (!planillaHandler.PlanillaExistente(DateTime.Now.Date))
            {
                if (planillaHandler.GenerarPlanillaColaboradores())
                {
                    return RedirectToAction("Index");
                }
            }
            var alerta = Alertas.Error("Ya se genero la plantilla para el dia de hoy, no se pueden crear dos plantillas en un mismo dia.");
            TempData["Alerta"] = JsonConvert.SerializeObject(alerta);
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

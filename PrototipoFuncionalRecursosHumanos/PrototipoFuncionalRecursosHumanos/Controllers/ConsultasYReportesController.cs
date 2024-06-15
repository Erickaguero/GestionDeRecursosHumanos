using Microsoft.AspNetCore.Mvc;
using PrototipoFuncionalRecursosHumanos.Models;
using PrototipoFuncionalRecursosHumanos.Services;
using Rotativa.AspNetCore;

namespace PrototipoFuncionalRecursosHumanos.Controllers
{
    public class ConsultasYReportesController : Controller
    {
        private Autenticador authenticator = new Autenticador();
        private ColaboradorHandler colaboradorHandler = new ColaboradorHandler();
        private AsistenciasHandler asistenciasHandler = new AsistenciasHandler();
        private PlanillaHandler planillaHandler = new PlanillaHandler();
        private AguinaldoHandler aguinaldoHandler = new AguinaldoHandler();
        private LiquidacionHandler liquidacionHandler = new LiquidacionHandler();

        public IActionResult Index(string elementoADesplegar = "")
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador") return RedirectToAction("Index", "Home");

            var modelo = new ConsultasYReportes();
            CargarDatos(modelo, elementoADesplegar);
            modelo.DesplegarBotones = true;
            return View(modelo);
        }

        public IActionResult GenerarReporte(string elementoADesplegar = "")
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador") return RedirectToAction("Index", "Home");
            var modelo = new ConsultasYReportes();
            CargarDatos(modelo, elementoADesplegar);
            modelo.DesplegarBotones = false;
            // Redirige a la vista que deseas convertir en PDF
            return new ViewAsPdf("Index", modelo);
        }

        private void CargarDatos(ConsultasYReportes modelo, string elementoADesplegar)
        {
            switch (elementoADesplegar)
            {
                case "asistencias":
                    modelo.Asistencias = asistenciasHandler.ObtenerAsistencias();
                    foreach (var asistencia in modelo.Asistencias)
                    {
                        asistencia.Colaborador = colaboradorHandler.ObtenerColaborador((int)asistencia.Colaborador.IdColaborador);
                    }
                    break;
                case "colaboradores":
                    modelo.Colaboradores = colaboradorHandler.ObtenerColaboradores();
                    break;
                case "planilla":
                    modelo.Planillas = planillaHandler.ObtenerPlanillas();
                    foreach (var planilla in modelo.Planillas)
                    {
                        planilla.Colaborador = colaboradorHandler.ObtenerColaborador((int)planilla.Colaborador.IdColaborador);
                    }
                    break;
                case "aguinaldo":
                    modelo.Aguinaldos = aguinaldoHandler.ObtenerAguinaldos();
                    foreach (var aguinaldo in modelo.Aguinaldos)
                    {
                        aguinaldo.Colaborador = colaboradorHandler.ObtenerColaborador((int)aguinaldo.Colaborador.IdColaborador);
                    }
                    break;
                case "liquidacion":
                    modelo.Liquidaciones = liquidacionHandler.ObtenerLiquidaciones();
                    foreach (var liquidacion in modelo.Liquidaciones)
                    {
                        liquidacion.Colaborador = colaboradorHandler.ObtenerColaborador((int)liquidacion.Colaborador.IdColaborador);
                    }
                    break;
            }
            modelo.ElementoADesplegar = elementoADesplegar;
        }
    }
}

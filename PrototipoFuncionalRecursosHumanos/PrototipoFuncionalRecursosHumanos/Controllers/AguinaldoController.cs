using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PrototipoFuncionalRecursosHumanos.Models;
using PrototipoFuncionalRecursosHumanos.Services;

namespace PrototipoFuncionalRecursosHumanos.Controllers
{
    public class AguinaldoController : Controller
    {
        private Autenticador authenticator = new Autenticador();
        AguinaldoHandler aguinaldoHandler = new AguinaldoHandler();
        ColaboradorHandler colaboradorHandler = new ColaboradorHandler();

        public IActionResult Index()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador" || Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");
            List<Aguinaldo> aguinaldos = aguinaldoHandler.ObtenerAguinaldos();
            ObtenerInformacionColaboradores(aguinaldos);
            return View(aguinaldos);
        }

        public IActionResult CrearAguinaldo()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador" || Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");
            List<Colaborador> colaboradores = colaboradorHandler.ObtenerColaboradores();
            return View(colaboradores);
        }

        public IActionResult GenerarAguinaldoColaboradores()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador" || Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");
            if (!aguinaldoHandler.AguinaldoExistente(DateTime.Now.Date))
            {
                if (aguinaldoHandler.GenerarAguinaldoColaboradores())
                {
                    return RedirectToAction("Index");
                }
            }
            var alerta = Alertas.Error("Ya se genero el aguinaldo el dia de hoy, no se pueden generar dos aguinaldos en un mismo dia.");
            TempData["Alerta"] = JsonConvert.SerializeObject(alerta);
            return RedirectToAction("CrearAguinaldo");
        }

        public void ObtenerInformacionColaboradores(List<Aguinaldo> aguinaldos)
        {
            foreach (var aguinaldo in aguinaldos)
            {
                aguinaldo.Colaborador = colaboradorHandler.ObtenerColaborador((int)aguinaldo.Colaborador.IdColaborador);
            }
        }
    }
}

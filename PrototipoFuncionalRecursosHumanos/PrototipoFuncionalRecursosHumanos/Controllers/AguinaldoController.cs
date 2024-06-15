using Microsoft.AspNetCore.Mvc;
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
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador") return RedirectToAction("Index", "Home");
            List<Aguinaldo> aguinaldos = aguinaldoHandler.ObtenerAguinaldos();
            ObtenerInformacionColaboradores(aguinaldos);
            return View(aguinaldos);
        }

        public IActionResult CrearAguinaldo()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador") return RedirectToAction("Index", "Home");
            List<Colaborador> colaboradores = colaboradorHandler.ObtenerColaboradores();
            return View(colaboradores);
        }

        public IActionResult GenerarAguinaldoColaboradores()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador") return RedirectToAction("Index", "Home");
            if (aguinaldoHandler.GenerarAguinaldoColaboradores())
            {
                return RedirectToAction("Index");
            }
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

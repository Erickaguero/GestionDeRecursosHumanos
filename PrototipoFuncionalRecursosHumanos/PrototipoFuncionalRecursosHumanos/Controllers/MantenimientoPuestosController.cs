using Microsoft.AspNetCore.Mvc;
using PrototipoFuncionalRecursosHumanos.Models;
using PrototipoFuncionalRecursosHumanos.Services;


namespace PrototipoFuncionalRecursosHumanos.Controllers
{
    public class MantenimientoPuestosController : Controller
    {
        private Autenticador authenticator = new Autenticador();
        private PuestoHandler puestoHandler = new PuestoHandler();

        [HttpGet]

        public IActionResult Index()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador") return RedirectToAction("Index", "Home");
            List<Puesto> puestos = puestoHandler.ObtenerPuestos();
            return View(puestos);
        }
        [HttpGet]
        public IActionResult CrearPuesto()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador") return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult CrearPuesto(Puesto puesto)
        {
            if (ModelState.IsValid)
            {
                puestoHandler.AgregarPuesto(puesto);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult EditarPuesto(int idPuesto)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador") return RedirectToAction("Index", "Home");
            Puesto puesto = puestoHandler.ObtenerPuesto(idPuesto);
            if (puesto == null)
            {
                return NotFound();
            }

            TempData["IdPuesto"] = puesto.IdPuesto;
            return View(puesto);
        }

        [HttpPost]
        public IActionResult EditarPuesto(Puesto puesto)
        {
            if (ModelState.IsValid)
            {
                if (TempData["IdPuesto"] != null)
                {
                    puesto.IdPuesto = (int)TempData["IdPuesto"];
                    puestoHandler.EditarPuesto(puesto);
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult EliminarPuesto(int idPuesto)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador") return RedirectToAction("Index", "Home");
            puestoHandler.EliminarPuesto(idPuesto);
            return RedirectToAction("Index");
        }

    }
}

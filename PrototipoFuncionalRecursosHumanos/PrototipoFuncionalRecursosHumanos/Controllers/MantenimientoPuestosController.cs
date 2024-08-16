using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
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
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador" || Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");
            List<Puesto> puestos = puestoHandler.ObtenerPuestos();
            return View(puestos);
        }
        [HttpGet]
        public IActionResult CrearPuesto()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador" || Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult CrearPuesto(Puesto puesto)
        {
            ValidarPuesto(puesto, ModelState);
            if (ModelState.IsValid)
            {
                if (puestoHandler.AgregarPuesto(puesto)) {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult EditarPuesto(int idPuesto)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador" || Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");
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
            ValidarPuesto(puesto, ModelState);
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
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador" || Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");
            if (!puestoHandler.EliminarPuesto(idPuesto))
            {
                var alerta = Alertas.Error("No se puede eliminar el puesto porque hay colaboradores que tienen dicho puesto.");
                TempData["Alerta"] = JsonConvert.SerializeObject(alerta);
            }
            return RedirectToAction("Index");
        }

        public void ValidarPuesto(Puesto puesto, ModelStateDictionary ModelState)
        {
            if (string.IsNullOrEmpty(puesto.NombrePuesto))
            {
                ModelState.AddModelError("NombrePuesto", "El nombre del puesto es requerido.");
            }
            if (puesto.CostoPorHora == null)
            {
                ModelState.AddModelError("CostoPorHora", "El costo por hora es requerido.");

            } else 
            {
                if (puesto.CostoPorHora <= 0)
                {
                    ModelState.AddModelError("CostoPorHora", "El costo por hora debe ser mayor a 0.");
                }
            }
        }

    }
}

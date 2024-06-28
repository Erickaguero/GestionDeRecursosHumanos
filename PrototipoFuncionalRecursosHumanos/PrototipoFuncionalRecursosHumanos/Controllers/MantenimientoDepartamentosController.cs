using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PrototipoFuncionalRecursosHumanos.Models;
using PrototipoFuncionalRecursosHumanos.Services;

namespace PrototipoFuncionalRecursosHumanos.Controllers
{
    public class MantenimientoDepartamentosController : Controller
    {
        private Autenticador authenticator = new Autenticador();
        private DepartamentoHandler departamentoHandler = new DepartamentoHandler();

        [HttpGet]

        public IActionResult Index()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador") return RedirectToAction("Index", "Home");
            List<Departamento> departamentos = departamentoHandler.ObtenerDepartamentos();
            return View(departamentos);
        }

        [HttpGet]
        public IActionResult CrearDepartamento()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador") return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult CrearDepartamento(Departamento departamento)
        {
            ValidarDepartamento(departamento, ModelState);
            if (ModelState.IsValid)
            {
                departamentoHandler.AgregarDepartamento(departamento.Nombre);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult EditarDepartamento(int idDepartamento)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador") return RedirectToAction("Index", "Home");
            Departamento departamento = departamentoHandler.ObtenerDepartamento(idDepartamento);
            if (departamento == null)
            {
                return NotFound();
            }

            TempData["IdDepartamento"] = departamento.IdDepartamento;
            return View(departamento);
        }

        [HttpPost]
        public IActionResult EditarDepartamento(Departamento departamento)
        {
            ValidarDepartamento(departamento, ModelState);
            if (ModelState.IsValid)
            {
                if (TempData["IdDepartamento"] != null)
                {
                    departamento.IdDepartamento = (int)TempData["IdDepartamento"];
                    if (departamentoHandler.EditarDepartamento(departamento))
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult EliminarDepartamento(int idDepartamento)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador") return RedirectToAction("Index", "Home");
            departamentoHandler.EliminarDepartamento(idDepartamento);
            return RedirectToAction("Index");
        }

        public void ValidarDepartamento(Departamento departamento, ModelStateDictionary ModelState)
        {
            if (string.IsNullOrEmpty(departamento.Nombre))
            {
                ModelState.AddModelError("Nombre", "El nombre del departamento es requerido.");
            }
        }
    }
}

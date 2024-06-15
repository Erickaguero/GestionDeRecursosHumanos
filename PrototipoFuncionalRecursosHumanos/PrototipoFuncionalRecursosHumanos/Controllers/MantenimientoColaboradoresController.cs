using Microsoft.AspNetCore.Mvc;
using PrototipoFuncionalRecursosHumanos.Models;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using PrototipoFuncionalRecursosHumanos.Services;

namespace PrototipoFuncionalRecursosHumanos.Controllers
{
    public class MantenimientoColaboradoresController : Controller
    {
        private Autenticador authenticator = new Autenticador();
        private ColaboradorHandler colaboradorHandler = new ColaboradorHandler();
        private RolDeUsuarioHandler rolDeUsuarioHandler = new RolDeUsuarioHandler();
        private DepartamentoHandler departamentoHandler = new DepartamentoHandler();
        private PuestoHandler puestoHandler = new PuestoHandler();
        private EnviadorCorreos emailSender = new EnviadorCorreos();
        private GeneradorContrasena passwordGenerator = new GeneradorContrasena();

        [HttpGet]
        public IActionResult Index()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador") return RedirectToAction("Index", "Home");
            List<Colaborador> colaboradores = colaboradorHandler.ObtenerColaboradores();
            return View(colaboradores);
        }

        [HttpGet]
        public IActionResult CrearColaborador()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador") return RedirectToAction("Index", "Home");
            ViewBag.RolesDeUsuario = rolDeUsuarioHandler.ObtenerRolesDeUsuario();
            ViewBag.Departamentos = departamentoHandler.ObtenerDepartamentos();
            ViewBag.Puestos = puestoHandler.ObtenerPuestos();
            return View();
        }

        [HttpPost]
        public IActionResult CrearColaborador(Colaborador colaborador)
        {
            if (ModelState.IsValid)
            {
                colaborador.Usuario.Contrasena = passwordGenerator.GenerarContrasenaSegura();
                if (colaboradorHandler.AgregarColaborador(colaborador)) { 
                    emailSender.EnviarCorreoColaborador(colaborador);
                    return RedirectToAction("Index"); // Redirige al usuario a la página de inicio después de agregar el colaborador
                }
            }
            // Si los modelos no son válidos, devuelve la vista con los modelos para mostrar los errores de validación
            ViewBag.RolesDeUsuario = rolDeUsuarioHandler.ObtenerRolesDeUsuario();
            ViewBag.Departamentos = departamentoHandler.ObtenerDepartamentos();
            ViewBag.Puestos = puestoHandler.ObtenerPuestos();
            return View();
        }

        [HttpGet]
        public IActionResult EditarColaborador(int idColaborador)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador") return RedirectToAction("Index", "Home");
            Colaborador colaborador = colaboradorHandler.ObtenerColaborador(idColaborador);
            if (colaborador == null)
            {
                return NotFound();
            }

            TempData["IdColaborador"] = colaborador.IdColaborador;
            TempData["ContrasenaUsuario"] = colaborador.Usuario.Contrasena;
            ViewBag.RolesDeUsuario = rolDeUsuarioHandler.ObtenerRolesDeUsuario();
            ViewBag.Departamentos = departamentoHandler.ObtenerDepartamentos();
            ViewBag.Puestos = puestoHandler.ObtenerPuestos();

            return View(colaborador);
        }

        [HttpPost]
        public IActionResult EditarColaborador(Colaborador colaborador)
        {
            if (ModelState.IsValid)
            {
                if (TempData["IdColaborador"] != null && TempData["ContrasenaUsuario"] != null)
                {
                    colaborador.IdColaborador = (int)TempData["IdColaborador"];
                    colaborador.Usuario.Contrasena = (string)TempData["ContrasenaUsuario"];
                    if (colaboradorHandler.EditarColaborador(colaborador))
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            // Si los modelos no son válidos, devuelve la vista con los modelos para mostrar los errores de validación
            ViewBag.RolesDeUsuario = rolDeUsuarioHandler.ObtenerRolesDeUsuario();
            ViewBag.Departamentos = departamentoHandler.ObtenerDepartamentos();
            ViewBag.Puestos = puestoHandler.ObtenerPuestos();
            return View();
        }

        [HttpGet]
        public IActionResult EliminarColaborador(int idColaborador)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador") return RedirectToAction("Index", "Home");
            colaboradorHandler.EliminarColaborador(idColaborador);
            return RedirectToAction("Index");
        }
    }
}

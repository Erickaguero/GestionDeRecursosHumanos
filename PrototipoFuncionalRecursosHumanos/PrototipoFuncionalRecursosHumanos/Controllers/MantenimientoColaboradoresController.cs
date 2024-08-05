using Microsoft.AspNetCore.Mvc;
using PrototipoFuncionalRecursosHumanos.Models;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using PrototipoFuncionalRecursosHumanos.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

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
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador" || Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");
            List<Colaborador> colaboradoresActivos = colaboradorHandler.ObtenerColaboradores();
            List<Colaborador> colaboradoresInactivos = colaboradorHandler.ObtenerColaboradoresInactivos();
            List<Colaborador> colaboradores = new List<Colaborador>(colaboradoresActivos.Concat(colaboradoresInactivos));
            return View(colaboradores);
        }

        [HttpGet]
        public IActionResult CrearColaborador()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador" || Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");
            ViewBag.RolesDeUsuario = rolDeUsuarioHandler.ObtenerRolesDeUsuario();
            ViewBag.Departamentos = departamentoHandler.ObtenerDepartamentos();
            ViewBag.Puestos = puestoHandler.ObtenerPuestos();
            return View();
        }

        [HttpPost]
        public IActionResult CrearColaborador(Colaborador colaborador)
        {
            ValidarColaborador(colaborador, ModelState);
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
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador" || Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");
            Colaborador colaborador = colaboradorHandler.ObtenerColaborador(idColaborador);
            TempData["IdColaborador"] = colaborador.IdColaborador;
            TempData["CorreoColaborador"] = colaborador.Usuario.Correo;
            TempData["ContrasenaColaborador"] = colaborador.Usuario.Contrasena;
            ViewBag.RolesDeUsuario = rolDeUsuarioHandler.ObtenerRolesDeUsuario();
            ViewBag.Departamentos = departamentoHandler.ObtenerDepartamentos();
            ViewBag.Puestos = puestoHandler.ObtenerPuestos();

            return View(colaborador);
        }

        [HttpGet]
        public IActionResult ReiniciarContrasena(int idColaborador)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador" || Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");
            Colaborador colaborador = colaboradorHandler.ObtenerColaborador(idColaborador);
            colaborador.Usuario.Contrasena = passwordGenerator.GenerarContrasenaSegura();
            emailSender.EnviarCorreoColaborador(colaborador);
            var alerta = Alertas.Exito("Se reinicio la contraseña con éxito.");
            TempData["Alerta"] = JsonConvert.SerializeObject(alerta);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditarColaborador(Colaborador colaborador)
        {
            if (TempData["IdColaborador"] != null && TempData["CorreoColaborador"] != null)
            {
                colaborador.IdColaborador = (int)TempData["IdColaborador"];
                if (colaborador.Usuario.Correo != (string)TempData["CorreoColaborador"])
                {
                    colaborador.Usuario.Contrasena = passwordGenerator.GenerarContrasenaSegura();
                }
                else
                {
                    colaborador.Usuario.Contrasena = (string)TempData["ContrasenaColaborador"];
                }
            }
            ValidarColaborador(colaborador, ModelState);
            if (ModelState.IsValid)
            {
                if (colaboradorHandler.EditarColaborador(colaborador))
                {
                    emailSender.EnviarCorreoColaborador(colaborador);
                    return RedirectToAction("Index");
                }
            }
            // Si los modelos no son válidos, devuelve la vista con los modelos para mostrar los errores de validación
            TempData["IdColaborador"] = colaborador.IdColaborador;
            TempData["CorreoColaborador"] = colaborador.Usuario.Correo;
            TempData["ContrasenaColaborador"] = colaborador.Usuario.Contrasena;
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
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador" || Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");
            colaboradorHandler.EliminarColaborador(idColaborador);
            return RedirectToAction("Index");
        }

        public void ValidarColaborador(Colaborador colaborador, ModelStateDictionary ModelState)
        {
            if (string.IsNullOrEmpty(colaborador.Persona.Identificacion))
            {
                ModelState.AddModelError("Persona.Identificacion", "La identificación es requerida.");
            }
            else
            {
                if (colaboradorHandler.ExisteColaboradorPorIdentificacion(colaborador.Persona.Identificacion) && (colaborador.IdColaborador == null || (colaboradorHandler.ObtenerColaborador((int)colaborador.IdColaborador).Persona.Identificacion != colaborador.Persona.Identificacion)))
                {
                    ModelState.AddModelError("Persona.Identificacion", "Ya existe un colaborador con la misma identificación.");
                }
            }
            if (string.IsNullOrEmpty(colaborador.Persona.TipoIdentificacion))
            {
                ModelState.AddModelError("Persona.TipoIdentificacion", "El tipo de identificación es requerida.");
            }
            if (!string.IsNullOrEmpty(colaborador.Persona.Identificacion) && !string.IsNullOrEmpty(colaborador.Persona.TipoIdentificacion))
            {
                if (colaborador.Persona.TipoIdentificacion == "nacional" && colaborador.Persona.Identificacion.Length != 9)
                {
                    ModelState.AddModelError("Persona.Identificacion", "La cédula ingresada no es valida.");
                }
                else if (colaborador.Persona.TipoIdentificacion == "extranjero" && colaborador.Persona.Identificacion.Length < 10)
                {
                    ModelState.AddModelError("Persona.Identificacion", "El dimex ingresado no es valido.");
                }
            }
            if (string.IsNullOrEmpty(colaborador.Persona.Nombre))
            {
                ModelState.AddModelError("Persona.Nombre", "El nombre es requerido.");
            }
            if (string.IsNullOrEmpty(colaborador.Persona.Apellido1))
            {
                ModelState.AddModelError("Persona.Apellido1", "El primer apellido es requerido.");
            }
            if (string.IsNullOrEmpty(colaborador.Persona.Apellido2))
            {
                ModelState.AddModelError("Persona.Apellido2", "El segundo apellido es requerido.");
            }
            if (colaborador.Persona.FechaDeNacimiento == null)
            {
                ModelState.AddModelError("Persona.FechaDeNacimiento", "La fecha de nacimiento es requerida.");
            }
            else
            {
                if (colaborador.Persona.FechaDeNacimiento.Value.Date > DateTime.Now.Date)
                {
                    ModelState.AddModelError("Persona.FechaDeNacimiento", "La fecha de nacimiento no puede ser en el futuro.");
                }
                else if (colaborador.Persona.FechaDeNacimiento.Value.Date > DateTime.Now.Date.AddYears(-15))
                {
                    ModelState.AddModelError("Persona.FechaDeNacimiento", "La persona debe tener al menos 15 años.");
                }
            }
            if (string.IsNullOrEmpty(colaborador.Usuario.Correo))
            {
                ModelState.AddModelError("Usuario.Correo", "El correo es requerido.");
            }
            else
            {
                if (colaboradorHandler.ExisteColaboradorPorCorreo(colaborador.Usuario.Correo) && (colaborador.IdColaborador == null || (colaboradorHandler.ObtenerColaborador((int)colaborador.IdColaborador).Usuario.Correo != colaborador.Usuario.Correo)))
                {
                    ModelState.AddModelError("Usuario.Correo", "Ya existe un colaborador con el mismo correo.");
                }
            }
            if (colaborador.Usuario.RolDeUsuario == null || colaborador.Usuario.RolDeUsuario.IdRolDeUsuario == null)
            {
                ModelState.AddModelError("Usuario.RolDeUsuario.IdRolDeUsuario", "El rol de usuario es requerido.");
            }
            if (colaborador.Departamento == null || colaborador.Departamento.IdDepartamento == null)
            {
                ModelState.AddModelError("Departamento.IdDepartamento", "El departamento es requerido.");
            }
            if (colaborador.Puesto == null || colaborador.Puesto.IdPuesto == null)
            {
                ModelState.AddModelError("Puesto.IdPuesto", "El puesto es requerido.");
            }
        }
    }
}

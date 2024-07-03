using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PrototipoFuncionalRecursosHumanos.Models;
using PrototipoFuncionalRecursosHumanos.Services;

namespace PrototipoFuncionalRecursosHumanos.Controllers
{
    public class GestionPermisosController : Controller
    {
        private Autenticador authenticator = new Autenticador();
        private ColaboradorHandler colaboradorHandler = new ColaboradorHandler();
        private PermisosHandler permisosHandler = new PermisosHandler();
        private TipoPermisosHandler tipoPermisosHandler = new TipoPermisosHandler();
        private ValidacionesHandler validacionesHandler = new ValidacionesHandler();

        [HttpGet]
        public IActionResult SolicitarPermisos()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            List<Permisos> permisos = permisosHandler.ObtenerPermisos(colaborador.IdColaborador);
            foreach (var permiso in permisos)
            {
                permiso.Colaborador = colaborador;
            }
            ObtenerInformacionTipoPermisos(permisos);
            ViewBag.PermisosAprobados = permisos.Where(permiso => permiso.Estado == "Aprobado").ToList();
            ViewBag.PermisosRechazados = permisos.Where(permiso => permiso.Estado == "Rechazado").ToList();
            ViewBag.PermisosPendientes = permisos.Where(permiso => permiso.Estado == "Pendiente" || permiso.Estado == "Aprobado por jefatura").ToList();
            return View();
        }

        [HttpGet]
        public IActionResult CrearSolicitudPermiso()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult CrearSolicitudPermiso(Permisos permiso)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            permiso.Colaborador = colaborador;
            ValidarPermisos(permiso, ModelState);
            if (ModelState.IsValid)
            {
                permiso.Estado = "Pendiente";

                if (permisosHandler.AgregarPermiso(permiso))
                {
                    return RedirectToAction("SolicitarPermisos");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult EliminarPermiso(int idPermiso)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            permisosHandler.EliminarPermiso(idPermiso);
            return RedirectToAction("SolicitarPermisos");
        }

        [HttpGet]
        public IActionResult AprobarPermisos()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            var rolDeUsuario = colaborador.Usuario.RolDeUsuario.Descripcion;
            if (rolDeUsuario != "administrador" && rolDeUsuario != "jefatura") return RedirectToAction("Index", "Home");
            List<Permisos> permisos = new List<Permisos>();
            if (rolDeUsuario == "administrador") permisos = permisosHandler.ObtenerPermisosParaAprobarPorAdministrador(colaborador.IdColaborador);
            if (rolDeUsuario == "jefatura") permisos = permisosHandler.ObtenerPermisosParaAprobarPorJefatura(colaborador.IdColaborador);
            ObtenerInformacionColaboradores(permisos);
            ObtenerInformacionTipoPermisos(permisos);
            ViewBag.PermisosAprobadosPorJefatura = permisos.Where(permiso => permiso.Estado == "Aprobado por jefatura").ToList();
            ViewBag.PermisosPendientes = permisos.Where(permiso => permiso.Estado == "Pendiente").ToList();
            ViewBag.TipoPermisos = tipoPermisosHandler.ObtenerTipoPermisos();
            return View();
        }

        [HttpPost]
        public IActionResult EditarTipoPermiso(int idPermiso, int idTipoPermiso)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            permisosHandler.EditarTipoPermiso(idPermiso, idTipoPermiso);
            return RedirectToAction("AprobarPermisos");
        }

        [HttpGet]
        public IActionResult AprobarPermiso(int idPermiso, string tipoPermiso)
        {
            if (tipoPermiso == "no definido") return RedirectToAction("AprobarPermisos");
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            var rolDeUsuario = colaborador.Usuario.RolDeUsuario.Descripcion;
            if (rolDeUsuario != "administrador" && rolDeUsuario != "jefatura") return RedirectToAction("Index", "Home");

            if (rolDeUsuario == "administrador") permisosHandler.AprobarPermisoAdministrador(idPermiso);
            if (rolDeUsuario == "jefatura") permisosHandler.AprobarPermisoJefatura(idPermiso);

            return RedirectToAction("AprobarPermisos");
        }

        [HttpGet]
        public IActionResult RechazarPermiso(int idPermiso)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            var rolDeUsuario = colaborador.Usuario.RolDeUsuario.Descripcion;
            if (rolDeUsuario != "administrador" && rolDeUsuario != "jefatura") return RedirectToAction("Index", "Home");
            permisosHandler.RechazarPermiso(idPermiso);

            return RedirectToAction("AprobarPermisos");
        }

        public void ValidarPermisos(Permisos permiso, ModelStateDictionary ModelState)
        {
            if (permiso.FechaPermiso == null)
            {
                ModelState.AddModelError("FechaPermiso", "La fecha de permiso es obligatoria.");
            }
            if (permiso.Horas == null)
            {
                ModelState.AddModelError("Horas", "La cantidad de horas es obligatoria.");
            }
            if (permiso.Horas < 1 || permiso.Horas > 2)
            {
                ModelState.AddModelError("Horas", "La cantidad de horas ingresada es invalida.");
            }
            if (string.IsNullOrEmpty(permiso.Justificacion))
            {
                ModelState.AddModelError("Justificacion", "La justificación es obligatoria.");
            }
            if (permiso.FechaPermiso != null && permiso.FechaPermiso < DateTime.Now.Date)
            {
                ModelState.AddModelError("FechaPermiso", "La fecha del permiso no puede ser una fecha anterior a la de hoy.");
            }
            if (permiso.FechaPermiso != null)
            {
                if (permisosHandler.PermisoExistente(permiso))
                {
                    ModelState.AddModelError("FechaPermiso", "Ya tienes una solicitud de permiso en esa fecha");
                }
                if (validacionesHandler.ValidarSiEsFeriado((DateTime)permiso.FechaPermiso))
                {
                    ModelState.AddModelError("FechaPermiso", "No puedes solicitar permisos en un día feriado.");
                }
                if (!validacionesHandler.ValidarFechaUnica((DateTime)permiso.FechaPermiso, (int)permiso.Colaborador.IdColaborador))
                {
                    ModelState.AddModelError("FechaPermiso", "No puede solicitar un permiso si ya solicito una hora extra, incapacidad, vacacion o permiso en esa misma fecha.");
                }
            }
        }

        public void ObtenerInformacionColaboradores(List<Permisos> permisos)
        {
            foreach (var permiso in permisos)
            {
                permiso.Colaborador = colaboradorHandler.ObtenerColaborador((int)permiso.Colaborador.IdColaborador);
            }
        }

        public void ObtenerInformacionTipoPermisos(List<Permisos> permisos)
        {
            foreach (var permiso in permisos)
            {
                permiso.TipoPermiso = tipoPermisosHandler.ObtenerTipoPermiso((int)permiso.TipoPermiso.IdTipoPermiso);
            }
        }
    }
}

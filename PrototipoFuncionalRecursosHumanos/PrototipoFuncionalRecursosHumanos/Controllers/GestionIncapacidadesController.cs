using Microsoft.AspNetCore.Mvc;
using PrototipoFuncionalRecursosHumanos.Models;
using PrototipoFuncionalRecursosHumanos.Services;

namespace PrototipoFuncionalRecursosHumanos.Controllers
{
    public class GestionIncapacidadesController : Controller
    {
        private Autenticador authenticator = new Autenticador();
        private ColaboradorHandler colaboradorHandler = new ColaboradorHandler();
        private IncapacidadesHandler incapacidadesHandler = new IncapacidadesHandler();
        private TipoIncapacidadesHandler tipoIncapacidadesHandler = new TipoIncapacidadesHandler();

        [HttpGet]
        public IActionResult SolicitarIncapacidades()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            List<Incapacidades> incapacidades = incapacidadesHandler.ObtenerIncapacidades(colaborador.IdColaborador);
            foreach (var incapacidad in incapacidades)
            {
                incapacidad.Colaborador = colaborador;
            }
            ObtenerInformacionTipoIncapacidades(incapacidades);
            ViewBag.IncapacidadesAprobadas = incapacidades.Where(incapacidad => incapacidad.Estado == "Aprobado").ToList();
            ViewBag.IncapacidadesRechazadas = incapacidades.Where(incapacidad => incapacidad.Estado == "Rechazado").ToList();
            ViewBag.IncapacidadesPendientes = incapacidades.Where(incapacidad => incapacidad.Estado == "Pendiente" || incapacidad.Estado == "Aprobado por jefatura").ToList();
            return View();
        }

        [HttpGet]
        public IActionResult CrearSolicitudIncapacidad()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult CrearSolicitudIncapacidad(Incapacidades incapacidad)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            incapacidad.Colaborador = colaborador;
            if (ModelState.IsValid)
            {
                if (incapacidadesHandler.IncapacidadExistente(incapacidad))
                {
                    ModelState.AddModelError("FechaFin", "Ya tienes una solicitud de incapacidad en esa fecha");
                    return View();
                }
                incapacidad.Estado = "Pendiente";

                if (incapacidadesHandler.AgregarIncapacidad(incapacidad))
                {
                    return RedirectToAction("SolicitarIncapacidades");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult EliminarIncapacidad(int idIncapacidad)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            incapacidadesHandler.EliminarIncapacidad(idIncapacidad);
            return RedirectToAction("SolicitarIncapacidades");
        }

        [HttpGet]
        public IActionResult AprobarIncapacidades()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            var rolDeUsuario = colaborador.Usuario.RolDeUsuario.Descripcion;
            if (rolDeUsuario != "administrador" && rolDeUsuario != "jefatura") return RedirectToAction("Index", "Home");
            List<Incapacidades> incapacidades = new List<Incapacidades>();
            if (rolDeUsuario == "administrador") incapacidades = incapacidadesHandler.ObtenerIncapacidadesParaAprobarPorAdministrador(colaborador.IdColaborador);
            if (rolDeUsuario == "jefatura") incapacidades = incapacidadesHandler.ObtenerIncapacidadesParaAprobarPorJefatura(colaborador.IdColaborador);
            ObtenerInformacionColaboradores(incapacidades);
            ObtenerInformacionTipoIncapacidades(incapacidades);
            ViewBag.IncapacidadesAprobadasPorJefatura = incapacidades.Where(incapacidad => incapacidad.Estado == "Aprobado por jefatura").ToList();
            ViewBag.IncapacidadesPendientes = incapacidades.Where(incapacidad => incapacidad.Estado == "Pendiente").ToList();
            ViewBag.TipoIncapacidades = tipoIncapacidadesHandler.ObtenerTipoIncapacidades();
            return View();
        }

        [HttpPost]
        public IActionResult EditarTipoIncapacidad(int idIncapacidad, int idTipoIncapacidad)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            incapacidadesHandler.EditarTipoIncapacidad(idIncapacidad, idTipoIncapacidad);
            return RedirectToAction("AprobarIncapacidades");
        }

        [HttpGet]
        public IActionResult AprobarIncapacidad(int idIncapacidad, string tipoIncapacidad)
        {
            if (tipoIncapacidad == "no definido") return RedirectToAction("AprobarIncapacidades");
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            var rolDeUsuario = colaborador.Usuario.RolDeUsuario.Descripcion;
            if (rolDeUsuario != "administrador" && rolDeUsuario != "jefatura") return RedirectToAction("Index", "Home");

            if (rolDeUsuario == "administrador") incapacidadesHandler.AprobarIncapacidadAdministrador(idIncapacidad);
            if (rolDeUsuario == "jefatura") incapacidadesHandler.AprobarIncapacidadJefatura(idIncapacidad);

            return RedirectToAction("AprobarIncapacidades");
        }

        [HttpGet]
        public IActionResult RechazarIncapacidad(int idIncapacidad)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            var rolDeUsuario = colaborador.Usuario.RolDeUsuario.Descripcion;
            if (rolDeUsuario != "administrador" && rolDeUsuario != "jefatura") return RedirectToAction("Index", "Home");
            incapacidadesHandler.RechazarIncapacidad(idIncapacidad);

            return RedirectToAction("AprobarIncapacidades");
        }

        public void ObtenerInformacionColaboradores(List<Incapacidades> incapacidades)
        {
            foreach (var incapacidad in incapacidades)
            {
                incapacidad.Colaborador = colaboradorHandler.ObtenerColaborador((int)incapacidad.Colaborador.IdColaborador);
            }
        }

        public void ObtenerInformacionTipoIncapacidades(List<Incapacidades> incapacidades)
        {
            foreach (var incapacidad in incapacidades)
            {
                incapacidad.TipoIncapacidad = tipoIncapacidadesHandler.ObtenerTipoIncapacidad((int)incapacidad.TipoIncapacidad.IdTipoIncapacidad);
            }
        }
    }
}

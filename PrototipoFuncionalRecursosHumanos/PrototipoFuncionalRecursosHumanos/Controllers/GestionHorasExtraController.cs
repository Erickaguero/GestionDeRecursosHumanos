using Microsoft.AspNetCore.Mvc;
using PrototipoFuncionalRecursosHumanos.Models;
using PrototipoFuncionalRecursosHumanos.Services;

namespace PrototipoFuncionalRecursosHumanos.Controllers
{
    public class GestionHorasExtraController : Controller
    {
        private Autenticador authenticator = new Autenticador();
        private ColaboradorHandler colaboradorHandler = new ColaboradorHandler();
        private HorasExtraHandler horasExtraHandler = new HorasExtraHandler();

        [HttpGet]
        public IActionResult SolicitarHorasExtra()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            List<HorasExtra> horasExtra = horasExtraHandler.ObtenerHorasExtra(colaborador.IdColaborador);
            foreach (var horaExtra in horasExtra)
            {
                horaExtra.Colaborador = colaborador;
            }
            ViewBag.HorasExtraAprobadas = horasExtra.Where(horaExtra => horaExtra.Estado == "Aprobado").ToList();
            ViewBag.HorasExtraRechazadas = horasExtra.Where(horaExtra => horaExtra.Estado == "Rechazado").ToList();
            ViewBag.HorasExtraPendientes = horasExtra.Where(horaExtra => horaExtra.Estado == "Pendiente" || horaExtra.Estado == "Aprobado por jefatura").ToList();
            return View();
        }

        [HttpGet]
        public IActionResult CrearSolicitudHorasExtra()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult CrearSolicitudHorasExtra(HorasExtra horasExtra)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            horasExtra.Colaborador = colaborador;
            if (ModelState.IsValid)
            {
                if (horasExtraHandler.HorasExtraExistentes(horasExtra))
                {
                    ModelState.AddModelError("FechaHoraExtra", "Ya tienes una solicitud de horas extra en esa fecha");
                    return View();
                }
                horasExtra.Estado = "Pendiente";

                if (horasExtraHandler.AgregarHorasExtra(horasExtra))
                {
                    return RedirectToAction("SolicitarHorasExtra");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult EliminarHorasExtra(int idHorasExtra)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            horasExtraHandler.EliminarHorasExtra(idHorasExtra);
            return RedirectToAction("SolicitarHorasExtra");
        }

        [HttpGet]
        public IActionResult AprobarHorasExtra()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            var rolDeUsuario = colaborador.Usuario.RolDeUsuario.Descripcion;
            if (rolDeUsuario != "administrador" && rolDeUsuario != "jefatura") return RedirectToAction("Index", "Home");
            List<HorasExtra> horasExtras = new List<HorasExtra>();
            if (rolDeUsuario == "administrador") horasExtras = horasExtraHandler.ObtenerHorasExtraParaAprobarPorAdministrador(colaborador.IdColaborador);
            if (rolDeUsuario == "jefatura") horasExtras = horasExtraHandler.ObtenerHorasExtraParaAprobarPorJefatura(colaborador.IdColaborador);
            ObtenerInformacionColaboradores(horasExtras);
            ViewBag.HorasExtraAprobadasPorJefatura = horasExtras.Where(horaExtra => horaExtra.Estado == "Aprobado por jefatura").ToList();
            ViewBag.HorasExtraPendientes = horasExtras.Where(horaExtra => horaExtra.Estado == "Pendiente").ToList();
            return View();
        }

        [HttpGet]
        public IActionResult AprobarHoraExtra(int idHoraExtra)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            var rolDeUsuario = colaborador.Usuario.RolDeUsuario.Descripcion;
            if (rolDeUsuario != "administrador" && rolDeUsuario != "jefatura") return RedirectToAction("Index", "Home");

            if (rolDeUsuario == "administrador") horasExtraHandler.AprobarHorasExtraAdministrador(idHoraExtra);
            if (rolDeUsuario == "jefatura") horasExtraHandler.AprobarHorasExtraJefatura(idHoraExtra);

            return RedirectToAction("AprobarHorasExtra");
        }

        [HttpGet]
        public IActionResult RechazarHoraExtra(int idHoraExtra)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            var rolDeUsuario = colaborador.Usuario.RolDeUsuario.Descripcion;
            if (rolDeUsuario != "administrador" && rolDeUsuario != "jefatura") return RedirectToAction("Index", "Home");
            horasExtraHandler.RechazarHorasExtra(idHoraExtra);

            return RedirectToAction("AprobarHorasExtra");
        }

        public void ObtenerInformacionColaboradores(List<HorasExtra> horasExtras)
        {
            foreach (var horaExtra in horasExtras)
            {
                horaExtra.Colaborador = colaboradorHandler.ObtenerColaborador((int)horaExtra.Colaborador.IdColaborador);
            }
        }
    }
}

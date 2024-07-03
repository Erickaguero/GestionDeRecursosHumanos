using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PrototipoFuncionalRecursosHumanos.Models;
using PrototipoFuncionalRecursosHumanos.Services;

namespace PrototipoFuncionalRecursosHumanos.Controllers
{
    public class GestionHorasExtraController : Controller
    {
        private Autenticador authenticator = new Autenticador();
        private ColaboradorHandler colaboradorHandler = new ColaboradorHandler();
        private HorasExtraHandler horasExtraHandler = new HorasExtraHandler();
        private ValidacionesHandler validacionesHandler = new ValidacionesHandler();

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
            ValidarHorasExtra(horasExtra, ModelState);
            if (ModelState.IsValid)
            {
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

        public void ValidarHorasExtra(HorasExtra horasExtra, ModelStateDictionary ModelState)
        {
            if (horasExtra.FechaHorasExtra == null)
            {
                ModelState.AddModelError("FechaHorasExtra", "La fecha de la hora extra es obligatoria.");
            }
            if (horasExtra.FechaHorasExtra != null)
            {
                DateTime hoy = DateTime.Now.Date;
                DateTime fechaInicio;
                DateTime fechaFin = hoy;
                bool esUltimoDiaDelMes = hoy.Day == DateTime.DaysInMonth(hoy.Year, hoy.Month);

                if (hoy.Day <= 14 || esUltimoDiaDelMes)
                {
                    int mesAnterior = hoy.Month - 1;
                    int año = hoy.Year;
                    if (mesAnterior == 0)
                    {
                        mesAnterior = 12;
                        año--;
                    }
                    fechaInicio = new DateTime(año, mesAnterior, DateTime.DaysInMonth(año, mesAnterior));
                }
                else
                {
                    fechaInicio = new DateTime(hoy.Year, hoy.Month, 15);
                }

                if (horasExtra.FechaHorasExtra > fechaFin)
                {
                    ModelState.AddModelError("FechaHorasExtra", "La fecha de la hora extra no puede ser una fecha mayor a la de hoy.");
                }
                else if (horasExtra.FechaHorasExtra < fechaInicio)
                {
                    string fechaInicioFormateada = fechaInicio.ToString("dd/MM/yyyy");
                    string fechaFinFormateada = fechaFin.ToString("dd/MM/yyyy");
                    ModelState.AddModelError("FechaHorasExtra", $"La fecha de la hora extra no está dentro del periodo actual válido. El periodo válido es desde el {fechaInicioFormateada} hasta el {fechaFinFormateada}.");
                }
            }
            if (horasExtra.Horas == null)
            {
                ModelState.AddModelError("Horas", "La cantidad de horas es obligatoria.");
            }
            if (horasExtra.Horas < 1)
            {
                ModelState.AddModelError("Horas", "La cantidad de horas ingresada es invalida.");
            }
            if (string.IsNullOrEmpty(horasExtra.Justificacion))
            {
                ModelState.AddModelError("Justificacion", "La justificación es obligatoria.");
            }
            if (horasExtra.FechaHorasExtra != null)
            {
                if (horasExtraHandler.HorasExtraExistentes(horasExtra))
                {
                    ModelState.AddModelError("FechaHorasExtra", "Ya tienes una solicitud de horas extra en esa fecha");
                }
                if (validacionesHandler.ValidarSiEsFeriado((DateTime)horasExtra.FechaHorasExtra))
                {
                    ModelState.AddModelError("FechaHorasExtra", "No puedes solicitar horas extra en un día feriado.");
                }
                if (!validacionesHandler.ValidarFechaUnica((DateTime)horasExtra.FechaHorasExtra, (int)horasExtra.Colaborador.IdColaborador))
                {
                    ModelState.AddModelError("FechaHorasExtra", "No puede solicitar horas extra si ya solicito una hora extra, incapacidad, vacacion o permiso en esa misma fecha.");
                }
            }
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

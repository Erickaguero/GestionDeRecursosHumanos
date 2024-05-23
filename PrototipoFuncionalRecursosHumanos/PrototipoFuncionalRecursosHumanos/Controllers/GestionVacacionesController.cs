using Microsoft.AspNetCore.Mvc;
using PrototipoFuncionalRecursosHumanos.Models;
using PrototipoFuncionalRecursosHumanos.Services;

namespace PrototipoFuncionalRecursosHumanos.Controllers
{
    public class GestionVacacionesController : Controller
    {
        private Autenticador authenticator = new Autenticador();
        private ColaboradorHandler colaboradorHandler = new ColaboradorHandler();
        private VacacionesHandler vacacionesHandler = new VacacionesHandler();

        [HttpGet]
        public IActionResult SolicitarVacaciones()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            List <Vacaciones> vacaciones = vacacionesHandler.ObtenerVacaciones(colaborador.IdColaborador);
            foreach (var vacacion in vacaciones)
            {
                vacacion.Colaborador = colaborador;
            }
            ViewBag.VacacionesAprobadas = vacaciones.Where(vacacion => vacacion.Estado == "Aprobado").ToList();
            ViewBag.VacacionesRechazadas = vacaciones.Where(vacacion => vacacion.Estado == "Rechazado").ToList();
            ViewBag.VacacionesPendientes = vacaciones.Where(vacacion => vacacion.Estado == "Pendiente" || vacacion.Estado == "Aprobado por jefatura").ToList();
            return View();
        }

        [HttpGet]
        public IActionResult CrearSolicitudVacaciones()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            ViewBag.CantidadDiasDisponibles = vacacionesHandler.ObtenerDiasDisponibles(colaborador.IdColaborador);
            return View();
        }

        [HttpPost]
        public IActionResult CrearSolicitudVacaciones(Vacaciones vacacion)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            vacacion.Colaborador = colaborador;
            ViewBag.CantidadDiasDisponibles = vacacionesHandler.ObtenerDiasDisponibles(colaborador.IdColaborador);
            if (ModelState.IsValid)
            {
                if (!TieneSuficientesDiasVacaciones(vacacion))
                {
                    ModelState.AddModelError("FechaFin", "No tienes suficientes días de vacaciones disponibles");
                    return View();
                }
                if (vacacionesHandler.VacacionesExistentes(vacacion))
                {
                    ModelState.AddModelError("FechaFin", "Ya tienes una solicitud de vacaciones en esas fechas");
                    return View();
                }
                vacacion.Estado = "Pendiente";

                if (vacacionesHandler.AgregarVacacion(vacacion)) {
                    return RedirectToAction("SolicitarVacaciones");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult EliminarVacacion(int idVacacion)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            vacacionesHandler.EliminarVacacion(idVacacion);
                    return RedirectToAction("SolicitarVacaciones");
        }

        [HttpGet]
        public IActionResult AprobarVacaciones()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            var rolDeUsuario = colaborador.Usuario.RolDeUsuario.Descripcion;
            if (rolDeUsuario != "administrador" && rolDeUsuario != "jefatura") return RedirectToAction("Index", "Home");
            List <Vacaciones> vacaciones = new List<Vacaciones>();
            if (rolDeUsuario == "administrador") vacaciones = vacacionesHandler.ObtenerVacacionesParaAprobarPorAdministrador(colaborador.IdColaborador);
            if (rolDeUsuario == "jefatura") vacaciones = vacacionesHandler.ObtenerVacacionesParaAprobarPorJefatura(colaborador.IdColaborador);
            ObtenerInformacionColaboradores(vacaciones);
            ViewBag.VacacionesAprobadasPorJefatura = vacaciones.Where(vacacion => vacacion.Estado == "Aprobado por jefatura").ToList();
            ViewBag.VacacionesPendientes = vacaciones.Where(vacacion => vacacion.Estado == "Pendiente").ToList();
            return View();
        }

        [HttpGet]
        public IActionResult AprobarVacacion(int idVacacion)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            var rolDeUsuario = colaborador.Usuario.RolDeUsuario.Descripcion;
            if (rolDeUsuario != "administrador" && rolDeUsuario != "jefatura") return RedirectToAction("Index", "Home");

            if (rolDeUsuario == "administrador") vacacionesHandler.AprobarVacacionAdministrador(idVacacion);
            if (rolDeUsuario == "jefatura") vacacionesHandler.AprobarVacacionJefatura(idVacacion);

            return RedirectToAction("AprobarVacaciones");
        }

        [HttpGet]
        public IActionResult RechazarVacacion(int idVacacion)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            var rolDeUsuario = colaborador.Usuario.RolDeUsuario.Descripcion;
            if (rolDeUsuario != "administrador" && rolDeUsuario != "jefatura") return RedirectToAction("Index", "Home");
            vacacionesHandler.RechazarVacacion(idVacacion);

            return RedirectToAction("AprobarVacaciones");
        }

        public bool TieneSuficientesDiasVacaciones(Vacaciones? vacacion)
        {
            // Calcular los días de vacaciones que el colaborador quiere tomar
            int diasSolicitados = CalcularDiasLaborables(vacacion.FechaInicio, vacacion.FechaFin);

            // Obtener los días de vacaciones a los que tiene derecho el colaborador
            int diasVacaciones = vacacionesHandler.ObtenerDiasDisponibles(vacacion.Colaborador.IdColaborador);

            // Comprobar si el colaborador tiene suficientes días de vacaciones disponibles
            return diasVacaciones >= diasSolicitados;
        }

        public int CalcularDiasLaborables(DateTime? fechaInicio, DateTime? fechaFin)
        {
            int diasTotales = (fechaFin.Value - fechaInicio.Value).Days + 1;
            int diasLaborables = 0;

            for (int i = 0; i < diasTotales; i++)
            {
                DateTime diaActual = fechaInicio.Value.AddDays(i);
                if (diaActual.DayOfWeek != DayOfWeek.Saturday && diaActual.DayOfWeek != DayOfWeek.Sunday)
                {
                    diasLaborables++;
                }
            }

            return diasLaborables;
        }

        public void ObtenerInformacionColaboradores(List<Vacaciones> vacaciones)
        {
            foreach (var vacacion in vacaciones)
            {
                vacacion.Colaborador = colaboradorHandler.ObtenerColaborador((int)vacacion.Colaborador.IdColaborador);
            }
        }
    }
}

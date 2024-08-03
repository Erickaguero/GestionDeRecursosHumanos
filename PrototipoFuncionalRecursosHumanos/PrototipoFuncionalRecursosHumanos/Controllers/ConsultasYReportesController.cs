using Microsoft.AspNetCore.Mvc;
using PrototipoFuncionalRecursosHumanos.Models;
using PrototipoFuncionalRecursosHumanos.Services;
using Rotativa.AspNetCore;
using System.Reflection;

namespace PrototipoFuncionalRecursosHumanos.Controllers
{
    public class ConsultasYReportesController : Controller
    {
        private Autenticador authenticator = new Autenticador();
        private ColaboradorHandler colaboradorHandler = new ColaboradorHandler();
        private AsistenciasHandler asistenciasHandler = new AsistenciasHandler();
        private PlanillaHandler planillaHandler = new PlanillaHandler();
        private AguinaldoHandler aguinaldoHandler = new AguinaldoHandler();
        private LiquidacionHandler liquidacionHandler = new LiquidacionHandler();
        private HorasExtraHandler horasExtraHandler = new HorasExtraHandler();
        private PermisosHandler permisosHandler = new PermisosHandler();
        private TipoPermisosHandler tipoPermisosHandler = new TipoPermisosHandler();
        private IncapacidadesHandler incapacidadesHandler = new IncapacidadesHandler();
        private TipoIncapacidadesHandler tipoIncapacidadesHandler = new TipoIncapacidadesHandler();
        private VacacionesHandler vacacionesHandler = new VacacionesHandler();

        public IActionResult Index(string elementoADesplegar = "", string filtro = "")
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador" || Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");

            var modelo = new ConsultasYReportes();
            modelo.ElementoADesplegar = elementoADesplegar;
            modelo.Filtro = filtro;
            CargarDatos(modelo, elementoADesplegar, filtro);
            modelo.DesplegarBotones = true;
            return View(modelo);
        }

        [HttpPost]
        public IActionResult Index(ConsultasYReportes model)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador" || Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
            {
                model.DesplegarBotones = true;
                return View(model);
            }

            return RedirectToAction("Index", new { elementoADesplegar = model.ElementoADesplegar, filtro = model.Filtro });
        }

        public IActionResult GenerarReporte(string elementoADesplegar = "", string filtro = "")
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerRolColaborador(Request) != "administrador") return RedirectToAction("Index", "Home");
            var modelo = new ConsultasYReportes();
            CargarDatos(modelo, elementoADesplegar, filtro);
            modelo.DesplegarBotones = false;
            // Redirige a la vista que deseas convertir en PDF
            return new ViewAsPdf("Index", modelo);
        }

        private void CargarDatos(ConsultasYReportes modelo, string elementoADesplegar, string filtro)
        {
            switch (elementoADesplegar)
            {
                case "asistencias":
                    modelo.Asistencias = asistenciasHandler.ObtenerAsistencias();
                    foreach (var asistencia in modelo.Asistencias)
                    {
                        asistencia.Colaborador = colaboradorHandler.ObtenerColaborador((int)asistencia.Colaborador.IdColaborador);
                    }
                    modelo.Asistencias = modelo.Asistencias
                        .Where(a => CoincideConFiltro(a, filtro))
                        .ToList();
                    break;
                case "colaboradoresActivos":
                    modelo.ColaboradoresActivos = colaboradorHandler.ObtenerColaboradores();
                    modelo.ColaboradoresActivos = modelo.ColaboradoresActivos
                        .Where(c => CoincideConFiltro(c, filtro))
                        .ToList();
                    break;
                case "colaboradoresInactivos":
                    modelo.ColaboradoresInactivos = colaboradorHandler.ObtenerColaboradoresInactivos();
                    modelo.ColaboradoresInactivos = modelo.ColaboradoresInactivos
                        .Where(c => CoincideConFiltro(c, filtro))
                        .ToList();
                    break;
                case "planilla":
                    modelo.Planillas = planillaHandler.ObtenerPlanillas();
                    foreach (var planilla in modelo.Planillas)
                    {
                        planilla.Colaborador = colaboradorHandler.ObtenerColaborador((int)planilla.Colaborador.IdColaborador);
                    }
                    modelo.Planillas = modelo.Planillas
                        .Where(p => CoincideConFiltro(p, filtro))
                        .ToList();
                    break;
                case "aguinaldo":
                    modelo.Aguinaldos = aguinaldoHandler.ObtenerAguinaldos();
                    foreach (var aguinaldo in modelo.Aguinaldos)
                    {
                        aguinaldo.Colaborador = colaboradorHandler.ObtenerColaborador((int)aguinaldo.Colaborador.IdColaborador);
                    }
                    modelo.Aguinaldos = modelo.Aguinaldos
                        .Where(a => CoincideConFiltro(a, filtro))
                        .ToList();
                    break;
                case "liquidacion":
                    modelo.Liquidaciones = liquidacionHandler.ObtenerLiquidaciones();
                    foreach (var liquidacion in modelo.Liquidaciones)
                    {
                        liquidacion.Colaborador = colaboradorHandler.ObtenerColaborador((int)liquidacion.Colaborador.IdColaborador);
                    }
                    modelo.Liquidaciones = modelo.Liquidaciones
                        .Where(l => CoincideConFiltro(l, filtro))
                        .ToList();
                    break;
                case "horasExtra":
                    modelo.HorasExtras = horasExtraHandler.ObtenerHorasExtra();
                    foreach (var horaExtra in modelo.HorasExtras)
                    {
                        horaExtra.Colaborador = colaboradorHandler.ObtenerColaborador((int)horaExtra.Colaborador.IdColaborador);
                    }
                    modelo.HorasExtras = modelo.HorasExtras
                        .Where(h => CoincideConFiltro(h, filtro))
                        .ToList();
                    break;
                case "permisos":
                    modelo.Permisos = permisosHandler.ObtenerPermisos();
                    foreach (var permiso in modelo.Permisos)
                    {
                        permiso.Colaborador = colaboradorHandler.ObtenerColaborador((int)permiso.Colaborador.IdColaborador);
                        permiso.TipoPermiso = tipoPermisosHandler.ObtenerTipoPermiso((int)permiso.TipoPermiso.IdTipoPermiso);
                    }
                    modelo.Permisos = modelo.Permisos
                        .Where(p => CoincideConFiltro(p, filtro))
                        .ToList();
                    break;
                case "incapacidades":
                    modelo.Incapacidades = incapacidadesHandler.ObtenerIncapacidades();
                    foreach (var incapacidad in modelo.Incapacidades)
                    {
                        incapacidad.Colaborador = colaboradorHandler.ObtenerColaborador((int)incapacidad.Colaborador.IdColaborador);
                        incapacidad.TipoIncapacidad = tipoIncapacidadesHandler.ObtenerTipoIncapacidad((int)incapacidad.TipoIncapacidad.IdTipoIncapacidad);
                    }
                    modelo.Incapacidades = modelo.Incapacidades
                        .Where(i => CoincideConFiltro(i, filtro))
                        .ToList();
                    break;
                case "vacaciones":
                    modelo.Vacaciones = vacacionesHandler.ObtenerVacaciones();
                    foreach (var vacacion in modelo.Vacaciones)
                    {
                        vacacion.Colaborador = colaboradorHandler.ObtenerColaborador((int)vacacion.Colaborador.IdColaborador);
                    }
                    modelo.Vacaciones = modelo.Vacaciones
                        .Where(v => CoincideConFiltro(v, filtro))
                        .ToList();
                    break;
            }
        }
        private bool CoincideConFiltro(object obj, string filtro)
        {
            if (obj == null)
            {
                return false;
            }

            var tipo = obj.GetType();
            var propiedades = tipo.GetProperties();
            filtro = filtro.ToLower();

            foreach (var propiedad in propiedades)
            {
                var valor = propiedad.GetValue(obj);

                // Caso especial para propiedades DateTime
                if (valor is DateTime fecha)
                {
                    if (fecha.ToString("dd/MM/yyyy").Contains(filtro))
                    {
                        return true;
                    }
                }
                // Convierte el valor de la propiedad a una cadena y verifica si coincide con el filtro
                else if (valor != null && valor.ToString().ToLower().Contains(filtro))
                {
                    return true;
                }

                // Si la propiedad es una clase (excepto para string, que ya hemos manejado), 
                // verifica recursivamente sus propiedades
                else if (valor != null && propiedad.PropertyType.IsClass && !(valor is string))
                {
                    if (CoincideConFiltro(valor, filtro))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using PrototipoFuncionalRecursosHumanos.Handlers;
using PrototipoFuncionalRecursosHumanos.Models;
using PrototipoFuncionalRecursosHumanos.Services;

namespace PrototipoFuncionalRecursosHumanos.Controllers
{
    public class SimulacionController : Controller
    {
        private Autenticador authenticator = new Autenticador();
        private ColaboradorHandler colaboradorHandler = new ColaboradorHandler();
        private SimulacionHandler simulacionHandler = new SimulacionHandler();

        [HttpGet]
        public IActionResult Index()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpGet]
        public IActionResult GenerarAsistenciasColaborador()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);

            if (simulacionHandler.GenerarAsistencia((int)colaborador.IdColaborador))
            {
                var alerta = Alertas.Exito("Se generarón las asistencias con exito.");
                TempData["Alerta"] = JsonConvert.SerializeObject(alerta);
            }
            else
            {
                var alerta = Alertas.Error("Hubo un error al generar las asistencias.");
                TempData["Alerta"] = JsonConvert.SerializeObject(alerta);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GenerarHoraExtraHoy()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);

            if (simulacionHandler.ValidarFuncionamientoHorasExtra((int)colaborador.IdColaborador))
            {
                var alerta = Alertas.Exito("Se generó un registro de asistencia indicando que el colaborador salio hoy a las 8pm.");
                TempData["Alerta"] = JsonConvert.SerializeObject(alerta);
            }
            else
            {
                var alerta = Alertas.Error("Hubo un error al generar la asistencia indicando que el colaborador salio hoy a las 8pm.");
                TempData["Alerta"] = JsonConvert.SerializeObject(alerta);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GenerarFeriados()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");
            if (simulacionHandler.GenerarFeriados())
            {
                var alerta = Alertas.Exito("Se generarón feriados para hoy y los siguientes dos días.");
                TempData["Alerta"] = JsonConvert.SerializeObject(alerta);
            }
            else
            {
                var alerta = Alertas.Error("Hubo un error al generar los feriados");
                TempData["Alerta"] = JsonConvert.SerializeObject(alerta);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ReiniciarFeriadosConValoresPorDefecto()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");
            if (simulacionHandler.ReiniciarFeriados())
            {
                var alerta = Alertas.Exito("Se reiniciarón los feriados a su estado original.");
                TempData["Alerta"] = JsonConvert.SerializeObject(alerta);
            }
            else
            {
                var alerta = Alertas.Error("Hubo un error al reiniciar los feriados a su estado original");
                TempData["Alerta"] = JsonConvert.SerializeObject(alerta);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ValidarAguinaldo()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            var resultadoAguinlado = simulacionHandler.ValidarFuncionamientoAguinaldo((int)colaborador.IdColaborador);
            if (resultadoAguinlado != 0)
            {
                var alerta = Alertas.Exito("El valor del aguinaldo es: " + resultadoAguinlado.ToString());
                TempData["Alerta"] = JsonConvert.SerializeObject(alerta);
            }
            else
            {
                var alerta = Alertas.Error("Hubo un error al validar el aguinaldo");
                TempData["Alerta"] = JsonConvert.SerializeObject(alerta);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ValidarImpuestoRenta(double salario)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");
            var resultadoImpuestoRenta = simulacionHandler.CalcularImpuestoRentaQuincenal(salario / 2);
            var alerta = Alertas.Exito("El impuesto de renta quincenal seria " + resultadoImpuestoRenta.ToString() + " y el impuesto de renta mensual seria: " + (resultadoImpuestoRenta * 2).ToString());
            TempData["Alerta"] = JsonConvert.SerializeObject(alerta);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ModificarFechaContratacionAnio()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);

            if (simulacionHandler.ModificarFechaContratacionPorAnio((int)colaborador.IdColaborador))
            {
                var alerta = Alertas.Exito("Se modifico la fecha de contratación del colaborador.");
                TempData["Alerta"] = JsonConvert.SerializeObject(alerta);
            }
            else
            {
                var alerta = Alertas.Error("Hubo un error al modificar la fecha de contratación.");
                TempData["Alerta"] = JsonConvert.SerializeObject(alerta);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ModificarFechaContratacionMes()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);

            if (simulacionHandler.ModificarFechaContratacionPorMes((int)colaborador.IdColaborador))
            {
                var alerta = Alertas.Exito("Se modifico la fecha de contratación del colaborador.");
                TempData["Alerta"] = JsonConvert.SerializeObject(alerta);
            }
            else
            {
                var alerta = Alertas.Error("Hubo un error al modificar la fecha de contratación.");
                TempData["Alerta"] = JsonConvert.SerializeObject(alerta);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult SimularPlanillaColaborador(float horasExtra, float horasIncapacidades, float horasPermiso, float horasTrabajadas, float horasVacaciones)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            bool exito = simulacionHandler.SimularPlanillaColaborador((int)colaborador.IdColaborador, horasExtra, horasIncapacidades, horasPermiso, horasTrabajadas, horasVacaciones);
            if (exito)
            {
                var alerta = Alertas.Exito("La simulación de la planilla se realizó con éxito.");
                TempData["Alerta"] = JsonConvert.SerializeObject(alerta);
            }
            else
            {
                var alerta = Alertas.Error("Hubo un error al simular la planilla.");
                TempData["Alerta"] = JsonConvert.SerializeObject(alerta);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EliminarPlanillas()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");
            bool exito = simulacionHandler.EliminarPlanillas();
            if (exito)
            {
                var alerta = Alertas.Exito("La eliminación de las planillas se realizó con éxito.");
                TempData["Alerta"] = JsonConvert.SerializeObject(alerta);
            }
            else
            {
                var alerta = Alertas.Error("Hubo un error al eliminar las planillas.");
                TempData["Alerta"] = JsonConvert.SerializeObject(alerta);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EliminarAguinaldos()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");

            bool exito = simulacionHandler.EliminarAguinaldos();
            if (exito)
            {
                var alerta = Alertas.Exito("La eliminación de los aguinaldos se realizó con éxito.");
                TempData["Alerta"] = JsonConvert.SerializeObject(alerta);
            }
            else
            {
                var alerta = Alertas.Error("Hubo un error al eliminar los aguinaldos.");
                TempData["Alerta"] = JsonConvert.SerializeObject(alerta);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EliminarLiquidaciones()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            if (Autorizador.ObtenerEstadoColaborador(Request) != "activo") return RedirectToAction("Index", "Home");

            bool exito = simulacionHandler.EliminarLiquidaciones();
            if (exito)
            {
                var alerta = Alertas.Exito("La eliminación de las liquidaciones se realizó con éxito.");
                TempData["Alerta"] = JsonConvert.SerializeObject(alerta);
            }
            else
            {
                var alerta = Alertas.Error("Hubo un error al eliminar las liquidaciones.");
                TempData["Alerta"] = JsonConvert.SerializeObject(alerta);
            }

            return RedirectToAction("Index");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using PrototipoFuncionalRecursosHumanos.Models;
using PrototipoFuncionalRecursosHumanos.Services;
using System.Reflection;

namespace PrototipoFuncionalRecursosHumanos.Controllers
{
    public class EvaluacionEmpleadoController : Controller
    {
        private Autenticador authenticator = new Autenticador();
        private ColaboradorHandler colaboradorHandler = new ColaboradorHandler();
        private EvaluacionHandler evaluacionHandler = new EvaluacionHandler();

        public IActionResult Index()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            var rolDeUsuario = colaborador.Usuario.RolDeUsuario.Descripcion;
            if (rolDeUsuario != "administrador") return RedirectToAction("Index", "Home");

            List<Colaborador> colaboradores = colaboradorHandler.ObtenerColaboradores();
            colaboradores.RemoveAll(c => c.IdColaborador == colaborador.IdColaborador);
            return View(colaboradores);
        }

        public IActionResult EvaluarEmpleado(int idColaborador)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            if (colaborador.IdColaborador == idColaborador) return RedirectToAction("Index", "Home");
            var rolDeUsuario = colaborador.Usuario.RolDeUsuario.Descripcion;
            if (rolDeUsuario != "administrador") return RedirectToAction("Index", "Home");
            Evaluacion evaluacion = new Evaluacion();
            evaluacion.Colaborador = colaborador;

            evaluacion.Preguntas = new List<Pregunta>
            {
                new Pregunta("¿El colaborador muestra iniciativa en su trabajo?"),
                new Pregunta("¿El colaborador se adapta bien a los cambios?"),
                new Pregunta("¿El colaborador trabaja bien en equipo?"),
                new Pregunta("¿El colaborador se esfuerza por mejorar sus habilidades y conocimientos?"),
                new Pregunta("¿El colaborador respeta las opiniones y sugerencias de los demás?"),
                new Pregunta("¿El colaborador cumple con los plazos establecidos?"),
                new Pregunta("¿El colaborador contribuye a un ambiente de trabajo positivo?")
            };

            return View(evaluacion);
        }

        [HttpPost]
        public IActionResult EvaluarEmpleado(Evaluacion evaluacion)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            var rolDeUsuario = colaborador.Usuario.RolDeUsuario.Descripcion;
            if (rolDeUsuario != "administrador") return RedirectToAction("Index", "Home");
            evaluacion.Colaborador = colaborador;
            if (ModelState.IsValid) 
            {
                if (evaluacion.Preguntas.Any(p => p.Respuesta == null))
                {
                    // TODO: Implementar un mensaje de error en la vista
                    return View(evaluacion);
                }
                evaluacion.PromedioEvaluacion = CalcularNotaEvaluacion(evaluacion.Preguntas);
                if (evaluacionHandler.AgregarEvaluacion(evaluacion))
                {
                    return RedirectToAction("Index");
                }
            }
            return View(evaluacion);
        }

        public IActionResult ListaEvaluaciones(int idColaborador)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            if (colaborador.IdColaborador == idColaborador) return RedirectToAction("Index", "Home");
            var rolDeUsuario = colaborador.Usuario.RolDeUsuario.Descripcion;
            if (rolDeUsuario != "administrador") return RedirectToAction("Index", "Home");

            List<Evaluacion> evaluaciones = evaluacionHandler.ObtenerEvaluaciones(idColaborador);
            return View(evaluaciones);
        }

        public IActionResult EliminarEvaluacion(int idColaborador, int idEvaluacion)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            if (colaborador.IdColaborador == idColaborador) return RedirectToAction("Index", "Home");
            var rolDeUsuario = colaborador.Usuario.RolDeUsuario.Descripcion;
            if (rolDeUsuario != "administrador") return RedirectToAction("Index", "Home");
            return RedirectToAction("Index");
        }

        public double CalcularNotaEvaluacion(List<Pregunta> preguntas)
        {
            int ptsObtenidos = 0;
            int ptsMaximos = 0;
            foreach (var pregunta in preguntas)
            {
                ptsObtenidos += pregunta.Respuesta ?? 0;
                ptsMaximos += 5;
            }
            double notaFinal = (ptsObtenidos * 100) / ptsMaximos;
            return notaFinal;
        }
    }
}

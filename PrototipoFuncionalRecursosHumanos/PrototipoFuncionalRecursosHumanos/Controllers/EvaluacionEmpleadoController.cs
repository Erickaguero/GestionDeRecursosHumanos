using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
            var colaboradorEvaluado = colaboradorHandler.ObtenerColaborador(idColaborador);
            evaluacion.Colaborador = colaboradorEvaluado;

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
            var colaboradorEvaluado = colaboradorHandler.ObtenerColaborador((int)evaluacion.Colaborador.IdColaborador);
            evaluacion.Colaborador = colaboradorEvaluado;
            ValidarEvaluacion(evaluacion, ModelState);
            if (ModelState.IsValid) 
            {

                evaluacion.NotaEvaluacion = CalcularNotaEvaluacion(evaluacion.Preguntas);
                if (evaluacionHandler.AgregarEvaluacion(evaluacion))
                {
                    return RedirectToAction("Index");
                }
            }
            // Definir una lista de preguntas predeterminadas
            var preguntasPredeterminadas = new List<Pregunta>
            {
                new Pregunta("¿El colaborador muestra iniciativa en su trabajo?"),
                new Pregunta("¿El colaborador se adapta bien a los cambios?"),
                new Pregunta("¿El colaborador trabaja bien en equipo?"),
                new Pregunta("¿El colaborador se esfuerza por mejorar sus habilidades y conocimientos?"),
                new Pregunta("¿El colaborador respeta las opiniones y sugerencias de los demás?"),
                new Pregunta("¿El colaborador cumple con los plazos establecidos?"),
                new Pregunta("¿El colaborador contribuye a un ambiente de trabajo positivo?")
            };

            for (int i = 0; i < preguntasPredeterminadas.Count; i++)
            {
                if (evaluacion.Preguntas[i] == null)
                {
                    evaluacion.Preguntas[i] = preguntasPredeterminadas[i];
                } else if (evaluacion.Preguntas[i].Texto == null)
                {
                    evaluacion.Preguntas[i].Texto = preguntasPredeterminadas[i].Texto;
                }
            }
            return View(evaluacion);
        }

        public IActionResult ListaEvaluaciones(int idColaborador = -1)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");

            if (idColaborador == -1)
            {
                var colaborador = colaboradorHandler.ObtenerColaborador(correo);
                idColaborador = (int)colaborador.IdColaborador;
            }
            List<Evaluacion> evaluaciones = evaluacionHandler.ObtenerEvaluaciones(idColaborador);
            return View(evaluaciones);
        }

        public IActionResult EliminarEvaluacion(int idColaborador, int idEvaluacion)
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");
            var colaborador = colaboradorHandler.ObtenerColaborador(correo);
            var rolDeUsuario = colaborador.Usuario.RolDeUsuario.Descripcion;
            if (rolDeUsuario != "administrador") return RedirectToAction("ListaEvaluaciones", new { idColaborador = idColaborador });
            evaluacionHandler.EliminarEvaluacion(idEvaluacion);
            return RedirectToAction("ListaEvaluaciones", new { idColaborador = idColaborador });
        }

        public void ValidarEvaluacion(Evaluacion evaluacion, ModelStateDictionary ModelState)
        {
            if (evaluacion.Preguntas.Any(pregunta => pregunta == null || pregunta.Respuesta == null))
            {
                ModelState.AddModelError("Preguntas", "Debes llenar todas las preguntas obligatoriamente.");
            }
        }

        public double CalcularNotaEvaluacion(List<Pregunta> preguntas)
        {
            int ptsObtenidos = 0;
            int ptsMaximos = 0;
            foreach (var pregunta in preguntas)
            {
                ptsObtenidos += pregunta.Respuesta ?? 0;
                ptsMaximos += 4;
            }
            double notaFinal = (ptsObtenidos * 100) / ptsMaximos;
            return notaFinal;
        }
    }
}

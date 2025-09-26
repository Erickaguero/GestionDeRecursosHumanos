using Microsoft.AspNetCore.Mvc;
using PrototipoFuncionalRecursosHumanos.Models;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using PrototipoFuncionalRecursosHumanos.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting.Server;

namespace PrototipoFuncionalRecursosHumanos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private Autenticador authenticator = new Autenticador();
        private EnviadorCorreos emailSender = new EnviadorCorreos();
        private GeneradorContrasena passwordGenerator = new GeneradorContrasena();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("IniciarSesion");
        }

        [HttpGet]
        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public IActionResult IniciarSesion(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                UsuarioHandler usuarioHandler = new UsuarioHandler();
                Usuario usuarioObtenido = usuarioHandler.ObtenerUsuario(usuario.Correo);    
                if (usuarioObtenido != null)
                {
                    if (usuario.Contrasena.Equals(usuarioObtenido.Contrasena))
                    {
                        authenticator.CrearToken(usuario.Correo, Response);
                        return RedirectToAction("MenuPrincipal");
                    } else
                    {
                        ModelState.AddModelError("Contrasena", "El usuario o contraseña no son válidos");
                    }
                } else
                {
                    ModelState.AddModelError("Contrasena", "El usuario o contraseña no son válidos");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult CerrarSesion()
        {
            Response.Cookies.Delete("token");
            return RedirectToAction("IniciarSesion");
        }

        [HttpGet]
        public IActionResult RecuperarContrasena()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RecuperarContrasena(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                UsuarioHandler usuarioHandler = new UsuarioHandler();
                Usuario usuarioObtenido = usuarioHandler.ObtenerUsuario(usuario.Correo);
                if (usuarioObtenido != null)
                {
                    var contrasenaNueva = passwordGenerator.GenerarContrasenaSegura();
                    if (usuarioHandler.ModificarContrasena(usuario.Correo, contrasenaNueva))
                    {
                        emailSender.EnviarCorreoRecuperarContrasena(usuarioObtenido.Correo, contrasenaNueva);
                        return RedirectToAction("IniciarSesion");
                    }
                } else
                {
                    ModelState.AddModelError("Correo", "Datos inválidos para recuperar contraseña");
                }
            }
            ModelState.AddModelError("Correo", "Hubo un error a la hora de procesar su peticion");
            return View();
        }

        [HttpGet]
        public IActionResult MenuPrincipal()
        {
            var correo = authenticator.ValidarToken(Request);
            if (correo == null) return RedirectToAction("Index", "Home");

            MenuPrincipal modelo = new MenuPrincipal();
            ObtenerImagenesEmpleados(modelo);
            ObtenerInformacionColaborador(modelo, correo);
            return View(modelo);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public void ObtenerImagenesEmpleados(MenuPrincipal modelo)
        {
            var app = WebApplication.CreateBuilder();
            string rutaRelativa = "images/employees";
            string rutaCarpeta = Path.Combine(app.Environment.WebRootPath, rutaRelativa);
            List<string> archivosImagen = Directory.EnumerateFiles(rutaCarpeta)
                .Where(file => file.EndsWith(".jpeg") || file.EndsWith(".jpg"))
                .Select(archivo => Path.Combine("~/" + rutaRelativa, Path.GetFileName(archivo)).Replace("\\", "/"))
                .ToList();

            modelo.UbicacionesImagenesColaboradores = archivosImagen;
        }

        public void ObtenerInformacionColaborador(MenuPrincipal modelo, string correo)
        {
            ColaboradorHandler colaboradorHandler = new ColaboradorHandler();
            Colaborador colaborador = colaboradorHandler.ObtenerColaborador(correo);
            modelo.NombreCompletoColaborador = colaborador.Persona.Nombre + " " + colaborador.Persona.Apellido1 + " " + colaborador.Persona.Apellido2;
            modelo.HorasTrabajadasPeriodo = colaboradorHandler.CalcularHorasTrabajadasPorColaboradorId((int)colaborador.IdColaborador);
            modelo.HorasExtrasPeriodo = colaboradorHandler.CalcularHorasExtraPorColaboradorId((int)colaborador.IdColaborador);
            modelo.HorasPermisoPeriodo = colaboradorHandler.CalcularHorasPermisoPorColaboradorId((int)colaborador.IdColaborador);
            modelo.HorasIncapacidadPeriodo =  colaboradorHandler.CalcularHorasIncapacidadesPorColaboradorId((int)colaborador.IdColaborador);

        }
    }
}

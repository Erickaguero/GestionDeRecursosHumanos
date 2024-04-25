using Microsoft.AspNetCore.Mvc;
using PrototipoFuncionalRecursosHumanos.Models;
using System.Diagnostics;

namespace PrototipoFuncionalRecursosHumanos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                UsuarioHandler usuarioHandler = new UsuarioHandler();
                Usuario usuarioObtenido = usuarioHandler.ObtenerUsuario(usuario.Correo);    
                if (usuarioObtenido != null)
                {
                    if (usuario.Contrasena.Equals(usuarioObtenido.Contrasena))
                    {
                        return RedirectToAction("MenuPrincipal");
                    } else
                    {
                        ModelState.AddModelError("Contrasena", "El usuario o contraseña no son validos");
                    }
                } else
                {
                    ModelState.AddModelError("Contrasena", "El usuario o contraseña no son validos");
                }
            }
            return View();
        }

        public IActionResult MenuPrincipal()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

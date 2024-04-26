using Microsoft.AspNetCore.Mvc;
using PrototipoFuncionalRecursosHumanos.Models;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;

namespace PrototipoFuncionalRecursosHumanos.Controllers
{
    public class MantenimientoColaboradoresController : Controller
    {
        public ColaboradorHandler colaboradorHandler = new ColaboradorHandler();

        [HttpGet]
        public IActionResult Index()
        {
            List<Colaborador> colaboradores = colaboradorHandler.ObtenerColaboradores();
            return View(colaboradores);
        }

        [HttpGet]
        public IActionResult CrearColaborador()
        {
            RolDeUsuarioHandler rolDeUsuarioHandler = new RolDeUsuarioHandler();
            ViewBag.RolesDeUsuario = rolDeUsuarioHandler.ObtenerRolesDeUsuario();
            return View();
        }

        [HttpPost]
        public IActionResult CrearColaborador(Colaborador colaborador)
        {
            if (ModelState.IsValid)
            {
                colaborador.Usuario.Contrasena = GenerarContrasenaSegura();
                colaboradorHandler.AgregarColaborador(colaborador);
                EnviarCorreoColaborador(colaborador);
                return RedirectToAction("Index"); // Redirige al usuario a la página de inicio después de agregar el colaborador
            }
            // Si los modelos no son válidos, devuelve la vista con los modelos para mostrar los errores de validación
            RolDeUsuarioHandler rolDeUsuarioHandler = new RolDeUsuarioHandler();
            ViewBag.RolesDeUsuario = rolDeUsuarioHandler.ObtenerRolesDeUsuario();
            return View();
        }

        [HttpGet]
        public IActionResult EditarColaborador(int idColaborador)
        {
            Colaborador colaborador = colaboradorHandler.ObtenerColaborador(idColaborador);
            if (colaborador == null)
            {
                return NotFound();
            }

            TempData["IdColaborador"] = colaborador.IdColaborador;
            TempData["ContrasenaUsuario"] = colaborador.Usuario.Contrasena;
            RolDeUsuarioHandler rolDeUsuarioHandler = new RolDeUsuarioHandler();

            ViewBag.RolesDeUsuario = rolDeUsuarioHandler.ObtenerRolesDeUsuario();

            return View(colaborador);
        }

        [HttpPost]
        public IActionResult EditarColaborador(Colaborador colaborador)
        {
            if (ModelState.IsValid)
            {
                if (TempData["IdColaborador"] != null && TempData["ContrasenaUsuario"] != null)
                {
                    colaborador.IdColaborador = (int)TempData["IdColaborador"];
                    colaborador.Usuario.Contrasena = (string)TempData["ContrasenaUsuario"];
                    colaboradorHandler.EditarColaborador(colaborador);
                    return RedirectToAction("Index"); // Redirige al usuario a la página de inicio después de agregar el colaborador
                }
            }
            // Si los modelos no son válidos, devuelve la vista con los modelos para mostrar los errores de validación
            RolDeUsuarioHandler rolDeUsuarioHandler = new RolDeUsuarioHandler();
            ViewBag.RolesDeUsuario = rolDeUsuarioHandler.ObtenerRolesDeUsuario();
            return View();
        }


        [HttpGet]
        public IActionResult EliminarColaborador(int idColaborador)
        {
            colaboradorHandler.EliminarColaborador(idColaborador);
            return RedirectToAction("Index");
        }

        public string GenerarContrasenaSegura()
        {
            int longitud = 8;
            string caracteresPermitidos = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890@#_";
            char[] chars = new char[longitud];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                for (int i = 0; i < longitud; i++)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    chars[i] = caracteresPermitidos[(int)(num % (uint)caracteresPermitidos.Length)];
                }
            }

            return new string(chars);
        }

        public void EnviarCorreoColaborador(Colaborador colaborador)
        {
            var builder = WebApplication.CreateBuilder();
            var correoAplicacion = builder.Configuration["Email:Username"];
            var contrasenaCorreoAplicacion = builder.Configuration["Email:Password"];
            var correo = new MailMessage();
            correo.From = new MailAddress(correoAplicacion);
            correo.To.Add(colaborador.Usuario.Correo);
            correo.Subject = "Bienvenido al equipo de trabajo " + colaborador.Persona.Nombre ;
            correo.Body = "Esta es tu contraseña para que inicies sesion: " + colaborador.Usuario.Contrasena;
            correo.IsBodyHtml = false;

            using (var smtp = new SmtpClient("smtp.gmail.com"))
            {
                smtp.Port = 587; // El puerto para Gmail generalmente es 587
                smtp.Credentials = new NetworkCredential(correoAplicacion, contrasenaCorreoAplicacion);
                smtp.EnableSsl = true; // Gmail requiere SSL
                smtp.Send(correo);
            }
        }
    }
}

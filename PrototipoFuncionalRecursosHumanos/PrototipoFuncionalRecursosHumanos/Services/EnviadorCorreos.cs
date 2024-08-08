using PrototipoFuncionalRecursosHumanos.Models;
using System.Net.Mail;
using System.Net;

namespace PrototipoFuncionalRecursosHumanos.Services
{
    public class EnviadorCorreos
    {
        public EnviadorCorreos()
        {
        }

        public void EnviarCorreoColaborador(Colaborador colaborador)
        {
            var builder = WebApplication.CreateBuilder();
            var correoAplicacion = builder.Configuration["Email:Username"];
            var contrasenaCorreoAplicacion = builder.Configuration["Email:Password"];
            var correo = new MailMessage();
            correo.From = new MailAddress(correoAplicacion);
            correo.To.Add(colaborador.Usuario.Correo);
            correo.Subject = "menu_icon bienvenido al equipo de trabajo " + colaborador.Persona.Nombre;
            correo.Body = "Esta es tu contraseña para que inicies sesión: " + colaborador.Usuario.Contrasena;
            correo.Body += "\n\nPuedes iniciar sesión en el sistema a través de este enlace: https://localhost:7120/";
            correo.IsBodyHtml = false;

            using (var smtp = new SmtpClient("smtp.gmail.com"))
            {
                smtp.Port = 587; // El puerto para Gmail generalmente es 587
                smtp.Credentials = new NetworkCredential(correoAplicacion, contrasenaCorreoAplicacion);
                smtp.EnableSsl = true; // Gmail requiere SSL
                smtp.Send(correo);
            }
        }

        public void EnviarCorreoRecuperarContrasena(string correoUsuario, string contrasenaNueva)
        {
            var builder = WebApplication.CreateBuilder();
            var correoAplicacion = builder.Configuration["Email:Username"];
            var contrasenaCorreoAplicacion = builder.Configuration["Email:Password"];
            var correo = new MailMessage();
            correo.From = new MailAddress(correoAplicacion);
            correo.To.Add(correoUsuario);
            correo.Subject = "Se hizo una solicitud de recuperación de contraseña";
            correo.Body = "Esta es tu nueva contraseña para que inicies sesión: " + contrasenaNueva;
            correo.Body += "\n\nPuedes iniciar sesión en el sistema a través de este enlace: https://localhost:7120/";
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

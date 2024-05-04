using PrototipoFuncionalRecursosHumanos.Models;
using System.Net.Mail;
using System.Net;

namespace PrototipoFuncionalRecursosHumanos.Services
{
    public class EmailSender
    {
        public EmailSender()
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
            correo.Subject = "Bienvenido al equipo de trabajo " + colaborador.Persona.Nombre;
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

        public void EnviarCorreoRecuperarContrasena(string correoUsuario, string contrasenaNueva)
        {
            var builder = WebApplication.CreateBuilder();
            var correoAplicacion = builder.Configuration["Email:Username"];
            var contrasenaCorreoAplicacion = builder.Configuration["Email:Password"];
            var correo = new MailMessage();
            correo.From = new MailAddress(correoAplicacion);
            correo.To.Add(correoUsuario);
            correo.Subject = "Se hizo una solicitud de recuperación de contraseña";
            correo.Body = "Esta es tu nueva contraseña para que inicies sesion: " + contrasenaNueva;
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

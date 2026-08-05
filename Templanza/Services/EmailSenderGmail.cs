using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Templanza.Data;
using Templanza.Models;

namespace Templanza.Services
{
    // Manda emails reales por Gmail SMTP y deja registro en CorreoEnviado.
    public class EmailSenderGmail : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmailSenderGmail> _logger;

        public EmailSenderGmail(IConfiguration configuration, ApplicationDbContext context, ILogger<EmailSenderGmail> logger)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;
        }

        public async Task<bool> EnviarAsync(string destinatario, string asunto, string cuerpoHtml)
        {
            var usuario = _configuration["Smtp:Usuario"]
                ?? throw new InvalidOperationException("Falta configurar Smtp:Usuario (dotnet user-secrets).");
            var contrasenia = _configuration["Smtp:Password"]
                ?? throw new InvalidOperationException("Falta configurar Smtp:Password (dotnet user-secrets).");
            var nombreRemitente = _configuration["Smtp:NombreRemitente"] ?? "Templanza";

            var exito = true;
            try
            {
                var mensaje = new MimeMessage();
                mensaje.From.Add(new MailboxAddress(nombreRemitente, usuario));
                mensaje.To.Add(MailboxAddress.Parse(destinatario));
                mensaje.Subject = asunto;
                mensaje.Body = new TextPart("html") { Text = cuerpoHtml };

                using var cliente = new SmtpClient();
                await cliente.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await cliente.AuthenticateAsync(usuario, contrasenia);
                await cliente.SendAsync(mensaje);
                await cliente.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                exito = false;
                _logger.LogError(ex, "Error al enviar email a {Destinatario}", destinatario);
            }

            _context.CorreosEnviados.Add(new CorreoEnviado
            {
                Destinatario = destinatario,
                Asunto = asunto,
                Cuerpo = cuerpoHtml,
                FechaEnvio = DateTime.Now,
                Exito = exito
            });
            await _context.SaveChangesAsync();

            return exito;
        }
    }
}

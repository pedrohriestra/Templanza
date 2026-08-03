namespace Templanza.Services
{
    /// <summary>
    /// Las páginas propias de Identity UI (ForgotPassword, ResetPassword, cambio de email
    /// desde el perfil, etc.) dependen de Microsoft.AspNetCore.Identity.UI.Services.IEmailSender,
    /// no de nuestra interfaz. Este adapter delega en EmailSenderGmail para que esas páginas
    /// también manden el correo real en vez de usar el no-op interno del paquete.
    /// </summary>
    public class IdentityUiEmailSenderAdapter : Microsoft.AspNetCore.Identity.UI.Services.IEmailSender
    {
        private readonly IEmailSender _emailSender;

        public IdentityUiEmailSenderAdapter(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
            => _emailSender.EnviarAsync(email, subject, htmlMessage);
    }
}

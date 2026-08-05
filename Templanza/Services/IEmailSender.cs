namespace Templanza.Services
{
    // Contrato propio para enviar emails (implementado por EmailSenderGmail).
    public interface IEmailSender
    {
        Task<bool> EnviarAsync(string destinatario, string asunto, string cuerpoHtml);
    }
}

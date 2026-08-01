namespace Templanza.Services
{
    public interface IEmailSender
    {
        Task<bool> EnviarAsync(string destinatario, string asunto, string cuerpoHtml);
    }
}

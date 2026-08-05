using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Templanza.Data;
using Templanza.Models;

namespace Templanza.Services
{
    // Manda emails por la API HTTP de Resend (Render bloquea las conexiones SMTP salientes) y deja registro en CorreoEnviado.
    public class EmailSenderResend : IEmailSender
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmailSenderResend> _logger;

        public EmailSenderResend(HttpClient httpClient, IConfiguration configuration, ApplicationDbContext context, ILogger<EmailSenderResend> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _context = context;
            _logger = logger;
        }

        public async Task<bool> EnviarAsync(string destinatario, string asunto, string cuerpoHtml)
        {
            var apiKey = _configuration["Resend:ApiKey"]
                ?? throw new InvalidOperationException("Falta configurar Resend:ApiKey.");
            var remitente = _configuration["Resend:Remitente"] ?? "Templanza <onboarding@resend.dev>";

            var exito = true;
            try
            {
                var payload = JsonSerializer.Serialize(new
                {
                    from = remitente,
                    to = new[] { destinatario },
                    subject = asunto,
                    html = cuerpoHtml
                });

                using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.resend.com/emails")
                {
                    Content = new StringContent(payload, Encoding.UTF8, "application/json")
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                using var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    exito = false;
                    var body = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Resend respondió {StatusCode}: {Body}", response.StatusCode, body);
                }
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

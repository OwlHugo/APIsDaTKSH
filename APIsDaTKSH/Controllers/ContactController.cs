// Controllers/ContactController.cs
using APIsDaTKSH.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace APIsDaTKSH.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ContactController> _logger;

        public ContactController(IConfiguration configuration, ILogger<ContactController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ContactModel contactForm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await SendEmail(contactForm);

                return Ok("Formulário de contato recebido com sucesso!");
            }
            catch (SmtpException ex)
            {
                _logger.LogError(ex, "Erro ao enviar e-mail");
                return StatusCode(500, "Erro ao enviar e-mail.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro interno: {ex.Message}");
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        private async Task SendEmail(ContactModel contactForm)
        {
            using (var mailMessage = new MailMessage())
            using (var smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpServer"]))
            {
                mailMessage.From = new MailAddress(_configuration["EmailSettings:SenderEmail"]);
                mailMessage.Subject = "New contact form";
                mailMessage.To.Add(contactForm.Email);
                mailMessage.Body = $"FullName: {contactForm.FullName}\nEmail: {contactForm.Email}\nMessage: {contactForm.Message}";

                smtpClient.Credentials = new NetworkCredential(
                    _configuration["EmailSettings:SenderEmail"],
                    _configuration["EmailSettings:SenderPassword"]
                );
                smtpClient.EnableSsl = true;
                smtpClient.Port = int.Parse(_configuration["EmailSettings:SmtpPort"]);

                await smtpClient.SendMailAsync(mailMessage).ConfigureAwait(false);
            }
        }
    }
}

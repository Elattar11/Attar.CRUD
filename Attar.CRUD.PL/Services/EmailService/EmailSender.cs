using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Attar.CRUD.PL.Services.EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _conf;

        public EmailSender(IConfiguration conf)
        {
            _conf = conf;
        }
        public async Task SendAsync(string from, string recipients, string subject, string body)
        {
            var senderEmail = _conf["EmailSettings:senderEmail"];
            var senderPassword = _conf["EmailSettings:senderPassword"];

            var emailMessage = new MailMessage();

            emailMessage.From = new MailAddress(from);
            emailMessage.To.Add(recipients);
            emailMessage.Subject = subject;
            emailMessage.Body = body;

            var smtpClient = new SmtpClient(_conf["EmailSettings:SmtpClientServer"], int.Parse(_conf["EmailSettings:SmtpClientPort"]))
            {
                Credentials = new NetworkCredential(senderEmail, senderPassword), 
                EnableSsl = true
            };

            await smtpClient.SendMailAsync(emailMessage);
        }
    }
}

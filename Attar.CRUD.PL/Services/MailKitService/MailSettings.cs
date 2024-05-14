using Attar.CRUD.DAL.Entities;
using Attar.CRUD.PL.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Security;
using System.Threading.Tasks;

namespace Attar.CRUD.PL.Services.MailKitService
{
    public class MailSettings : IMailSettings
    {
        private  MailKitSettings _options;

        public MailSettings(IOptions<MailKitSettings> options)
        {
            _options = options.Value;
        }
        public void SendMail(Email email)
        {
            var mail = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_options.Email),
                Subject = email.Subject
            };

            mail.To.Add(MailboxAddress.Parse(email.To));
            mail.From.Add(new MailboxAddress(_options.DisplayName, _options.Email)); 

            var builder = new BodyBuilder();

            builder.TextBody = email.Body;

            mail.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            smtp.Connect(_options.Host, _options.Port , SecureSocketOptions.StartTls);

            smtp.Authenticate(_options.Email, _options.Password);

            smtp.Send(mail);

            smtp.Disconnect(true);
        }
    }
}

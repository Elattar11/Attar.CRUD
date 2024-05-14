using System.Threading.Tasks;

namespace Attar.CRUD.PL.Services.EmailService
{
    public interface IEmailSender
    {
        Task SendAsync(string from, string recipients, string subject, string body);
    }
}

using Attar.CRUD.DAL.Entities;
using System.Threading.Tasks;

namespace Attar.CRUD.PL.Services.MailKitService
{
    public interface IMailSettings
    {
        public void SendMail(Email email);
    }
}

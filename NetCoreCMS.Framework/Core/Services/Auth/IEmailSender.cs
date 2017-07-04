using System.Threading.Tasks;

namespace NetCoreCMS.Framework.Core.Services.Auth
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}

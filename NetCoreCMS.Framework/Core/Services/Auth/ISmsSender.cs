using System.Threading.Tasks;

namespace NetCoreCMS.Framework.Core.Services.Auth
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}

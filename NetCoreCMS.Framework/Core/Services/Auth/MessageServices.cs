/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Messages;
using NetCoreCMS.Framework.Core.Network;
using NetCoreCMS.Framework.Utility;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using MimeKit.Text;
using System.Net;

namespace NetCoreCMS.Framework.Core.Services.Auth
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        ILogger _logger;
        NccSettingsService _nccSettingsService;

        public AuthMessageSender(NccSettingsService settingsService, ILoggerFactory factory)
        {
            _nccSettingsService = settingsService;
            _logger = factory.CreateLogger<AuthMessageSender>();
        }
         
        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {

            var smtpSettings = _nccSettingsService.GetByKey<SmtpSettings>();
            if (smtpSettings == null)
            {
                _logger.LogError("SMTP Settings not found");
                GlobalMessageRegistry.RegisterMessage(
                    new GlobalMessage()
                    {
                        For = GlobalMessage.MessageFor.Both,
                        Registrater = nameof(AuthMessageSender),
                        Type = GlobalMessage.MessageType.Error,
                        Text = "Email Send Failed. SMTP Settings not set yet."
                    },
                    new System.TimeSpan(0, 1, 0)
                );
            }
            else
            {
                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress(smtpSettings.FromName, smtpSettings.FromEmail));
                emailMessage.To.Add(new MailboxAddress("User", email));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart(TextFormat.Html) { Text = message };

                using (var client = new SmtpClient())
                {
                    client.LocalDomain = smtpSettings.Host;                    
                    client.ConnectAsync(smtpSettings.Host, smtpSettings.Port, smtpSettings.UseSSL).Wait();
                    client.Authenticate(new NetworkCredential(smtpSettings.UserName, smtpSettings.Password));
                    client.SendAsync(emailMessage).Wait();
                    client.DisconnectAsync(true).Wait();
                }
            }

            return Task.FromResult(0); 
        }

        public Task SendEmailConfirmationAsync(string email, string callbackUrl)
        {
            SendEmailAsync(email, "NetCoreCMS new signup", "Thank you for your registration. Please confirm your registration <a href='" + callbackUrl + "'>Verify</a>");
            return Task.FromResult(0);
        }
    }
}

/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/
// For more details see https://go.microsoft.com/fwlink/?LinkID=532713
using NetCoreCMS.Framework.Core.Services.Auth;
using System;
using System.Threading.Tasks;

namespace NetCoreCMS.Web.Services { 
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }

        public Task SendEmailConfirmationAsync(string email, string callbackUrl)
        {
            throw new NotImplementedException();
        }
    }
}

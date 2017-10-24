/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/
 
using MediatR;
using NetCoreCMS.Framework.Core.Events.App;
using NetCoreCMS.Framework.Core.Events.Themes;
using NetCoreCMS.Framework.Core.Messages;

namespace NetCoreCMS.Framework.Core.Hooks
{
    public class OnAppActivityHandler : IRequestHandler<OnAppActivity, AppActivity>
    {    
        public AppActivity Handle(OnAppActivity message)
        {
            GlobalMessageRegistry.RegisterMessage(new GlobalMessage() {
                For = GlobalMessage.MessageFor.Both,
                Registrater = "CoreModule",
                Text = message.Data.ActivityType.ToString(),
                Type = GlobalMessage.MessageType.Info
            }, new System.TimeSpan(0,1,0));
            return message.Data;
        }     
    }
}

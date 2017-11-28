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
using NetCoreCMS.Framework.Core.Messages;

namespace NetCoreCMS.Framework.Core.Hooks
{
    public class OnAppActivityHandler : IRequestHandler<OnAppActivity, AppActivity>
    {    
        public AppActivity Handle(OnAppActivity message)
        {
            return message.Data;
        }     
    }
}

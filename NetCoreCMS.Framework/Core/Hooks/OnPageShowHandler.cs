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
using NetCoreCMS.Framework.Core.Events.Page;
using NetCoreCMS.Framework.Core.Models;

namespace NetCoreCMS.Framework.Core.Hooks
{
    public class OnPageShowHandler : IRequestHandler<OnPageShow, NccPage>
    {
        public NccPage Handle(OnPageShow message)
        {
            var post = message.Page;            
            return post;
        }
    }
}

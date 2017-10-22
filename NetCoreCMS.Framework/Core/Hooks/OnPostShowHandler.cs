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
using NetCoreCMS.Framework.Core.Events.Post;
using NetCoreCMS.Framework.Core.Models;
 

namespace NetCoreCMS.Framework.Core.Hooks
{
    public class OnPostShowHandler : IRequestHandler<OnPostShow, NccPost>
    {
        public NccPost Handle(OnPostShow message)
        {
            var post = message.Post; 
            return post;
        }
    }
}

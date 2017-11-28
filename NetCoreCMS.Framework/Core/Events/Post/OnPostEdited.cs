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
using NetCoreCMS.Framework.Core.Models;

namespace NetCoreCMS.Framework.Core.Events.Post
{
    public class OnPostEdited : IRequest<NccPost>
    {
        public NccPost OldPost { get; set; }
        public NccPost NewPost { get; set; }
        public OnPostEdited(NccPost oldPost, NccPost newPost)
        {
            OldPost = oldPost;
            NewPost = newPost;
        }
    }
}

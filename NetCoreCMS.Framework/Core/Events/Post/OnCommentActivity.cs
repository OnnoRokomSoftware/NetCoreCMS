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

namespace NetCoreCMS.Framework.Core.Events.Post
{
    public class OnCommentActivity : IRequest<CommentActivity>
    {
        public CommentActivity Data { get; set; }
        public OnCommentActivity(CommentActivity data)
        {
            Data = Data;
        }
    }
}

/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/
 
using NetCoreCMS.Framework.Core.Models;
namespace NetCoreCMS.Framework.Core.Events.Post
{
    public class CommentActivity
    {
        public NccPost Post{ get; set; }
        public NccComment Comment { get; set; }
        public NccUser Author { get; set; }
        public Type ActionType { get; set; }

        public enum Type
        {
            NewComment,
            CommentApproved,
            CommentRejected,
            CommentDeleted
        }
    }
}

/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;
using NetCoreCMS.Framework.Core.Mvc.Models;

namespace NetCoreCMS.Framework.Core.Models
{
    [Serializable]
    public class NccComment : BaseModel<long>
    { 
        public string Title { get; set; }
        public string Content { get; set; }

        public NccPost Post { get; set; }
        public NccUser Author { get; set; }

        public string AuthorName { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public NccCommentStatus CommentStatus { get; set; }

        public enum NccCommentStatus
        {
            Pending,
            Approved,
            Rejected,
            Spam
        }
    } 
}

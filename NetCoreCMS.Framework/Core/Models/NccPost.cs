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
using System.Collections.Generic;
using NetCoreCMS.Framework.Core.Mvc.Models;

namespace NetCoreCMS.Framework.Core.Models
{
    [Serializable]
    public class NccPost : BaseModel<long>
    {
        public NccPost()
        { 
            Categories = new List<NccPostCategory>();
            Tags = new List<NccPostTag>();
            Comments = new List<NccComment>();
            PostDetails = new List<NccPostDetails>();
        }
                
        public bool IsFeatured { get; set; }
        public bool IsStiky { get; set; }
        public bool AllowComment { get; set; }
        public string ThumImage { get; set; }
        public string Layout { get; set; }
        public string RelatedPosts { get; set; }
        public DateTime PublishDate { get; set; }
        public long CommentCount { get; set; }

        public NccPost Parent { get; set; }
        public NccUser Author { get; set; }
        public NccPostStatus PostStatus { get; set; }
        public NccPostType PostType { get; set; }

        public List<NccPostDetails> PostDetails { get; set; }
        public List<NccPostCategory> Categories { get; set; }
        public List<NccPostTag> Tags { get; set; }
        public List<NccComment> Comments { get; set; }
        
        public enum NccPostStatus
        {
            Draft,
            Reviewed,
            Published,
            UnPublished,
            Archived
        }
        public enum NccPostType
        {
            Public,
            Private,
            PasswordProtected
        }

    }
}

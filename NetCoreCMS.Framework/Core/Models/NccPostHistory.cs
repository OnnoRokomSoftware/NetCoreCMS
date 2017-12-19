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
using static NetCoreCMS.Framework.Core.Models.NccPost;

namespace NetCoreCMS.Framework.Core.Models
{
    [Serializable]
    public class NccPostHistory : BaseModel<long>
    {
        public NccPostHistory()
        { 
            
        }
                
        public bool IsFeatured { get; set; }
        public bool IsStiky { get; set; }
        public bool AllowComment { get; set; }
        public string ThumImage { get; set; }
        public string Layout { get; set; }
        public string RelatedPosts { get; set; }
        public DateTime PublishDate { get; set; }
        public long CommentCount { get; set; }

        public long PostId { get; set; }

        public NccPost Parent { get; set; }
        public NccUser Author { get; set; }
        public NccPostStatus PostStatus { get; set; }
        public NccPostType PostType { get; set; }
        
        public List<NccPostDetailsHistory> PostDetailsHistory { get; set; }
    }
}

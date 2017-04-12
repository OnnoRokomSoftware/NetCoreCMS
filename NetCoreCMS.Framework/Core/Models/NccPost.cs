/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
using NetCoreCMS.Framework.Core.Mvc.Models;
using System.Collections.Generic;

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccPost : BaseModel
    {
        public NccPost Parent { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeyword { get; set; }
        public byte[] Content { get; set; }
        public string ThumImage { get; set; }
        public NccUser Author { get; set; }
        public List<NccPostCategory> Categories { get; set; }
        public List<NccTag> Tags { get; set; }
        public List<NccPostComment> PostComments { get; set; }
    }
}

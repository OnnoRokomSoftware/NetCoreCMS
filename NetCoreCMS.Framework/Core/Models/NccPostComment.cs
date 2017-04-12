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
    public class NccPostComment : BaseModel
    {
        public NccPost Post { get; set; }
        public string Title { get; set; }
        public byte[] Content { get; set; }
        public NccUser Author { get; set; }
        
    }
}

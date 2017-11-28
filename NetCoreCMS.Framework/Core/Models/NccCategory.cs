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
    public class NccCategory : BaseModel<long>
    { 
        public NccCategory()
        {
            CategoryDetails = new List<NccCategoryDetails>();
            Posts = new List<NccPostCategory>();
        }

        public string CategoryImage { get; set; }

        public NccCategory Parent { get; set; }
        public List<NccCategoryDetails> CategoryDetails { get; set; }
        public List<NccPostCategory> Posts { get; set; }
    }
}

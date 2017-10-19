/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System.Collections.Generic;
using NetCoreCMS.Framework.Core.Mvc.Models;

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccCategory : BaseModel, IBaseModel<long>
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

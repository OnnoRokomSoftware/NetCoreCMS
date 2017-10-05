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

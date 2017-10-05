using System.Collections.Generic;
using NetCoreCMS.Framework.Core.Mvc.Models;

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccTag : BaseModel, IBaseModel<long>
    { 
        public NccTag()
        {
            Posts = new List<NccPostTag>();
        }

        public List<NccPostTag> Posts { get; set; }
    }
}

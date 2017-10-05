using NetCoreCMS.Framework.Core.Mvc.Models;

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccComment : BaseModel, IBaseModel<long>
    { 
        public string Title { get; set; }
        public string Content { get; set; }

        public NccPost Post { get; set; }
        public NccUser Author { get; set; } 
    }
}

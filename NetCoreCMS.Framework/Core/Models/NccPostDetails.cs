using NetCoreCMS.Framework.Core.Mvc.Models;

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccPostDetails : BaseModel, IBaseModel<long>
    { 
        public string Title { get; set; }
        public string Slug { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeyword { get; set; }
        public string Content { get; set; }
        public string Language { get; set; }

        public NccPost Post { get; set; }
    }
}

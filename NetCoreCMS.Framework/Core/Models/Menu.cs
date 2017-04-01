using NetCoreCMS.Framework.Core.Mvc.Models;

namespace NetCoreCMS.Framework.Core.Models
{
    public class Menu : BaseModel
    {
        public Menu ParentMenu { get; set; }
        public string Data { get; set; }
        public string Target { get; set; }
        public int Position { get; set; }
        public ActionType ActionType { get; set; }        
    }

    public enum ActionType
    {
        Url,
        Page,
        Blog,
        BlogPost,
        PostCategory,
        Module
    }
}

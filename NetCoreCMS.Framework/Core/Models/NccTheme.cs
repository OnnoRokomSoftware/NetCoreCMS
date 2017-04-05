using NetCoreCMS.Framework.Core.Mvc.Models;

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccTheme : BaseModel
    {   
        public string ThemeName { get; set; }
        public string Type { get; set; }
        public string Author { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string NetCoreCMSVersion { get; set; }
        public string Category { get; set; }   
    }
}

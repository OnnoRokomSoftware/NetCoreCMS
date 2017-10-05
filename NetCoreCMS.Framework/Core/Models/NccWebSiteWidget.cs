using NetCoreCMS.Framework.Core.Mvc.Models;

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccWebSiteWidget : BaseModel, IBaseModel<long>
    {   
        public string ThemeId { get; set; }
        public string ModuleId { get; set; }
        public string LayoutName { get; set; }
        public string Zone { get; set; }
        public string WidgetId { get; set; }
        public int WidgetOrder { get; set; }
        public string WidgetConfigJson { get; set; }
        public string WidgetData { get; set; }

        public NccWebSite WebSite { get; set; }
    }

    public enum WebSiteWidgetStatus
    {
        Active,
        Inactive,
        Installed,
        Uninstalled
    }
}

using NetCoreCMS.Framework.Core.Mvc.Models;

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccModuleDependency : BaseModel, IBaseModel<long>
    {
        public string ModuleId { get; set; }
        public string MinVersion { get; set; }
        public string MaxVersion { get; set; }
    }
}

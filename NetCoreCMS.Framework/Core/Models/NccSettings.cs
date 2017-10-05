using NetCoreCMS.Framework.Core.Mvc.Models;
using System.Reflection;

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccSettings : BaseModel, IBaseModel<long>
    {
        public NccSettings()
        {
            var assembly = Assembly.GetCallingAssembly();
            GroupId = assembly.FullName;
        }
        public string GroupId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; } 
    }
}

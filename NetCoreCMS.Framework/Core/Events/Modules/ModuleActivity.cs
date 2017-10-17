using NetCoreCMS.Framework.Modules;

namespace NetCoreCMS.Framework.Core.Events.Modules
{
    public class ModuleActivity
    {
        public Module Module{ get; set; }
        public Type ActivityType { get; set; }

        public enum Type
        {
            Downloaded,
            Installed,
            Activated,
            Deactivated,
            Uninstalled            
        }
    }
}

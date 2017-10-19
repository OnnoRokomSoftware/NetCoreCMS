/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/
 
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

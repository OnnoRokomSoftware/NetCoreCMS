using NetCoreCMS.Framework.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Modules.Cms.Lib
{
    public class ModuleHelper
    {
        public static string GetActionText(NccModule module)
        {
            var actionText = "";

            if (module.ModuleStatus == NccModule.NccModuleStatus.New || module.ModuleStatus == NccModule.NccModuleStatus.UnInstalled)
            {
                actionText = "<a href=\"/CmsModule/InstallModule/?id=" + module.ModuleId + "\" >Install</a>";
            }
            else if(module.ModuleStatus == NccModule.NccModuleStatus.Installed || module.ModuleStatus == NccModule.NccModuleStatus.Inactive)
            {
                actionText = "<a href=\"/CmsModule/ActiveateModule/?id=" + module.ModuleId + "\">Activate</a> | ";
                actionText += "<a href=\"/CmsModule/UnInstallModule/?id=" + module.ModuleId + "\">Uninstall</a>";
            }            
            else if (module.ModuleStatus == NccModule.NccModuleStatus.Active)
            {
                actionText = "<a href=\"/CmsModule/InactivateModule/?id=" + module.ModuleId + "\">Inactivate</a>";
            }
            else if(module.ModuleStatus == NccModule.NccModuleStatus.Deleted || module.ModuleStatus == NccModule.NccModuleStatus.Duplicate || module.ModuleStatus == NccModule.NccModuleStatus.InCompatible)
            {
                actionText = "<a href=\"/CmsModule/RemoveModule/?id=" + module.ModuleId + "\">Remove</a>";
            }
            return actionText;
        }
    }
}

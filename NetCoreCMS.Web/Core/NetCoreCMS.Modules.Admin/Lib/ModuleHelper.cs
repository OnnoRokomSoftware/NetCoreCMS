/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCoreCMS.Modules.Admin.Lib
{
    public class ModuleHelper
    {
        public static string GetActionText(NccModule module)
        {
            var actionText = "";
            if (!IsCoreModule(module))
            {
                if (module.ModuleStatus == NccModule.NccModuleStatus.New || module.ModuleStatus == NccModule.NccModuleStatus.UnInstalled)
                {
                    actionText = "<a href=\"/CmsModule/InstallModule/?id=" + module.Id + "\" >Install | </a>";
                    actionText += "<a href=\"/CmsModule/RemoveModule/?id=" + module.Id + "\">Remove</a>";
                }
                else if (module.ModuleStatus == NccModule.NccModuleStatus.Installed || module.ModuleStatus == NccModule.NccModuleStatus.Inactive)
                {
                    actionText = "<a href=\"/CmsModule/ActivateModule/?id=" + module.Id + "\">Activate</a> | ";
                    actionText += "<a href=\"/CmsModule/UnInstallModule/?id=" + module.Id + "\">Uninstall</a>";
                }
                else if (module.ModuleStatus == NccModule.NccModuleStatus.Active)
                {
                    actionText = "<a href=\"/CmsModule/DeactivateModule/?id=" + module.Id + "\">Deactivate</a>";
                }
                else if (module.ModuleStatus == NccModule.NccModuleStatus.Deleted || module.ModuleStatus == NccModule.NccModuleStatus.Duplicate || module.ModuleStatus == NccModule.NccModuleStatus.InCompatible)
                {
                    actionText = "<a href=\"/CmsModule/RemoveModule/?id=" + module.Id + "\">Remove</a>";
                }
            }
            else
            {
                actionText = "Pre Activated";
            }

            return actionText;
        }

        public static bool IsCoreModule(NccModule module)
        {
            var pathParts = module.Path.Split("\\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var hasCore = pathParts.Where(x => x.Equals("Core", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            return !string.IsNullOrEmpty(hasCore);
        }
    }
}

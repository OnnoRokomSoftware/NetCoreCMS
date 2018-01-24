/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.AspNetCore.Mvc.Razor;
using NetCoreCMS.Framework.Themes;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreCMS.Framework.Modules
{
    public class ModuleViewLocationExpendar : IViewLocationExpander
    {
        private const string _moduleKey = "module";
                
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (context.Values.ContainsKey(_moduleKey))
            {
                var module = context.Values[_moduleKey];
                if (!string.IsNullOrWhiteSpace(module))
                {
                    var moduleViewLocations = new string[]
                    {
                        "/Themes/ChildThemes/"+ ThemeHelper.ActiveTheme.ThemeId +"/" + module + "/Views/{0}.cshtml",
                        "/Themes/ChildThemes/"+ ThemeHelper.ActiveTheme.ThemeId +"/" + module + "/Views/{1}/{0}.cshtml",

                        "/Themes/"+ ThemeHelper.ActiveTheme.ThemeId +"/Views/{1}/{0}.cshtml",
                        "/Themes/"+ ThemeHelper.ActiveTheme.ThemeId +"/Views/{0}.cshtml",
                        "/Themes/"+ ThemeHelper.ActiveTheme.ThemeId +"/Views/" + module + "/{0}.cshtml",
                        "/Themes/"+ ThemeHelper.ActiveTheme.ThemeId +"/Views/" + module + "/{1}/{0}.cshtml", 
                        "/Themes/"+ ThemeHelper.ActiveTheme.ThemeId +"/Shared/{0}.cshtml",
                        "/Themes/"+ ThemeHelper.ActiveTheme.ThemeId +"/Shared/Layouts/{0}.cshtml",
                        
                        "/Modules/" + module + "/Views/{1}/{0}.cshtml",
                        "/Modules/" + module + "/Views/Shared/{0}.cshtml",
                        "/Modules/" + module + "/Views/{0}.cshtml",

                        "/Core/" + module + "/Views/{1}/{0}.cshtml",
                        "/Core/" + module + "/Views/Shared/{0}.cshtml",
                        "/Core/" + module + "/Views/{0}.cshtml",

                        "/Views/{1}/{0}.cshtml",
                        "/Views/Shared/{0}.cshtml"
                    };

                    viewLocations = moduleViewLocations.Concat(viewLocations);
                }
            }
            return viewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            dynamic controller = context.ActionContext.ActionDescriptor;
            
            if (controller != null)
            {
                var controllerTypeInfo = controller.ControllerTypeInfo;
                string moduleName = controllerTypeInfo.Module.Name;
                var area = 
                moduleName = moduleName.Remove(moduleName.Length - 4);
                if (moduleName != "NetCoreCMS.Web")
                {
                    context.Values[_moduleKey] = moduleName;
                }
            }
        }
    }
}

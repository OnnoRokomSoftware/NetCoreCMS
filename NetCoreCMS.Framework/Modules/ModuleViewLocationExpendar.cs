/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
using Microsoft.AspNetCore.Mvc.Razor;
using NetCoreCMS.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                    "/Themes/"+ GlobalConfig.ActiveTheme.ThemeName +"/Views/{1}/{0}.cshtml",
                    "/Themes/"+ GlobalConfig.ActiveTheme.ThemeName +"/Shared/{0}.cshtml",
                    "/Core/" + module + "/Views/{1}/{0}.cshtml",
                    "/Core/" + module + "/Views/Shared/{0}.cshtml",
                    "/Modules/" + module + "/Views/{1}/{0}.cshtml",
                    "/Modules/" + module + "/Views/Shared/{0}.cshtml",
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
            string moduleName = controller.ControllerTypeInfo.Module.Name;
            moduleName = moduleName.Remove(moduleName.Length-4);
            if (moduleName != "NetCoreCMS.Web")
            {
                context.Values[_moduleKey] = moduleName;
            }
        }
    }
}

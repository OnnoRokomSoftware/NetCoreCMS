/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;
using System.Collections.Generic;
using System.Reflection;
using NetCoreCMS.Framework.Modules;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules.Widgets;
using Microsoft.AspNetCore.Routing;
using NetCoreCMS.Framework.Core.Data;
using Microsoft.Extensions.DependencyInjection;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Models.ViewModels;

namespace NetCoreCMS.Core.Modules.Admin
{
    
    public class AdminModule : IModule
    {
        List<Widget> _widgets;
        public AdminModule()
        {
             
        }
        
        public string ModuleId { get; set; }
        public bool IsCore { get; set; }
        public string ModuleTitle { get; set; }
        public string Author { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string DemoUrl { get; set; }
        public string ManualUrl { get; set; }
        public bool AntiForgery { get; set; }
        public string Version { get; set; }
        public string MinNccVersion { get; set; }
        public string MaxNccVersion { get; set; }
        public string Description { get; set; }
        public string DependencyList { get; set; }        
        public string Category { get; set; }
        public List<NccModuleDependency> Dependencies { get; set; }
        public Assembly Assembly { get; set; }
        public string SortName { get; set; }
        public string Path { get; set; }
        public string Folder { get; set; }
        public int ModuleStatus { get; set; }
        public List<Widget> Widgets { get { return _widgets; } set { _widgets = value; } }
        public List<ModuleController> Controllers { get; set; }
        public bool Activate()
        {
            return true;
        }

        public bool Inactivate()
        {
            return true;
        }

        public void Init(IServiceCollection services)
        {
            
        }

        public void RegisterRoute(IRouteBuilder routes)
        {
            
        }

        public bool Install(NccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery)
        {
            return true;
        }

        public bool Uninstall(NccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery)
        {
            return true;
        }

        public bool Update(NccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery)
        {
            return true;
        }
    }
}

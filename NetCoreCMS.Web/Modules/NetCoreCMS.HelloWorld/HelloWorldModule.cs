using Microsoft.Extensions.DependencyInjection;
using NetCoreCMS.Framework.Modules;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.HelloWorld.Widgets;
using Microsoft.AspNetCore.Routing;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Core.Data;

namespace NetCoreCMS.Modules.HelloWorld
{
    public class HelloWorldModule : IModule
    {
        List<IWidget> _widgets;
        public HelloWorldModule()
        {
             
        }

        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string SortName { get; set; }
        public bool AntiForgery { get; set ; }
        public string Author { get ; set ; }
        public string Website { get ; set ; }
        public string Version { get ; set ; }
        public string NetCoreCMSVersion { get ; set ; }
        public string Description { get ; set ; }

        public List<string> Dependencies { get ; set ; }
        public string Category { get ; set ; }

        [NotMapped]
        public Assembly Assembly { get; set; }
        public string Path { get ; set ; }
        public int ModuleStatus { get; set; }
        public string ModuleTitle { get ; set ; }
        [NotMapped]
        public List<IWidget> Widgets { get { return _widgets; } set { _widgets = value; } }

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
    }
}

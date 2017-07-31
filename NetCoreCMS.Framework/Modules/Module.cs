using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Modules.Widgets;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Routing;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Core.Data;

namespace NetCoreCMS.Framework.Modules
{
    public class Module : IModule
    {
        List<Widget> _widgets;
        
        public string Id { get; set; }
        public string ModuleId { get; set; }
        public string ModuleTitle { get; set; }
        public string ModuleName { get; set; }
        public bool AntiForgery { get; set; }
        public string Author { get; set; }
        public string Website { get; set; }
        public string Version { get; set; }
        public string NetCoreCMSVersion { get; set; }
        public string Description { get; set; }
        public string DependencyList { get; set; }
        [NotMapped]
        public List<string> Dependencies
        {
            get
            {
                return DependencyList.Split(',').ToList();
            }
            set
            {
                DependencyList = value.ToArray().Join(",");
            }
        }

        public string Category { get; set; }
        public Assembly Assembly { get; set; }
        public string SortName { get; set; }
        public string Path { get; set; }
        public int ModuleStatus { get; set; }

        public List<Widget> Widgets { get { return _widgets; } set { _widgets = value; } }
        
        public Module()
        {
            _widgets = new List<Widget>();
            DependencyList = "";
        }

        public bool Activate()
        {
            throw new NotImplementedException();
        }

        public bool Inactivate()
        {
            throw new NotImplementedException();
        }

        public void Init(IServiceCollection services)
        {
            //Initilize the module here
        }
  
        public void RegisterRoute(IRouteBuilder routes)
        {
            throw new NotImplementedException();
        }

        public bool Install(NccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery)
        {
            throw new NotImplementedException();
        }

        public bool Uninstall(NccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery)
        {
            throw new NotImplementedException();
        }
    }
}

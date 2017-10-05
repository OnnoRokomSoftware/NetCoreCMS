using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NetCoreCMS.Framework.Core.Models;

namespace NetCoreCMS.Framework.Modules
{
    public class Plugins : IPlugins
    {
        public string PluginsId { get; set; }
        public string PluginsTitle { get; set; }        
        public bool AntiForgery { get; set; }
        public string Author { get; set; }
        public string Website { get; set; }
        public Version Version { get; set; }
        public Version NetCoreCMSVersion { get; set; }
        public string Description { get; set; }
        public List<string> Dependencies { get; set; }
        public string Category { get; set; }
        public Assembly Assembly { get; set; }
        public string SortName { get; set; }
        public string Path { get; set; }
        public NccPlugins.NccPluginsStatus Status { get; set; }
        

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

        public bool Install()
        {
            throw new NotImplementedException();
        }

        public void LoadPluginsInfo()
        {
            throw new NotImplementedException();
        }

        public bool Uninstall()
        {
            throw new NotImplementedException();
        }
    }
}

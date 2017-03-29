using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using NetCoreCMS.Framework.Modules;

namespace NetCoreCMS.Modules.Cms
{
    public class CmsModule : IModule
    {
        public CmsModule()
        {
            ModuleName = "NetCoreCMS.Modules.Cms";
            Author = "Xonaki";
            Website = "http://xonaki.com";
            AntiForgery = true;
            Description = "Builtin Content Management System Module.";
            Version = new Version(0, 1, 1);
            NetCoreCMSVersion = new Version(0, 1, 1);
            Dependencies = new List<string>();
            Category = "Core";
        }

        public string Id { get; set; }
        public string ModuleName { get; set; }
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

        public void Init(IServiceCollection services)
        {
            
        }
    }
}

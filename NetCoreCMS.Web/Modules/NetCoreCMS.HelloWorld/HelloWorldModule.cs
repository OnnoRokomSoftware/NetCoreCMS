using Microsoft.Extensions.DependencyInjection;
using NetCoreCMS.Framework.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace NetCoreCMS.Modules.HelloWorld
{
    public class HelloWorldModule : IModule
    {
        public HelloWorldModule()
        {
            ModuleName = "NetCoreCMS.Modules.HelloWorld";
            Author = "Xonaki";
            Website = "http://xonaki.com";
            AntiForgery = true;
            Description = "Builtin Content Management Example Module.";
            Version = new Version(0, 1, 1);
            NetCoreCMSVersion = new Version(0, 1, 1);
            Dependencies = new List<string>();
            Category = new List<string>();
        }

        public string ModuleName { get; set; }
        public bool AntiForgery { get; set ; }
        public string Author { get ; set ; }
        public string Website { get ; set ; }
        public Version Version { get ; set ; }
        public Version NetCoreCMSVersion { get ; set ; }
        public string Description { get ; set ; }
        public List<string> Dependencies { get ; set ; }
        public List<string> Category { get ; set ; }
        public Assembly Assembly { get ; set ; }
        public string SortName { get ; set ; }
        public string Path { get ; set ; }

        public void Init(IServiceCollection services)
        {
            //services.AddTransient<IAnotherTestService, AnotherTestService>();
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NetCoreCMS.Framework.Modules
{
    public interface IModule
    {
        string Id { get; set; }
        string ModuleName { get; set; }        
        bool AntiForgery { get; set; }
        string Author { get; set; }
        string Website { get; set; } 
        Version Version { get; set; }
        Version NetCoreCMSVersion { get; set; }
        string Description { get; set; }
        List<string> Dependencies { get; set; }
        string Category { get; set; }
        Assembly Assembly { get; set; }
        string SortName { get; set; }
        string Path { get; set; }
        void Init(IServiceCollection services);
    }
}

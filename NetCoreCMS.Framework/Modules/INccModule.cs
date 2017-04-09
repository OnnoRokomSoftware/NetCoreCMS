/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NetCoreCMS.Framework.Core.Models;


namespace NetCoreCMS.Framework.Modules
{
    public interface INccModule
    {
        string ModuleId { get; set; }
        string ModuleName { get; set; }
        string ModuleTitle { get; set; }
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
        ModuleStatus Status { get; set; }

        void Init(IServiceCollection services);
        bool Install();
        bool Uninstall();
        bool Activate();
        bool Inactivate();
        void LoadModuleInfo();
    }
}

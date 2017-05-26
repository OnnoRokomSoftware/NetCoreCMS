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
using NetCoreCMS.Framework.Modules.Widgets;

namespace NetCoreCMS.Framework.Modules
{
    public interface IModule
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
        List<IWidget> Widgets { get; set; }
        NccModule.NccModuleStatus Status { get; set; }

        void Init(IServiceCollection services);
        bool Install();
        bool Uninstall();
        bool Activate();
        bool Inactivate();
    }
}

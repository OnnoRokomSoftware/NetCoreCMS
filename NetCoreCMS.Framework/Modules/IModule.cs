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
using Microsoft.Extensions.DependencyInjection;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.Framework.Core.Services;
using Microsoft.AspNetCore.Routing;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Models.ViewModels;

namespace NetCoreCMS.Framework.Modules
{
    public delegate void CreateOrUpdateTable<ModelT>();

    public interface IModule
    {
        /// <summary>
        /// Lower value has heigher precedence. Default value is 100 if value does not provide at module.json file.
        /// </summary>
        int ExecutionOrder { get; set; }
        /// <summary>
        /// Unique Module ID
        /// </summary>        
        bool IsCore { get; set; }
        /// <summary>
        /// Description: Module name must be same as module folder name.
        /// </summary>
        string ModuleName { get; set; }
        /// <summary>
        /// Module title for display
        /// </summary>
        string ModuleTitle { get; set; }        
        string Author { get; set; }
        string Email { get; set; }        
        string Website { get; set; }
        string DemoUrl { get; set; }
        string ManualUrl { get; set; }
        bool AntiForgery { get; set; }
        string Description { get; set; }
        string Version { get; set; }
        string NccVersion { get; set; }        
        string Category { get; set; }
        List<NccModuleDependency> Dependencies { get; set; }        
        Assembly Assembly { get; set; }
        string SortName { get; set; }
        string Path { get; set; }
        string Folder { get; set; }
        string TablePrefix { get; set; }
        int ModuleStatus { get; set; }
        List<Widget> Widgets { get; set; }
        List<Menu> Menus { get; set; }
        string Area { get;}

        void Init(IServiceCollection services, INccSettingsService nccSettingsService);
        void RegisterRoute(IRouteBuilder routes);
        
        bool Install(INccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery, Func<Type, int> createUpdateTable);
        bool Uninstall(INccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery, Func<Type, int> deleteTable);
        bool Update(INccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery, Func<Type, int> createUpdateTable);
        bool Activate();
        bool Inactivate();
    }
}

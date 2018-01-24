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
using Microsoft.AspNetCore.Routing;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using Newtonsoft.Json;
using NetCoreCMS.Framework.Core.Models.ViewModels;
using NetCoreCMS.Framework.Core.Mvc.Models;

namespace NetCoreCMS.Framework.Modules
{
    public class Module : BaseModule, IModule
    {
                
    }

    public abstract class BaseModule : IModule
    {
        List<Widget> _widgets;

        public virtual int ExecutionOrder { get; set; }
        public string Id { get; set; }
        public string ModuleName { get; set; }
        public bool IsCore { get; set; }
        public string ModuleTitle { get; set; }
        public string Author { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string DemoUrl { get; set; }
        public bool AntiForgery { get; set; }
        public string Version { get; set; }
        public string NccVersion { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public List<NccModuleDependency> Dependencies { get; set; }
        public Assembly Assembly { get; set; }
        public string SortName { get; set; }
        public string Path { get; set; }
        public string Folder { get; set; }
        public string TablePrefix { get; set; }
        public string AssemblyPath { get; set; }
        public int ModuleStatus { get; set; }
        public List<Widget> Widgets { get { return _widgets; } set { _widgets = value; } }
        public List<Menu> Menus { get; set; }
        public virtual string Area { get { return ""; } }

        public virtual bool IsMultilangual { get { return true; } }
        public virtual List<SupportedDatabases> Databases
        {
            get
            {
                return new List<SupportedDatabases>() {
                    SupportedDatabases.InMemory,
                    SupportedDatabases.MSSQL,
                    SupportedDatabases.MsSqlLocalStorage,
                    SupportedDatabases.MySql,
                    SupportedDatabases.PgSql,
                    SupportedDatabases.SqLite };
            }
        }

        public BaseModule()
        {
            Menus = new List<Menu>();
            _widgets = new List<Widget>();
        }

        public virtual bool Activate()
        {
            return true;
        }

        public virtual bool Inactivate()
        {
            return true;
        }

        public virtual void Init(IServiceCollection services, INccSettingsService nccSettingsService)
        {
            //Initilize the module here
        }

        public virtual void RegisterRoute(IRouteBuilder routes)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingsService"></param>
        /// <param name="executeQuery"></param>
        /// <param name="createUpdateTable">arg1=typeof(model), arg2=DeleteUnusedColumns</param>
        /// <returns></returns>
        public virtual bool Install(INccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery, Func<Type, bool, int> createUpdateTable)
        {
            return true;
        }

        public virtual bool Update(INccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery, Func<Type, bool, int> createUpdateTable)
        {
            return true;
        }

        public virtual bool RemoveTables(INccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery, Func<Type, int> deleteTable)
        {
            return true;
        }

        public virtual bool Uninstall(INccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery)
        {
            return true;
        }

    }
}

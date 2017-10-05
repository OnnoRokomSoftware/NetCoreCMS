/*
 * Author: OnnoRokom Software Ltd
 * Website: http://onnorokomsoftware.com
 * Copyright (c) onnorokomsoftware.com
 * License: Commercial
*/
using Microsoft.Extensions.DependencyInjection;
using NetCoreCMS.Framework.Modules;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.UmsBdResult.Repository;
using NetCoreCMS.UmsBdResult.Services;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Core.Data;
using Microsoft.AspNetCore.Routing;
using NetCoreCMS.Framework.Core.Models;

namespace NetCoreCMS.UmsBdResult
{
    public class UmsbdResultModule : IModule
    {
        List<Widget> _widgets;
        public UmsbdResultModule()
        {

        }

        public string ModuleId { get; set; }
        public string ModuleTitle { get; set; }
        public string Author { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string DemoUrl { get; set; }
        public string ManualUrl { get; set; }
        public bool AntiForgery { get; set; }
        public string Version { get; set; }
        public string MinNccVersion { get; set; }
        public string MaxNccVersion { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public List<NccModuleDependency> Dependencies { get; set; }

        [NotMapped]
        public Assembly Assembly { get; set; }
        public string Path { get; set; }
        public string Folder { get; set; }
        public int ModuleStatus { get; set; }
        public string SortName { get; set; }
        [NotMapped]
        public List<Widget> Widgets { get { return _widgets; } set { _widgets = value; } }

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
            services.AddTransient<UmsBdResultRepository>();
            services.AddTransient<UmsBdResultService>();
        }

        public void RegisterRoute(IRouteBuilder routes)
        {

        }

        public bool Install(NccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery)
        {
            //var createQuery = @"
            //";

            //var nccDbQueryText = new NccDbQueryText() { MySql_QueryText = createQuery };
            //var retVal = executeQuery(nccDbQueryText);
            //if (!string.IsNullOrEmpty(retVal))
            //{
            //    return true;
            //}
            //return false;
            return true;
        }

        public bool Update(NccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery)
        {
            return true;
        }

        public bool Uninstall(NccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery)
        {
            //var deleteQuery = @"
            //;";

            //var nccDbQueryText = new NccDbQueryText() { MySql_QueryText = deleteQuery };
            //var retVal = executeQuery(nccDbQueryText);
            //if (!string.IsNullOrEmpty(retVal))
            //{
            //    return true;
            //}
            //return false;
            return true;
        }
    }
}

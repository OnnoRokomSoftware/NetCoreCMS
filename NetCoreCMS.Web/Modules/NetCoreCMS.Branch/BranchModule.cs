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
using NetCoreCMS.Branch.Repository;
using NetCoreCMS.Branch.Services;
using Microsoft.AspNetCore.Routing;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Core.Models;

namespace NetCoreCMS.Modules.Branch
{
    public class BranchModule : IModule
    {
        List<Widget> _widgets;
        public BranchModule()
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
        public string Path { get ; set ; }
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
            services.AddTransient<NccBranchRepository>();
            services.AddTransient<NccBranchService>();
        }

        public void RegisterRoute(IRouteBuilder routes)
        {

        }

        public bool Install(NccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery)
        {
            var createQuery = @"
                CREATE TABLE IF NOT EXISTS `ncc_branch` ( 
                    `Id` bigint(20) NOT NULL AUTO_INCREMENT,
                    `VersionNumber` int(11) NOT NULL,
                    `Metadata` varchar(250) COLLATE utf8_unicode_ci NULL,
                    `Name` varchar(250) COLLATE utf8_unicode_ci NOT NULL,
                    `CreationDate` datetime NOT NULL,
                    `ModificationDate` datetime NOT NULL,
                    `CreateBy` bigint(20) NOT NULL,
                    `ModifyBy` bigint(20) NOT NULL,
                    `Status` int(11) NOT NULL,
                    `Address` varchar(1000) COLLATE utf8_unicode_ci DEFAULT NULL,
                    `Description` text COLLATE utf8_unicode_ci,
                    `Phone` varchar(250) COLLATE utf8_unicode_ci DEFAULT NULL,
                    `LatLon` varchar(250) COLLATE utf8_unicode_ci DEFAULT NULL,
                    `GoogleLocation` varchar(1000) COLLATE utf8_unicode_ci DEFAULT NULL,
                    `ZoomLevel` int(11) NOT NULL,
                    `IsAdmissionOnly` bit(1) NOT NULL,
                    `Order` int(11) NOT NULL, 
                PRIMARY KEY (`Id`)) ENGINE = MyISAM;
            ";

            var nccDbQueryText = new NccDbQueryText() { MySql_QueryText = createQuery };
            var retVal = executeQuery(nccDbQueryText);
            if (!string.IsNullOrEmpty(retVal))
            {
                return true;
            }
            return false;
        }
        public bool Update(NccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery)
        {
            return true;
        }
        public bool Uninstall(NccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery)
        {
            var deleteQuery = @"
                DROP TABLE IF EXISTS `ncc_branch`;
            ;";

            var nccDbQueryText = new NccDbQueryText() { MySql_QueryText = deleteQuery };
            var retVal = executeQuery(nccDbQueryText);
            if (!string.IsNullOrEmpty(retVal))
            {
                return true;
            }
            return false;
        }
    }
}

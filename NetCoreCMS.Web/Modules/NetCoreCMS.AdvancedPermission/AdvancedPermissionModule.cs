using NetCoreCMS.Framework.Modules;
using System;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules.Widgets;
using System.Collections.Generic;
using System.Reflection;
using NetCoreCMS.Framework.Core.Models.ViewModels;

namespace NetCoreCMS.AdvancedPermission
{
    public class AdvancedPermission : IModule
    {
        public string ModuleId { get; set; }
        public bool IsCore { get; set; }
        public string ModuleTitle { get; set; }
        public string Author { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string DemoUrl { get; set; }
        public string ManualUrl { get; set; }
        public bool AntiForgery { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string MinNccVersion { get; set; }
        public string MaxNccVersion { get; set; }
        public string Category { get; set; }
        public List<NccModuleDependency> Dependencies { get; set; }
        public Assembly Assembly { get; set; }
        public string SortName { get; set; }
        public string Path { get; set; }
        public string Folder { get; set; }
        public int ModuleStatus { get; set; }
        public List<Widget> Widgets { get; set; }
        public List<Menu> Menus { get; set; }

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
             
        }

        public bool Install(NccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery)
        {
            var createQuery = @"
               CREATE TABLE `ncc_ap_UserPermission`( 
                        `Id` BIGINT NOT NULL AUTO_INCREMENT, 
                        `VersionNumber` INT, 
                        `Metadata` VARCHAR(256), 
                        `Name` VARCHAR(256), 
                        `CreationDate` DATETIME, 
                        `ModificationDate` DATETIME, 
                        `CreateBy` BIGINT, 
                        `ModifyBy` BIGINT, 
                        `Status` INT, 
                        `User_Id` BIGINT, 
                        `ModuleId` VARCHAR(256), 
                        `Controller` VARCHAR(256), 
                        `Action` VARCHAR(256), 
                        PRIMARY KEY (`Id`) ) ENGINE=INNODB CHARSET=ucs2 COLLATE=ucs2_unicode_ci; 

                ALTER TABLE `ncc_ap_userpermission` 
                ADD CONSTRAINT `Fk_user_Id` FOREIGN KEY (`User_Id`) REFERENCES `ncc_user`(`Id`) ON 
                UPDATE NO ACTION ON DELETE NO ACTION; 
            ";

            var nccDbQueryText = new NccDbQueryText() { MySql_QueryText = createQuery };
            var retVal = executeQuery(nccDbQueryText);
            if (!string.IsNullOrEmpty(retVal))
            {
                return true;
            }
            return false;
        }

        public void RegisterRoute(IRouteBuilder routes)
        {
             
        }

        public bool Uninstall(NccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery)
        {
            var deleteQuery = "DROP TABLE  IF EXISTS `ncc_ap_userpermission`;";
            var nccDbQueryText = new NccDbQueryText() { MySql_QueryText = deleteQuery };
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
    }
}

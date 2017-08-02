using Microsoft.Extensions.DependencyInjection;
using NetCoreCMS.Framework.Modules;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using NetCoreCMS.Framework.Modules.Widgets;
using Microsoft.AspNetCore.Routing;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.MatGallery.Services;
using NetCoreCMS.MatGallery.Repository;

namespace NetCoreCMS.Modules.MatGallery
{
    public class MatGalleryModule : IModule
    {
        List<Widget> _widgets;
        public MatGalleryModule()
        {
             
        }

        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string SortName { get; set; }
        public bool AntiForgery { get; set ; }
        public string Author { get ; set ; }
        public string Website { get ; set ; }
        public string Version { get ; set ; }
        public string NetCoreCMSVersion { get ; set ; }
        public string Description { get ; set ; }

        public List<string> Dependencies { get ; set ; }
        public string Category { get ; set ; }

        [NotMapped]
        public Assembly Assembly { get; set; }
        public string Path { get ; set ; }
        public int ModuleStatus { get; set; }
        public string ModuleTitle { get ; set ; }
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
            services.AddTransient<NccUserModuleRepository>();
            services.AddTransient<NccUserModuleLogRepository>();
            services.AddTransient<NccUserModuleService>();
        }

        public void RegisterRoute(IRouteBuilder routes)
        {
            
        }

        public bool Install(NccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery)
        {
            var createQuery = @"
                CREATE TABLE `Ncc_MTG_User_Module` ( 
                    `Id` BIGINT NOT NULL AUTO_INCREMENT , 
                    `VersionNumber` INT NOT NULL , 
                    `Name` VARCHAR(250) NOT NULL , 
                    `CreationDate` DATETIME NOT NULL , 
                    `ModificationDate` DATETIME NOT NULL , 
                    `CreateBy` BIGINT NOT NULL , 
                    `ModifyBy` BIGINT NOT NULL , 
                    `Status` INT NOT NULL , 

                    `ModuleVersion` VARCHAR(255) NULL , 
                    `NetCoreCMSVersion` VARCHAR(255) NULL , 
                    `ModuleId` VARCHAR(255) NULL , 
                    `ModuleTitle` VARCHAR(255) NULL , 
                    `Description` TEXT NULL , 
                    `Author` VARCHAR(255) NULL , 
                    `Email` VARCHAR(255) NULL , 
                    `Website` VARCHAR(255) NULL , 
                PRIMARY KEY (`Id`)) ENGINE = MyISAM;

                CREATE TABLE `Ncc_MTG_User_Module_Log` ( 
                    `Id` BIGINT NOT NULL AUTO_INCREMENT , 
                    `VersionNumber` INT NOT NULL , 
                    `Name` VARCHAR(250) NOT NULL , 
                    `CreationDate` DATETIME NOT NULL , 
                    `ModificationDate` DATETIME NOT NULL , 
                    `CreateBy` BIGINT NOT NULL , 
                    `ModifyBy` BIGINT NOT NULL , 
                    `Status` INT NOT NULL , 

                    `ModuleVersion` VARCHAR(255) NULL , 
                    `NetCoreCMSVersion` VARCHAR(255) NULL , 
                    `ModuleId` VARCHAR(255) NULL , 
                    `ModuleTitle` VARCHAR(255) NULL , 
                    `Description` TEXT NULL , 
                    `Author` VARCHAR(255) NULL , 
                    `Email` VARCHAR(255) NULL , 
                    `Website` VARCHAR(255) NULL , 

                    `NccUserModuleId` BIGINT NOT NULL , 
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

        public bool Uninstall(NccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery)
        {
            var deleteQuery = @"
                DROP TABLE `Ncc_MTG_User_Module_Log`; 
                DROP TABLE `Ncc_MTG_User_Module`;
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

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
using System.ComponentModel.DataAnnotations.Schema;
using NetCoreCMS.Framework.Modules.Widgets;
using Microsoft.AspNetCore.Routing;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Core.Data;
using Microsoft.Extensions.DependencyInjection;
using NetCoreCMS.Framework.Modules;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Models.ViewModels;
using NetCoreCMS.Modules.News.Models.Entity;
using NetCoreCMS.Framework.Utility;

namespace NetCoreCMS.Modules.News
{
    public class Module : IModule
    {
        List<Widget> _widgets;
        public Module()
        {
             
        }
        public bool IsCore { get; set; }
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
        public string TablePrefix { get; set; }
        public int ModuleStatus { get; set; }
        public string SortName { get ; set ; }
        [NotMapped]
        public List<Widget> Widgets { get { return _widgets; } set { _widgets = value; } }
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
            //services.AddTransient<NeNewsRepository>();
            //services.AddTransient<NeCategoryRepository>();

            //services.AddTransient<NeNewsService>();
            //services.AddTransient<NeCategoryService>();
        }

        public void RegisterRoute(IRouteBuilder routes)
        {
            
        }

        public bool Install(NccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery)
        {
            var createQuery = @"
                CREATE TABLE IF NOT EXISTS `"+ GlobalContext.GetTableName<NeCategory>() + @"` ( 
                    `Id` BIGINT NOT NULL AUTO_INCREMENT , 
                    `VersionNumber` INT NOT NULL , 
                    `Metadata` varchar(250) COLLATE utf8_unicode_ci NULL,
                    `Name` VARCHAR(250) COLLATE utf8_unicode_ci NULL , 
                    `CreationDate` DATETIME NOT NULL , 
                    `ModificationDate` DATETIME NOT NULL , 
                    `CreateBy` BIGINT NOT NULL , 
                    `ModifyBy` BIGINT NOT NULL , 
                    `Status` INT NOT NULL , 
                PRIMARY KEY (`Id`)) ENGINE = InnoDB;

                CREATE TABLE IF NOT EXISTS `"+ GlobalContext.GetTableName<NeCategoryDetails>() + @"` ( 
                    `Id` BIGINT NOT NULL AUTO_INCREMENT , 
                    `VersionNumber` INT NOT NULL , 
                    `Metadata` varchar(250) COLLATE utf8_unicode_ci NULL,
                    `Name` VARCHAR(250) COLLATE utf8_unicode_ci NULL , 
                    `CreationDate` DATETIME NOT NULL , 
                    `ModificationDate` DATETIME NOT NULL , 
                    `CreateBy` BIGINT NOT NULL , 
                    `ModifyBy` BIGINT NOT NULL , 
                    `Status` INT NOT NULL , 

                    `Language` varchar(250) COLLATE utf8_unicode_ci NULL,
                    `NeCategoryId` BIGINT NOT NULL ,                     
                PRIMARY KEY (`Id`)) ENGINE = InnoDB;

                CREATE TABLE IF NOT EXISTS `" + GlobalContext.GetTableName<NeNews>() + @"` ( 
                    `Id` BIGINT NOT NULL AUTO_INCREMENT , 
                    `VersionNumber` INT NOT NULL , 
                    `Metadata` varchar(250) COLLATE utf8_unicode_ci NULL,
                    `Name` VARCHAR(250) NOT NULL , 
                    `CreationDate` DATETIME NOT NULL , 
                    `ModificationDate` DATETIME NOT NULL , 
                    `CreateBy` BIGINT NOT NULL , 
                    `ModifyBy` BIGINT NOT NULL , 
                    `Status` INT NOT NULL , 

                    `HasDateRange` bit(1) NOT NULL , 
                    `PublishDate` DATETIME NULL , 
                    `ExpireDate` DATETIME NULL , 
                    `Order` INT NOT NULL , 
                PRIMARY KEY (`Id`)) ENGINE = InnoDB;

                CREATE TABLE IF NOT EXISTS `" + GlobalContext.GetTableName<NeNewsDetails>() + @"` ( 
                    `Id` BIGINT NOT NULL AUTO_INCREMENT , 
                    `VersionNumber` INT NOT NULL , 
                    `Metadata` varchar(250) COLLATE utf8_unicode_ci NULL,
                    `Name` VARCHAR(250) COLLATE utf8_unicode_ci NULL , 
                    `CreationDate` DATETIME NOT NULL , 
                    `ModificationDate` DATETIME NOT NULL , 
                    `CreateBy` BIGINT NOT NULL , 
                    `ModifyBy` BIGINT NOT NULL , 
                    `Status` INT NOT NULL , 

                    `Language` varchar(250) COLLATE utf8_unicode_ci NULL,
                    `Content` longtext NULL, 
                    `MetaKeyword` varchar(250) COLLATE utf8_unicode_ci NULL,
                    `MetaDescription` varchar(250) COLLATE utf8_unicode_ci NULL,
                    `Excerpt` VARCHAR(1000) NULL, 
                    `NeNewsId` BIGINT NOT NULL ,    
                PRIMARY KEY (`Id`)) ENGINE = InnoDB;

                CREATE TABLE IF NOT EXISTS `" + GlobalContext.GetTableName<NeNewsCategory>() + @"` ( 
                    `NeCategoryId` BIGINT NOT NULL , 
                    `NeNewsId` BIGINT NOT NULL  
                ) ENGINE = InnoDB;
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
                DROP TABLE IF EXISTS `" + GlobalContext.GetTableName<NeNewsCategory>() + @"`;
                DROP TABLE IF EXISTS `" + GlobalContext.GetTableName<NeCategoryDetails>() + @"`;
                DROP TABLE IF EXISTS `" + GlobalContext.GetTableName<NeCategory>() + @"`;
                DROP TABLE IF EXISTS `" + GlobalContext.GetTableName<NeNewsDetails>() + @"`; 
                DROP TABLE IF EXISTS `" + GlobalContext.GetTableName<NeNews>() + @"`; 
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

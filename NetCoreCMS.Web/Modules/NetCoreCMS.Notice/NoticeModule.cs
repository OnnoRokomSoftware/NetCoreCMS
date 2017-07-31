using Microsoft.Extensions.DependencyInjection;
using NetCoreCMS.Framework.Modules;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.Notice.Widgets;
using NetCoreCMS.Notice.Repository;
using NetCoreCMS.Notice.Services;
using Microsoft.AspNetCore.Routing;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Core.Data;

namespace NetCoreCMS.Modules.Notice
{
    public class NoticeModule : IModule
    {
        List<Widget> _widgets;
        public NoticeModule()
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
            services.AddTransient<NccNoticeRepository>();
            services.AddTransient<NccNoticeService>();
        }

        public void RegisterRoute(IRouteBuilder routes)
        {
            throw new NotImplementedException();
        }

        public bool Install(NccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery)
        {
            var noticeTableCreateQuery = "CREATE TABLE `ncc_notice` ( `Id` BIGINT NOT NULL AUTO_INCREMENT , `VersionNumber` INT NOT NULL , `Name` VARCHAR(250) NOT NULL , `CreationDate` DATETIME NOT NULL , `ModificationDate` DATETIME NOT NULL , `CreateBy` BIGINT NOT NULL , `ModifyBy` BIGINT NOT NULL , `Status` INT NOT NULL , `Title` VARCHAR(255) NOT NULL , `Content` TEXT NOT NULL , `NoticeStatus` INT NOT NULL , `NoticeType` INT NOT NULL , `PublishDate` DATETIME NOT NULL , `ExpireDate` DATETIME NOT NULL , `NoticeOrder` INT NOT NULL , PRIMARY KEY (`Id`)) ENGINE = MyISAM;";
            var nccDbQueryText = new NccDbQueryText() { MySql_QueryText = noticeTableCreateQuery };

            var retVal = executeQuery(nccDbQueryText);
            if (!string.IsNullOrEmpty(retVal))
            {
                return true;
            }
            return false;
        }

        public bool Uninstall(NccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery)
        {
            var noticeTableDeleteQuery = "DROP TABLE `ncc_notice`";
            var nccDbQueryText = new NccDbQueryText() { MySql_QueryText = noticeTableDeleteQuery };
            var retVal = executeQuery(nccDbQueryText);
            if (!string.IsNullOrEmpty(retVal))
            {
                return true;
            }
            return false;
        }
    }
}

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
using System.Linq;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.Framework.Modules;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Themes;

using NetCoreCMS.Framework.Setup;
using System.Collections;
using Microsoft.AspNetCore.Http;
using NetCoreCMS.Framework.Core.Mvc.Extensions;

namespace NetCoreCMS.Framework.Utility
{
    public class GlobalContext
    {
        public GlobalContext()
        {
            
        }

        public static NccWebSite WebSite { get; set; }
        public static bool IsRestartRequired { get; set; }
        public static List<IModule> Modules { get; set; } = new List<IModule>();
        public static List<Widget> Widgets{ get; set; } = new List<Widget>();
        public static List<NccWebSiteWidget> WebSiteWidgets { get; set; } = new List<NccWebSiteWidget>();
        public static List<Theme> Themes { get; set; } = new List<Theme>();
        public static List<NccMenu> Menus { get; set; } = new List<NccMenu>();
        public static SetupConfig SetupConfig { get; set; }
        public static string WebRootPath { get; set; }
        public static string ContentRootPath { get; set; }
        
        public static IApplicationBuilder App { get; set; }
        //public static Theme ActiveTheme { get; set; }
        
        public string SiteBaseUrl { get; set; }
        public string StartupController { get; set; }

        public static string CurrentLanguage { get; set; }

        public static List<IModule> GetActiveModules()
        {
            var query = from m in Modules where m.ModuleStatus == (int) NccModule.NccModuleStatus.Active select m;
            return query.ToList();
        }

        public string StartupAction { get; set; }

        public static IServiceCollection Services { get; set; }
        public static Hashtable ShortCodes { get; set; }
        public static IServiceProvider ServiceProvider { get; set; }

        public static void ListWidgets()
        {
            foreach (var item in Modules)
            {
                Widgets.AddRange(item.Widgets);
            }
        }

        public static long GetCurrentUserId()
        {
            HttpContextAccessor hca = new HttpContextAccessor();
            long? userId = hca.HttpContext?.User?.GetUserId();
            if (userId == null)
                return 0;
            return userId.Value;
        }

        public static Theme GetThemeByName(string themeName)
        {
            return Themes.Where(x => x.ThemeName == themeName).FirstOrDefault();
        } 

        public static string GetCurrentUserName()
        {
            HttpContextAccessor hca = new HttpContextAccessor();
            string userName = hca.HttpContext?.User?.Identity.Name;
            return userName;
        }

        public static IModule GetModuleByModuleId(string moduleId)
        {
            return Modules.Where(x => x.ModuleId == moduleId).FirstOrDefault();
        }
    }
}
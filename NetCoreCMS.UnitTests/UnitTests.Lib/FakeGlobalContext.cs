using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Modules;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using System;
using System.Collections.Generic;

namespace UnitTests.Lib
{
    public class FakeGlobalContext
    {
        public static void EnableMultiLanguage()
        {
            GlobalContext.WebSite = FakeNccWebSite.GetNccWebsite();
            GlobalContext.WebSite.IsMultiLangual = true;
        }

        public static void SetGlobalContextProperties()
        {
            GlobalContext.WebSite = FakeNccWebSite.GetNccWebsite();
            GlobalContext.App = null;
            GlobalContext.ContentRootPath = "C:\\initpub\\wwwroot\\NetCoreCMS\\wwwroot";
            GlobalContext.IsRestartRequired = false;
            GlobalContext.Menus = new List<NccMenu>();
            GlobalContext.Modules = new List<IModule>();
            GlobalContext.ServiceProvider = null;
            GlobalContext.Services = null;
            GlobalContext.SetupConfig = new SetupConfig();
            GlobalContext.ShortCodes = new System.Collections.Hashtable();
            GlobalContext.Themes = new List<Theme>();
            GlobalContext.WebRootPath = "C:\\initpub\\wwwroot\\NetCoreCMS";
            GlobalContext.WebSiteWidgets = new List<NccWebSiteWidget>();
            GlobalContext.Widgets = new List<NetCoreCMS.Framework.Modules.Widgets.Widget>();
        }

        public static void DisableMultiLanguage()
        {
            GlobalContext.WebSite = FakeNccWebSite.GetNccWebsite();
            GlobalContext.WebSite.IsMultiLangual = false;
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using System;
using System.Collections.Generic;
using System.IO;


namespace NetCoreCMS.Modules.Cms.Controllers
{
    public class CmsThemeController : NccController
    {
        ThemeManager _themeManager;
        ILoggerFactory _loggerFactory;
        ILogger _logger;

        public CmsThemeController(ThemeManager themeManager, ILoggerFactory factory)
        {
            _themeManager = themeManager;
            ILoggerFactory _loggerFactory = factory;
            _logger = _loggerFactory.CreateLogger<CmsThemeController>();
        }
        public ActionResult Index()
        {
            SetThemeViewData();
            return View();
        }

        private void SetThemeViewData()
        {
            var themePath = Path.Combine(GlobalConfig.ContentRootPath, NccInfo.ThemeFolder);
            ViewBag.Themes = GlobalConfig.Themes;
            ViewBag.ThemePath = themePath;
        }

        public ActionResult Activate(string themeName)
        {
            _themeManager.ActivateTheme(themeName);
            TempData["SuccessMessage"] = "Theme "+ themeName +" Activated Successfully. Restart site";

            return RedirectToAction("Index");
        }

        public ActionResult Install()
        {
            SetThemeViewData();
            return View();
        }

        //[HttpPost]
        //public ActionResult Install(HttpPostedFileBase file)
        //{
        //    SetThemeViewData();
        //    return View();
        //}
    }
}

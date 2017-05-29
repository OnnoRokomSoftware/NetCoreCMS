using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Network;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCoreCMS.Modules.Cms.Controllers
{
    public class CmsWidgetController : NccController
    {
        NccWebSiteWidgetService _nccWebSiteWidgetService;
        NccWebSiteService _nccWebSiteService;
        ILoggerFactory _loggerFactory;
        ILogger _logger;

        public CmsWidgetController(NccWebSiteWidgetService nccWebSiteWidgetService, NccWebSiteService nccWebSiteService, ILoggerFactory factory)
        {
            _loggerFactory = factory;
            _logger = factory.CreateLogger<CmsWidgetController>();
            _nccWebSiteWidgetService = nccWebSiteWidgetService;
            _nccWebSiteService = nccWebSiteService;
        }

        public ActionResult Index()
        {
            ViewBag.Modules = GlobalConfig.Modules;
            ViewBag.Theme = GlobalConfig.ActiveTheme;
            ViewBag.WebsiteWidgetZones = _nccWebSiteWidgetService.LoadAll();
            return View();
        }

        [HttpPost]
        public JsonResult SaveZoneWidget(string module, string theme, string layout, string zone, string widget)
        {
            var currentWebsite = _nccWebSiteService.LoadAll().FirstOrDefault();
            var nccWebSiteWidget = new NccWebSiteWidget() {
                LayoutName = layout,
                WebSite = currentWebsite,
                WidgetConfigJson = "",
                WidgetData = "",
                ThemeId = theme,
                WidgetId = widget,
                WidgetOrder = 1,
                Zone = zone,
                ModuleId = module,
            };

            _nccWebSiteWidgetService.Save(nccWebSiteWidget);

            return Json(new ApiResponse() { IsSuccess=true, Message="Save Successful." });
        }
        
        [HttpPost]
        public JsonResult RemoveZoneWidget(string module, string theme, string layout, string zone, string widget)
        {
            _nccWebSiteWidgetService.RemoveByModuleThemeLayoutZoneWidget(module,theme,layout,zone,widget);
            return Json(new ApiResponse() { IsSuccess = true, Message = "Remove Successful." });
        }
        public JsonResult SaveConfig(string widgetId, string configJson, string widgetData)
        {
            return Json(new ApiResponse());
        }

        public JsonResult GetConfig(string widgetId)
        {
            return Json(new ApiResponse());
        }
    }
}

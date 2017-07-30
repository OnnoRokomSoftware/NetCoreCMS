using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Network;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCoreCMS.Modules.Cms.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    [AdminMenu(Name = "Appearance", IconCls = "fa-tasks", Order = 5)]
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

        [AdminMenuItem(Name = "Widget", Url = "/CmsWidget", IconCls = "fa-magic", Order = 2)]
        public ActionResult Index()
        {
            ViewBag.Modules = GlobalConfig.GetActiveModules();
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
            GlobalConfig.WebSiteWidgets = _nccWebSiteWidgetService.LoadAll();

            return Json(new ApiResponse() { IsSuccess=true, Message="Save Successful." });
        }
        
        [HttpPost]
        public JsonResult RemoveZoneWidget(string module, string theme, string layout, string zone, string widget)
        {
            _nccWebSiteWidgetService.RemoveByModuleThemeLayoutZoneWidget(module,theme,layout,zone,widget);
            GlobalConfig.WebSiteWidgets = _nccWebSiteWidgetService.LoadAll();
            return Json(new ApiResponse() { IsSuccess = true, Message = "Remove Successful." });
        }


        /// <summary>
        /// Save your website widget configuration
        /// </summary>
        /// <param name="webSiteWidgetId"></param>
        /// <param name="data"></param>
        /// <returns>Response JSON </returns>
        public JsonResult SaveConfig(long webSiteWidgetId, string data)
        {
            var apiResponse = new ApiResponse(false, "Config save failed.");
            var webSiteWidget = _nccWebSiteWidgetService.Get(webSiteWidgetId);
            if (webSiteWidget != null)
            {
                webSiteWidget.WidgetConfigJson = data;
                _nccWebSiteWidgetService.Update(webSiteWidget);
                apiResponse.IsSuccess = true;
                apiResponse.Message = "Config save successfully.";
            }
            return Json(apiResponse);
        }

        public JsonResult GetConfig(long webSiteWidgetId)
        {
            var apiResponse = new ApiResponse(false, "Config save failed.");
            var webSiteWidget = _nccWebSiteWidgetService.Get(webSiteWidgetId);
            if (webSiteWidget != null)
            {
                apiResponse.IsSuccess = true;
                apiResponse.Message = "Success";
                apiResponse.Data = webSiteWidget.WidgetConfigJson;
            }
            return Json(apiResponse);
        } 
    }
}

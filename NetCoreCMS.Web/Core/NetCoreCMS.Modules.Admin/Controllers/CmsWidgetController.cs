/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/
 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Attributes;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Network;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCoreCMS.Core.Modules.Admin.Controllers
{
    [AdminMenu(Name = "Appearance", IconCls = "fa-tasks", Order = 4)]
    public class CmsWidgetController : NccController
    {
        NccWebSiteWidgetService _nccWebSiteWidgetService;
        NccWebSiteService _nccWebSiteService;
        ILoggerFactory _loggerFactory;        

        public CmsWidgetController(NccWebSiteWidgetService nccWebSiteWidgetService, NccWebSiteService nccWebSiteService, ILoggerFactory factory)
        {
            _loggerFactory = factory;
            _logger = factory.CreateLogger<CmsWidgetController>();
            _nccWebSiteWidgetService = nccWebSiteWidgetService;
            _nccWebSiteService = nccWebSiteService;
        }

        [AdminMenuItem(Name = "Widget", Url = "/CmsWidget", IconCls = "fa-magic", SubActions = new string[] { "SaveZoneWidget", "RemoveZoneWidget", "RemoveZoneWidgetById", "UpdateWebsiteWidgetOrder", "SaveConfig", "GetConfig" }, Order = 2)]
        public ActionResult Index(string sLayout = "")
        {
            ViewBag.sLayout = "";
            ViewBag.Modules = GlobalContext.GetActiveModules();
            ViewBag.Theme = ThemeHelper.ActiveTheme;
            if (ThemeHelper.ActiveTheme.Layouts.Where(x => x.Name == sLayout).Count() > 0)
            {
                ViewBag.sLayout = sLayout;
            }
            ViewBag.WebsiteWidgetZones = _nccWebSiteWidgetService.LoadAll().OrderBy(x => x.WidgetOrder).ToList();
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
            GlobalContext.WebSiteWidgets = _nccWebSiteWidgetService.LoadAll().OrderBy(x => x.WidgetOrder).ToList();

            return Json(new ApiResponse() { IsSuccess=true, Message="Save Successful.", Data = nccWebSiteWidget });
        }
        
        [HttpPost]
        public JsonResult RemoveZoneWidget(string module, string theme, string layout, string zone, string widget)
        {
            _nccWebSiteWidgetService.RemoveByModuleThemeLayoutZoneWidget(module,theme,layout,zone,widget);
            GlobalContext.WebSiteWidgets = _nccWebSiteWidgetService.LoadAll().OrderBy(x => x.WidgetOrder).ToList();
            return Json(new ApiResponse() { IsSuccess = true, Message = "Remove Successful." });
        }

        [HttpPost]
        public JsonResult RemoveZoneWidgetById(long zoneWidgetId)
        {
            var rsp = _nccWebSiteWidgetService.RemoveByZoneWidgetId(zoneWidgetId);
            if (rsp.StartsWith("Error:"))
            {
                Json(new ApiResponse() { IsSuccess = false, Message = "Remove Failed." });
            }
            GlobalContext.WebSiteWidgets = _nccWebSiteWidgetService.LoadAll().OrderBy(x => x.WidgetOrder).ToList();
            return Json(new ApiResponse() { IsSuccess = true, Message = "Remove Successful." });
        }

        [HttpPost]
        public JsonResult UpdateWebsiteWidgetOrder(long webSiteWidgetId, int oldOrder, string operation)
        {
            if(operation == "up")
            {
                _nccWebSiteWidgetService.UpOrder(webSiteWidgetId, oldOrder);
            }
            else
            {
                _nccWebSiteWidgetService.DownOrder(webSiteWidgetId, oldOrder);
            }
            
            GlobalContext.WebSiteWidgets = _nccWebSiteWidgetService.LoadAll().OrderBy(x=>x.WidgetOrder).ToList();
            return Json(new ApiResponse() { IsSuccess = true, Message = "Order update Successful." });
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
            var apiResponse = new ApiResponse(false, "Config get failed.");
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

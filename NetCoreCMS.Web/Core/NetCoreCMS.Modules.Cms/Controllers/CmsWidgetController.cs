using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Network;
using NetCoreCMS.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Modules.Cms.Controllers
{
    public class CmsWidgetController : NccController
    {
        public ActionResult Index()
        {
            ViewBag.Modules = GlobalConfig.Modules;
            ViewBag.Theme = GlobalConfig.ActiveTheme;
            return View();
        }

        public JsonResult SaveConfig(string widgetId, string configJson)
        {
            return Json(new ApiResponse());
        }

        public JsonResult GetConfig(string widgetId)
        {
            return Json(new ApiResponse());
        }
    }
}

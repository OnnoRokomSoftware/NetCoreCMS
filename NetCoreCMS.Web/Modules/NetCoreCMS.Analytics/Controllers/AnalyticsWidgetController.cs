using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Modules.Analytics.Controllers
{
    public class AnalyticsWidgetController : NccController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}

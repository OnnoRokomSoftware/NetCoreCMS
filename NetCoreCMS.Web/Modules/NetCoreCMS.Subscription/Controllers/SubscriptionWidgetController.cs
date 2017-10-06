using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace NetCoreCMS.Subscription.Controllers
{
    public class SubscriptionWidgetController : NccController
    {
        public SubscriptionWidgetController(ILoggerFactory loggerFactory)
        {
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}

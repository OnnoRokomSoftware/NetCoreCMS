using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace NetCoreCMS.HelloWorld.Controllers
{
    public class HelloWidgetController : NccController
    {
        public HelloWidgetController(ILoggerFactory loggerFactory)
        {
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.i18n;

namespace NetCoreCMS.HelloWorld.Controllers
{
    [AdminMenu(Name = "Hello Module", Order = 100)]
    [SiteMenu(Name = "Hello Module Site Menu", Order = 100)]
    public class HelloHomeController : NccController
    {
        public HelloHomeController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HelloHomeController>();
        }

        [AdminMenuItem(Name = "Index", Url = "/HelloHome/Index", Order = 1)]
        [SiteMenuItem(Name = "Hello world Home", Url = "/HelloHome/Index", Order = 1)]
        public ActionResult Index()
        {
            var nccTranslator = new NccTranslator<HelloHomeController>(CurrentLanguage);
            return View();
        }

        [SiteMenuItem(Name = "Role Home", Url = "/HelloHome/RoleHome", Order = 1)]
        public ActionResult RoleHome()
        {
            return View();
        }
    }
}

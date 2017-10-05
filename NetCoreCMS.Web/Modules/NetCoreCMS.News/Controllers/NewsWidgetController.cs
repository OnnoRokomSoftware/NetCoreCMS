using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Modules.News.Controllers
{
    public class NewsWidgetController : NccController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}

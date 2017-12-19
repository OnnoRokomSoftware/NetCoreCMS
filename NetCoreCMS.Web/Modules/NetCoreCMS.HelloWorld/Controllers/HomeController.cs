using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Mvc.Controllers;

namespace NetCoreCMS.HelloWorld.Controllers
{
    public class HomeController : NccController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}

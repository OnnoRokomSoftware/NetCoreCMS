using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.HelloWorld.Controllers
{
    public class HelloHomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}

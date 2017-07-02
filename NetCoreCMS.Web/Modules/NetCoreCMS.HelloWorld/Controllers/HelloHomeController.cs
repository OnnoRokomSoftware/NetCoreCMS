/*
 * Author: TecRT
 * Website: http://tecrt.com
 * Copyright (c) tecrt.com
 * License: BSD (3 Clause)
*/
using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.HelloWorld.Controllers
{
    [AdminMenu(Name = "Hello Module", Order = 1)]
    public class HelloHomeController : NccController
    {
        [AdminMenuItem(Name = "Index", Url = "/HelloHome/Index", Order = 1)]
        public ActionResult Index()
        {
            return View();
        }
    }
}

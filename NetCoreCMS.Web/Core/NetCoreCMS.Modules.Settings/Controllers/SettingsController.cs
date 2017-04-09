/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Mvc.Controllers;

namespace NetCoreCMS.Core.Modules.Settings.Controllers
{
    public class SettingsController : NccController
    {
        public ActionResult Index()
        { 
             return View(); 
        }
        
    }
}

/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Setup;

namespace NetCoreCMS.Core.Modules.Cms.Controllers
{
    [AllowAnonymous]
    public class CmsHomeController : NccController
    {
        public ActionResult Index()
        {
            if (SetupHelper.IsDbCreateComplete && SetupHelper.IsAdminCreateComplete)
            {
                return View();
            }
            return Redirect("/SetupHome/Index");
            
        }
        
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult ResourceNotFound()
        {   
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
         
    }
}

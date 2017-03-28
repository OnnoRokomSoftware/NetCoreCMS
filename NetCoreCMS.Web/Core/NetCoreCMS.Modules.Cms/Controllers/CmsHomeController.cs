using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Modules.Cms.Controllers
{
    public class CmsHomeController : NccController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}

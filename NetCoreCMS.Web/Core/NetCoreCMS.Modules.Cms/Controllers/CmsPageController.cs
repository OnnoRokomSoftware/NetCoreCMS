using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Setup;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Modules.Cms.Controllers
{
    [Authorize]
    public class CmsPageController : Controller
    {
        public ActionResult Index()
        {
            if (SetupHelper.IsDbCreateComplete && SetupHelper.IsAdminCreateComplete)
            {
                return View();
            }
            return Redirect("/SetupHome/Index");
        }
    }
}

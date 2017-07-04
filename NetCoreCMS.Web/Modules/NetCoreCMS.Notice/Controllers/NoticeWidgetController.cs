using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Notice.Controllers
{
    public class NoticeWidgetController : NccController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Modules.OnlineExam.Controllers
{
    public class OnlineExamWidgetController : NccController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}

/*
 * Author: TecRT
 * Website: http://tecrt.com
 * Copyright (c) tecrt.com
 * License: BSD (3 Clause)
*/
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

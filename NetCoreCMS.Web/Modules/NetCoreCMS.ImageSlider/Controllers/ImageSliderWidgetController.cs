using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.ImageSlider.Controllers
{
    public class ImageSliderWidgetController : NccController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}

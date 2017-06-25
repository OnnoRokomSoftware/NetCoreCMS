/*
 * Author: TecRT
 * Website: http://tecrt.com
 * Copyright (c) tecrt.com
 * License: BSD (3 Clause)
*/
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Network;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Modules.Cms.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCoreCMS.Core.Modules.Cms.Controllers
{
    [Authorize]
    public class CmsMediaController : Controller
    {
        private readonly ILogger _logger;
        public CmsMediaController(ILoggerFactory factory)
        {
            _logger = factory.CreateLogger<CmsMediaController>();
        }
        
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Upload()
        {            
            return View();
        }


        [HttpPost]
        public ActionResult Upload(NccPage model, string PageContent, long ParentId, string SubmitType)
        {
            
            return View();
        }        

        public ActionResult Delete(string fullPath)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(string fullPath, int status)
        {            
            ViewBag.MessageType = "SuccessMessage";
            ViewBag.Message = "Page deleted successful";
            return RedirectToAction("Index");
        }
    }
}

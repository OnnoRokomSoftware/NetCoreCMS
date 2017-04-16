/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Network;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Modules.Cms.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Core.Modules.Cms.Controllers
{
    [Authorize]
    public class CmsPageController : Controller
    {
        NccPageService _pageService;
        private readonly ILogger _logger;
        public CmsPageController(NccPageService pageService, ILoggerFactory factory)
        {
            _pageService = pageService;
            _logger = factory.CreateLogger<CmsPageController>();
        }
        public ActionResult Index()
        {
            var allPages = _pageService.GetAll();
            return View(allPages); 
        }

        [HttpPost]
        public ActionResult Index(NccPage model)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(NccPage model, string PageContent, long Parent)
        {
            ApiResponse rsp = new ApiResponse();
            try
            {
                model.Content = Encoding.UTF8.GetBytes(PageContent);
                if (ModelState.IsValid)
                {
                    _pageService.Save(model);
                    rsp.IsSuccess = true;
                    rsp.Message = "Page save successful";
                    rsp.Data = "";
                    return Json(rsp);
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError("Page create error.", ex.ToString());
            }

            rsp.IsSuccess = false;
            rsp.Message = "Error occoured. Please fill up all field correctly.";
            return Json(rsp);
        }
    }
}

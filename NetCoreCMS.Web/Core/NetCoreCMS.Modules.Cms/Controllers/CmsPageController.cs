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

        public ActionResult Index(string id)
        {            
            if(!string.IsNullOrEmpty(id))
            {
                var page = _pageService.GetBySlugs(id);
                if (page != null)
                {
                    return View(page);
                }
            }
            TempData["Message"] = "Page not found";
            return Redirect("/CmsHome/ResourceNotFound");
        }

        [HttpPost]
        public ActionResult Index(NccPage model)
        {
            return View();
        }
        
        public ActionResult Manage()
        {
            var allPages = _pageService.LoadAll();
            return View(allPages);
        }

        public ActionResult CreateEdit(long Id = 0)
        {
            NccPage page = new NccPage();
            page.Content = Encoding.UTF8.GetBytes("");
            if (Id > 0)
            {
                page = _pageService.Get(Id);
            }
            return View(page);
        }

        [HttpPost]
        public ActionResult CreateEdit(NccPage model, string PageContent, long Parent)
        {
            ApiResponse rsp = new ApiResponse();
            if (model.Id > 0)
            {
                try
                {
                    model.Content = Encoding.UTF8.GetBytes(PageContent);
                    if (ModelState.IsValid)
                    {
                        _pageService.Update(model);
                        rsp.IsSuccess = true;
                        rsp.Message = "Page updated successful";
                        rsp.Data = "";
                        return Json(rsp);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Page create error.", ex.ToString());
                }
            }
            else
            {
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
            }
            rsp.IsSuccess = false;
            rsp.Message = "Error occoured. Please fill up all field correctly.";
            return Json(rsp);
        }

        public ActionResult Delete(long Id)
        {
            NccPage page = _pageService.Get(Id);
            //page.
            return View(page);
        }

        [HttpPost]
        public ActionResult Delete(long Id, int status)
        {
            ApiResponse rsp = new ApiResponse();
            _pageService.DeletePermanently(Id);
            //rsp.IsSuccess = true;
            //rsp.Message = "Page deleted successful";
            //rsp.Data = "";
            ViewBag.MessageType = "SuccessMessage";
            ViewBag.Message = "Page deleted successful";
            return RedirectToAction("Index");
        }
    }
}

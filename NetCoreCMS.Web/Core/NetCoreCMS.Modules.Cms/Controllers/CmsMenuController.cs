using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Network;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Modules.Cms.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Modules.Cms.Controllers
{
    public class CmsMenuController : NccController
    {
        NccMenuService _menuService;
        NccPageService _pageService;

        public CmsMenuController(NccMenuService menuService, NccPageService pageService)
        {
            _pageService = pageService;
            _menuService = menuService;
        }
        public ActionResult Index()
        {
            ViewBag.AllPages = _pageService.LoadAllByPageStatus(NccPage.NccPageStatus.Published);
            ViewBag.RecentPages = _pageService.LoadRecentPages(5);

            return View();
        }

        [HttpPost]
        public JsonResult CreateMenu(string model)
        {
            var query = Request.Query;
            var menu = JsonConvert.DeserializeObject<NccMenuViewModel>(model);

            if(menu != null)
            {
                var r = new ApiResponse();
                r.IsSuccess = true;
                r.Message = "Received the tree, have to write server side code.";
                return Json(r);
            }

            ApiResponse rsp = new ApiResponse();
            rsp.IsSuccess = false;
            rsp.Message = "Error occoured. Please fill up all field correctly.";
            return Json(rsp);
        }
    }
}

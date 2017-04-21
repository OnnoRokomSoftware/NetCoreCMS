using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Services;
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

        public JsonResult CreateMenu(CmsMenuViewModel model)
    }
}

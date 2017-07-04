using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Utility;
using System;
using System.Linq;

namespace NetCoreCMS.Core.Modules.Admin.Controllers
{
    [Authorize("SuperAdmin")]
    public class AdminController : NccController
    {
        NccWebSiteService _webSiteService;
        NccPageService _pageService;
        NccPostService _postService;
        NccCategoryService _categoryService;
        ILogger _logger;

        public AdminController(
            NccWebSiteService nccWebSiteService, 
            NccPageService pageService, 
            NccPostService postService, 
            NccCategoryService categoryService,
            ILoggerFactory loggarFactory)
        {
            _webSiteService = nccWebSiteService;
            _pageService = pageService;
            _postService = postService;
            _categoryService = categoryService;
            _logger = loggarFactory.CreateLogger<AdminController>();
        }

        public ActionResult Index()
        {
            var webSite = new NccWebSite();
            var webSites = _webSiteService.LoadAll();

            if (webSites != null && webSites.Count > 0)
            {
                webSite = webSites.FirstOrDefault();
            }
            return View(webSite);
        }

        public ActionResult Settings()
        {
            var webSite = new NccWebSite();
            var webSites = _webSiteService.LoadAll();

            if (webSites != null && webSites.Count > 0)
            {
                webSite = webSites.FirstOrDefault();
            }
            return View(webSite);
        }

        [HttpPost]
        public ActionResult Settings(NccWebSite website)
        {
            if (ModelState.IsValid)
            {
                _webSiteService.Update(website);
            }
            else
            {
                ModelState.AddModelError("Name", "Please check all values and submit again.");
            }
            GlobalConfig.WebSite = website;
            return View(website);
        }

        public ActionResult Startup()
        {
            ViewBag.Pages = _pageService.LoadAllActive();
            ViewBag.Posts = _postService.LoadAllByPostStatusAndDate(NccPost.NccPostStatus.Published, DateTime.Now);
            ViewBag.Categories = _categoryService.LoadAllActive();
            ViewBag.ModuleSiteMenus = AdminMenuHelper.ModulesSiteMenus().Select(x=>x.Key).ToList();

            return View();
        }

        //public ContentResult StartupModuleMenuItemByMenu(string menuId)
        //{

        //}
    }
}

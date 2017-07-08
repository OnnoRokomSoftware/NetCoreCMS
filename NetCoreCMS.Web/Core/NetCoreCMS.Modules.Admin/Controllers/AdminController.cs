using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Modules.Admin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreCMS.Core.Modules.Admin.Controllers
{
    [Authorize(Roles ="SuperAdmin,Administrator")]
    [AdminMenu(Name ="Settings", Order = 10)]
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
        
        [Authorize]
        //[AdminMenuItem(Name = "Dashboard", Url = "/Admin", IconCls = "fa-dashboard", Order = 1)]
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

        #region Settings

        [AdminMenuItem(Name = "General", Url = "/Admin/Settings", IconCls = "fa-gear", Order = 2)]
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

        [AdminMenuItem(Name = "Startup", Url = "/Admin/Startup", IconCls = "fa-gear", Order = 3)]
        public ActionResult Startup()
        {
            var model = PrepareStartupViewData();            
            return View(model);
        }

        [HttpPost]
        public ActionResult Startup(StartupViewModel vmodel)
        {
            var setupConfig = SetupHelper.LoadSetup();
            setupConfig.StartupType = vmodel.StartupType;

            if (vmodel.StartupType == StartupTypes.Page)
            {
                setupConfig.StartupUrl = "/" + vmodel.PageSlug;
            }

            if (vmodel.StartupType == StartupTypes.Post)
            {
                setupConfig.StartupUrl = "/Post/" + vmodel.PostSlug;
            }

            if (vmodel.StartupType == StartupTypes.Category)
            {
                setupConfig.StartupUrl = "/Category/" + vmodel.PageSlug;
            }

            if (vmodel.StartupType == StartupTypes.Module)
            {
                setupConfig.StartupUrl = vmodel.ModuleSiteMenuUrl;
            }

            SetupHelper.UpdateSetup(setupConfig);
            var model = PrepareStartupViewData();

            return View(model);
        }
         
        public StartupViewModel PrepareStartupViewData()
        {
            var setupConfig = SetupHelper.LoadSetup();
            var model = new StartupViewModel();
            var moduleSiteMenuList = new List<SiteMenuItem>();

            model.Default = setupConfig.StartupUrl;
            model.StartupType = setupConfig.StartupType;

            model.Pages = new SelectList(_pageService.LoadAllActive(), "Slug", "Title", GetSlug(setupConfig.StartupUrl));
            model.Posts = new SelectList(_postService.LoadAllByPostStatusAndDate(NccPost.NccPostStatus.Published, DateTime.Now), "Slug", "Title", GetSlug(setupConfig.StartupUrl));
            model.Categories = new SelectList(_categoryService.LoadAllActive(), "Slug", "Title", GetSlug(setupConfig.StartupUrl));
            AdminMenuHelper.ModulesSiteMenus().Select(x => x.Value).ToList().ForEach(x => moduleSiteMenuList.AddRange(x));
            model.ModuleSiteMenus = new SelectList(moduleSiteMenuList, "Url", "Url", setupConfig.StartupUrl);

            ViewBag.DefaultChecked = "";
            ViewBag.PageChecked = "";
            ViewBag.CategoryChecked = "";
            ViewBag.PostChecked = "";
            ViewBag.ModuleChecked = "";

            if (setupConfig.StartupType == StartupTypes.Page)
            {
                ViewBag.PageChecked = "checked";
            }
            else if (setupConfig.StartupType == StartupTypes.Post)
            {
                ViewBag.PostChecked = "checked";
            }
            else if (setupConfig.StartupType == StartupTypes.Category)
            {
                ViewBag.CategoryChecked = "checked";
            }
            else if (setupConfig.StartupType == StartupTypes.Module)
            {
                ViewBag.ModuleChecked = "checked";
            }
            else
            {
                ViewBag.DefaultChecked = "checked";
            }

            return model;
        }

        public ActionResult ManageUsers()
        {

            return View();
        }

        #endregion

        #region Privet Methods

        public string GetSlug(string url)
        {
            var slug = "";            
            if (!string.IsNullOrEmpty(url))
            {
                var parts = url.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 1)
                {
                    slug = parts[parts.Length-1];
                }
            }
            return slug;
        }

        #endregion

    }
}

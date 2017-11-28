/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/
 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Utility;
using System.Linq;
using NetCoreCMS.Framework.Core.Mvc.Attributes;

namespace NetCoreCMS.Core.Modules.Admin.Controllers
{
    [AdminMenu( IsVisible = false, Name = "Dashboard", IconCls = "fa-cogs", Order = 99)]
    public class DashboardController : NccController
    {
        NccWebSiteService _webSiteService;
        NccPageService _pageService;
        NccPostService _postService;
        NccCategoryService _categoryService;
        NccSettingsService _settingsService;
        RoleManager<NccRole> _roleManager;
        UserManager<NccUser> _userManager;
        NccStartupService _startupService;
        IHostingEnvironment _hostingEnvironment;
        IConfiguration _configuration;
        NccModuleService _moduleService;

        public DashboardController(NccWebSiteService nccWebSiteService, NccPageService pageService, NccPostService postService, NccCategoryService categoryService, NccSettingsService settingsService, RoleManager<NccRole> roleManager, UserManager<NccUser> userManager, NccStartupService startupService, IConfiguration configuration, IHostingEnvironment hostingEnv,
        NccModuleService moduleService, ILoggerFactory loggarFactory)
        {
            _webSiteService = nccWebSiteService;
            _pageService = pageService;
            _postService = postService;
            _categoryService = categoryService;
            _settingsService = settingsService;
            _roleManager = roleManager;
            _userManager = userManager;
            _startupService = startupService;
            _configuration = configuration;
            _hostingEnvironment = hostingEnv;
            _moduleService = moduleService;
            _logger = loggarFactory.CreateLogger<AdminController>();
        }
        
        [AdminMenuItem( IsVisible = false, Name = "Dashboard", Url = "/Dashboard/Index", IconCls = "fa-dashboard", Order = 1)]
        public ActionResult Index()
        {
            var webSite = new NccWebSite();
            var webSites = _webSiteService.LoadAll();

            if (webSites != null && webSites.Count > 0)
            {
                webSite = webSites.FirstOrDefault();
            }
            ViewBag.TotalPublishedPage = _pageService.LoadAllByPageStatus(NccPage.NccPageStatus.Published).Count();
            ViewBag.TotalPage = _pageService.LoadAll(true).Count();
            ViewBag.TotalPublishedPost = _postService.Count(true, true, true, true);
            ViewBag.TotalPost = _postService.LoadAll(true).Count();
            ViewBag.TotalUser = _userManager.Users.Count();
            ViewBag.TotalModule = _moduleService.LoadAll().Count();
            ViewBag.TotalTheme = GlobalContext.Themes.Count();
            return View(webSite);
        }
    }
}
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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Models.ViewModels;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Network;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.i18n;
using NetCoreCMS.Framework.Modules;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Modules.Admin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace NetCoreCMS.Core.Modules.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    //[AdminMenu(Name = "Dashboard", IconCls = "fa-cogs", Order = 99)]
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

        [Authorize]
        //[AdminMenuItem(Name = "Dashboard", Url = "/Dashboard", IconCls = "fa-dashboard", Order = 1)]
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
            ViewBag.TotalPublishedPost = _postService.TotalPublishedPostCount();
            ViewBag.TotalPost = _postService.LoadAll(true).Count();
            ViewBag.TotalUser = _userManager.Users.Count();
            ViewBag.TotalModule = _moduleService.LoadAll().Count();
            ViewBag.TotalTheme = GlobalConfig.Themes.Count();
            return View(webSite);
        }
    }
}
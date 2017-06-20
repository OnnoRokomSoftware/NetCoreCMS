/*
 * Author: TecRT
 * Website: http://tecrt.com
 * Copyright (c) tecrt.com
 * License: BSD (3 Clause)
*/
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Utility;
using System.Linq;

namespace NetCoreCMS.Core.Modules.Admin.Controllers
{
    [Authorize("SuperAdmin")]
    public class AdminController : NccController
    {
        NccWebSiteService _webSiteService;

        public AdminController(NccWebSiteService nccWebSiteService)
        {
            _webSiteService = nccWebSiteService;
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
    }
}

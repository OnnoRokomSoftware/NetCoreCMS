/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Services;

namespace NetCoreCMS.Core.Modules.Settings.Controllers
{
    public class SettingsController : NccController
    {
        NccWebSiteService _webSiteService;
        public SettingsController(NccWebSiteService nccWebSiteService)
        {
            _webSiteService = nccWebSiteService;
        }
        public ActionResult Index()
        {
            var webSite = new NccWebSite();
            var webSites = _webSiteService.LoadAll();

            if(webSites != null && webSites.Count > 0)
            {
                webSite = webSites.FirstOrDefault();
            }
             return View(webSite); 
        }

        [HttpPost]
        public ActionResult Index(NccWebSite website)
        {
            if (ModelState.IsValid)
            {
                _webSiteService.Update(website);
            }
            else
            {
                ModelState.AddModelError("Name", "Please check all values and submit again.");
            }            
            return View(website);
        }
    }
}

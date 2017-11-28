using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Models.ViewModels;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Utility;
using System.Collections.Generic;

namespace NetCoreCMS.Modules.Cms.Controllers
{
    public class SearchController : NccController
    {
        NccPageService _pageService;
        public SearchController(NccPageService pageService, ILoggerFactory factory)
        {
            _pageService = pageService;
            _logger = factory.CreateLogger<SearchController>();
        }

        [AllowAnonymous]
        public ActionResult Index(string q = "", int page = 0)
        {
            ViewBag.DomainName = (Request.IsHttps == true ? "https://" : "http://") + Request.Host;
            if (q == null) q = "";            
            var postPerPage = GlobalContext.WebSite.WebSitePageSize;
            long totalPost = 0;

            q = q.Trim();
            ViewBag.Q = q;
            List<NccSearchViewModel> searchResults = new List<NccSearchViewModel>();
            if (q != "")
            {
                totalPost = _pageService.SearchCount(q);
                ViewBag.TotalResult = totalPost;
                if (totalPost > 0)
                    searchResults = _pageService.SearchLoad(page * postPerPage, postPerPage, q);
            }
            ViewBag.CurrentPage = page;
            ViewBag.PostPerPage = postPerPage;
            ViewBag.TotalPost = totalPost;
            return View(searchResults);
        }
    }
}
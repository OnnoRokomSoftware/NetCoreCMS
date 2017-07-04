using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using System;

namespace NetCoreCMS.Core.Modules.Media.Controllers
{
    [SiteMenu(Name = "Blog", Order = 1)]
    public class PostController : NccController
    {
        NccPostService _nccPostService;
        NccUserService _nccUserService;
        ILoggerFactory _loggerFactory;
        ILogger _logger;

        public PostController(NccPostService nccPostService, NccUserService nccUserService, ILoggerFactory loggerFactory)
        {
            _nccPostService = nccPostService;
            _loggerFactory = loggerFactory;
            _nccUserService = nccUserService;
            _logger = _loggerFactory.CreateLogger<BlogController>();
        }

        [AllowAnonymous]
        [SiteMenuItem(Name = "Index", Url = "/Post/Index", Order = 1)]
        public ActionResult Index(string slug="", int page = 0)
        {
            if (!string.IsNullOrEmpty(slug))
            {
                var post = _nccPostService.GetBySlugs(slug);
                if (post == null)
                    return Redirect(Constants.NotFoundUrl);
                return View("Details", post);
            }
            else
            {
                var allPost = _nccPostService.LoadAllByPostStatusAndDate(NccPost.NccPostStatus.Published, DateTime.Now);
                return View(allPost);
            }
        }

        [AllowAnonymous]        
        public ActionResult Details(string slug)
        {
            var post = _nccPostService.GetBySlugs(slug);
            if (post == null)
                return Redirect(Constants.NotFoundUrl);
            return View(post);
        }
        
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Services;

namespace NetCoreCMS.Core.Modules.Media.Controllers
{

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
        public ActionResult Index(int page = 0)
        {
            var allPost = _nccPostService.LoadAllByPostStatusAndDate(NccPost.NccPostStatus.Published);
            return View(allPost); 
        }

        [AllowAnonymous]
        public ActionResult Details(string slug)
        {
            var post = _nccPostService.GetBySlugs(slug);
            if (post == null)
                return Redirect("/Home/Error");
            return View(post);
        }
        
    }
}

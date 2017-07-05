using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Utility;
using System;
using NetCoreCMS.Framework.Core.Mvc.Extensions;
using System.Linq;
using NetCoreCMS.Framework.Core.Network;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Themes;

namespace NetCoreCMS.Core.Modules.Media.Controllers
{
    [Authorize(Roles ="SuperAdmin,Adimistrator,Editor")]
    [AdminMenu(Name ="Blog", Order = 5)]
    public class BlogController : NccController
    {
        NccPostService _nccPostService;
        NccUserService _nccUserService;
        ILoggerFactory _loggerFactory;
        ILogger _logger;

        public BlogController(NccPostService nccPostService, NccUserService nccUserService, ILoggerFactory loggerFactory)
        {
            _nccPostService = nccPostService;
            _loggerFactory = loggerFactory;
            _nccUserService = nccUserService;
            _logger = _loggerFactory.CreateLogger<BlogController>();
        }

        [AllowAnonymous]
        public ActionResult Index(int page = 0)
        { 
            return View(); 
        }

        [AllowAnonymous]
        public ActionResult Details(string slug)
        {

            return View();
        }

        [AdminMenuItem(Name ="New Post", Order = 1, Url = "/Blog/CreateEdit")]
        public ActionResult CreateEdit(long id=0)
        {
            PreparePostCreateEditView();

            NccPost post = new NccPost();
            
            if (id > 0)
            {
                post = _nccPostService.Get(id);
            }
            else
            {
                post.Content = "";
                post.PublishDate = DateTime.Now;
                post.PostStatus = NccPost.NccPostStatus.Draft;
            }

            return View(post);
        }

        [HttpPost]
        public ActionResult CreateEdit(NccPost post)
        {
            if (ModelState.IsValid)
            {
                var author = _nccUserService.Get(User.GetUserId());
                post.Author = author;
                post.Status = EntityStatus.Active;
                _nccPostService.Save(post);
                TempData["SuccessMessage"] = "Post save successful";
            }

            PreparePostCreateEditView();

            return View(post);
        }

        private void PreparePostCreateEditView()
        {
            ViewBag.DomainName = (Request.IsHttps == true ? "https://" : "http://") + Request.Host + "/Blog/";
            ViewBag.Layouts = GlobalConfig.ActiveTheme.Layouts;
        }

        [AdminMenuItem(Name = "Manage", Order = 2, Url = "/Blog/Manage")]
        public ActionResult Manage()
        {
            var allPosts = _nccPostService.LoadAll().OrderByDescending(p => p.CreationDate).ToList();
            return View(allPosts);
        }

        public ActionResult Delete(long Id)
        {
            ApiResponse rsp = new ApiResponse();
            _nccPostService.Remove(Id);            
            TempData["SuccessMessage"] = "Page deleted successful";
            return RedirectToAction("Manage");
        }
    }
}

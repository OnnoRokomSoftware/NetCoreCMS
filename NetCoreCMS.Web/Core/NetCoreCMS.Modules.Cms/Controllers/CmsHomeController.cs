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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Messages;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Network;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Modules.Cms.Models.ViewModels;
using System;
using System.Linq;

namespace NetCoreCMS.Core.Modules.Cms.Controllers
{
    [AllowAnonymous]
    public class CmsHomeController : NccController
    {
        private NccPageService _pageService;
        private NccPostService _postService;
        
        public CmsHomeController(NccPageService pageService, NccPostService nccPostService, ILoggerFactory factory)
        {
            _pageService = pageService;
            _postService = nccPostService;
            _logger = factory.CreateLogger<CmsHomeController>();
        }

        public ActionResult Index(int pageNumber = 0)
        {
            if (SetupHelper.IsDbCreateComplete && SetupHelper.IsAdminCreateComplete)
            {
                var postPerPage = GlobalConfig.WebSite.PerPagePostSize;
                var totalPost = _postService.GetPublishedPostCount();
                var stickyPost = _postService.LoadSpecialPosts(true, false);
                var featuredPosts = _postService.LoadSpecialPosts(false,true);
                var allPost = _postService.LoadPublished(pageNumber, postPerPage, false, false);

                return View(new HomePageViewModel() {
                    AllPosts = allPost,
                    CurrentLanguage = CurrentLanguage,
                    FeaturedPosts = featuredPosts,
                    StickyPost = stickyPost.FirstOrDefault(),
                    PageNumber = pageNumber,
                    PostPerPage = postPerPage,
                    TotalPost = totalPost,
                    PreviousPage = pageNumber - 1,
                    NextPage = pageNumber + 1,
                    TotalPage = (int) Math.Ceiling(totalPost/(decimal)postPerPage),
                });
            }
            return Redirect("/SetupHome/Index");
        }
        
        public JsonResult RemoveGlobalMessage(string id)
        {
            GlobalMessageRegistry.UnRegisterMessage(id);
            return Json(new ApiResponse() { IsSuccess = true, Message = "Success" });
        }

        public IActionResult ResourceNotFound()
        {
            return View();
        } 
    }
}

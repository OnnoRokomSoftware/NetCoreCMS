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
using NetCoreCMS.Framework.Core.Mvc.Attributes;

namespace NetCoreCMS.Core.Modules.Blog.Controllers
{
    [SiteMenu(IconCls = "fa-newspaper-o", Name = "Blog", Order = 100)]
    [AdminMenu(IconCls = "fa-newspaper-o", Name = "Blog", Order = 1)]
    public class BlogController : NccController
    {
        NccPostService _nccPostService;
        NccUserService _nccUserService;
        ILoggerFactory _loggerFactory;

        public BlogController(NccPostService nccPostService, NccUserService nccUserService, ILoggerFactory loggerFactory)
        {
            _nccPostService = nccPostService;
            _loggerFactory = loggerFactory;
            _nccUserService = nccUserService;
            _logger = _loggerFactory.CreateLogger<BlogController>();
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Post");
        }

        [AllowAnonymous]
        public ActionResult Details(string slug)
        {
            return RedirectToAction("Index", "Post", new { slug = slug });            
        }        
    }
}
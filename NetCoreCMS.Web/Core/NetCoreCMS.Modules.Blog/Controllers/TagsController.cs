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
using Microsoft.AspNetCore.Mvc.Rendering;
using NetCoreCMS.Framework.i18n;
using NetCoreCMS.Framework.Setup;
using System.Collections.Generic;

namespace NetCoreCMS.Core.Modules.Media.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator,Editor")]
    [SiteMenu(Name = "Blog", Order = 100)]
    [AdminMenu(Name = "Blog", Order = 5)]
    public class TagsController : NccController
    {
        NccTagService _nccTagService;
        ILoggerFactory _loggerFactory;

        public TagsController(NccTagService nccTagService, ILoggerFactory loggerFactory)
        {
            _nccTagService = nccTagService;
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<CategoryController>();
        }

        #region Admin Panel
        [AdminMenuItem(Name = "Tags", Url = "/Tags/Manage", IconCls = "", Order = 7)]
        public ActionResult Manage(long Id = 0)
        {
            var itemList = _nccTagService.LoadAll(false).OrderBy(x => x.Name).ToList();
            return View(itemList);
        }

        public ActionResult StatusUpdate(long Id = 0)
        {
            if (Id > 0)
            {
                var item = _nccTagService.Get(Id);
                if (item.Status == EntityStatus.Active)
                {
                    item.Status = EntityStatus.Inactive;
                    ViewBag.Message = "In-activated successfull.";
                }
                else
                {
                    item.Status = EntityStatus.Active;
                    ViewBag.Message = "Activated successfull.";
                }

                _nccTagService.Update(item);
                ViewBag.MessageType = "SuccessMessage";
            }
            return RedirectToAction("Manage");
        }

        public ActionResult Delete(long Id)
        {
            NccTag item = _nccTagService.Get(Id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Delete(long Id, int status)
        {
            if (Id > 0)
            {
                var item = _nccTagService.Get(Id);
                item.Status = EntityStatus.Deleted;
                ViewBag.Message = "Deleted successfull.";
                _nccTagService.Update(item);
                ViewBag.MessageType = "SuccessMessage";
            }
            return RedirectToAction("Manage");
        }
        #endregion

        #region Public View
        [AllowAnonymous]
        [SiteMenuItem(Name = "Post Tags", Url = "/Tags", Order = 1)]
        public ActionResult Index(string name = "")
        {
            ViewBag.CurrentLanguage = CurrentLanguage;
            if (!string.IsNullOrEmpty(name))
            {
                var item = _nccTagService.GetWithPost(name);
                if (item != null)
                {
                    return View("Details", item);
                }
            }
            var allPost = _nccTagService.LoadAll(true).OrderByDescending(x => x.Posts.Count).ToList();
            return View(allPost);
        }
        #endregion
    }
}
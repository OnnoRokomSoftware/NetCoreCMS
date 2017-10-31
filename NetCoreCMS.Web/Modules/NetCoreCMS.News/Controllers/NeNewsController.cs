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
using NetCoreCMS.Framework.Core.Mvc.Attributes;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Modules.News.Models;
using NetCoreCMS.Modules.News.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCoreCMS.Modules.News.Controllers
{

    [Authorize(Roles = "SuperAdmin,Administrator,Editor")]
    [AdminMenu(Name = "News", IconCls = "fa-link", Order = 100)]
    [SiteMenu(Name = "News", IconCls = "fa-link", Order = 100)]
    public class NeNewsController : NccController
    {
        #region Initialization
        private NccSettingsService _nccSettingsService;
        private NeNewsService _neNewsService;
        private NeCategoryService _neCategoryService;

        private NewsSettings nccNewsSettings;

        public NeNewsController(NccSettingsService nccSettingsService, ILoggerFactory factory, NeNewsService neNewsService, NeCategoryService neCategoryService)
        {
            _logger = factory.CreateLogger<NeNewsController>();
            nccNewsSettings = new NewsSettings();

            _nccSettingsService = nccSettingsService;
            _neNewsService = neNewsService;
            _neCategoryService = neCategoryService;
            try
            {
                var tempSettings = _nccSettingsService.GetByKey("NccLinkShare_Settings");
                if (tempSettings != null)
                {
                    nccNewsSettings = JsonConvert.DeserializeObject<NewsSettings>(tempSettings.Value);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
        #endregion

        #region Admin Panel
        [AdminMenuItem(Name = "Manage News", Url = "/NeNews/Manage", IconCls = "", Order = 1)]
        public ActionResult Manage()
        {
            var itemList = _neNewsService.LoadAll().OrderByDescending(x => x.Id).ToList();
            return View(itemList);
        }


        [AdminMenuItem(Name = "New News", Url = "/NeNews/CreateEdit", IconCls = "fa-plus", Order = 2)]
        public ActionResult CreateEdit(long Id = 0)
        {
            NeNews item = new NeNews();
            ViewBag.CategoryList = _neCategoryService.LoadAll(true);

            if (Id > 0)
            {
                item = _neNewsService.Get(Id);
            }
            return View(item);
        }

        [HttpPost]
        public ActionResult CreateEdit(NeNews model, string save, long[] LsCategory)
        {
            bool isSuccess = false;
            ViewBag.MessageType = "ErrorMessage";
            ViewBag.Message = "Error occoured. Please fill up all field correctly.";

            if (ModelState.IsValid)
            {
                if (LsCategory == null || LsCategory.Count() == 0)
                {
                    ViewBag.Message = "You have to select at least one category.";
                }
                else
                {
                    model.CategoryList = new List<NeNewsCategory>();
                    foreach (var item in LsCategory)
                    {
                        model.CategoryList.Add(new NeNewsCategory() { NeNews = model, NeCategoryId = item });
                    }
                    if (model.Id > 0)
                    {
                        _neNewsService.Update(model);
                        isSuccess = true;
                        ViewBag.MessageType = "SuccessMessage";
                        ViewBag.Message = "Data updated successfull.";
                    }
                    else
                    {
                        _neNewsService.Save(model);
                        isSuccess = true;
                        ViewBag.MessageType = "SuccessMessage";
                        ViewBag.Message = "Data saved successfull.";
                    }
                }
            }

            if (isSuccess == true && save == "Save")
            {
                return RedirectToAction("Manage");
            }
            ViewBag.CategoryList = _neCategoryService.LoadAll(true);
            return View(model);
        }

        public ActionResult StatusUpdate(long Id = 0)
        {
            if (Id > 0)
            {
                var item = _neNewsService.Get(Id);
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

                _neNewsService.Update(item);
                ViewBag.MessageType = "SuccessMessage";
            }
            return RedirectToAction("Manage");
        }

        public ActionResult Delete(long Id)
        {
            NeNews item = _neNewsService.Get(Id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Delete(long Id, int status)
        {
            _neNewsService.DeletePermanently(Id);
            ViewBag.MessageType = "SuccessMessage";
            ViewBag.Message = "Item deleted successful";
            return RedirectToAction("Manage");
        }
        #endregion

        #region User Panel
        [AllowAnonymous]
        [SiteMenuItem(Name = "News", Url = "/NeNews", IconCls = "", Order = 1)]
        public ActionResult Index(string category = "", int page = 0, int count = 10)
        {            
            List<NeNews> itemList = new List<NeNews>();
            if (category.Trim() == "")
                itemList = _neNewsService.LoadAll(true).OrderByDescending(x => x.Id).Skip(page * count).Take(count).ToList();
            else
                itemList = _neNewsService.LoadAllByCategory(category, page, count);
            return View(itemList);
        }
        [AllowAnonymous]
        public ActionResult Details(long newsId)
        {            
            NeNews item = new NeNews();
            item = _neNewsService.Get(newsId);
            return View(item);
        }
        #endregion
    }
}
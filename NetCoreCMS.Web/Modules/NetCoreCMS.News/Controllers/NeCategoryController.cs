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
    public class NeCategoryController : NccController
    {
        #region Initialization
        private NccSettingsService _nccSettingsService;
        private NeCategoryService _neCategoryService;

        private NewsSettings nccNewsSettings;

        public NeCategoryController(NccSettingsService nccSettingsService, ILoggerFactory factory, NeCategoryService neCategoryService)
        {
            _logger = factory.CreateLogger<NeNewsController>();
            nccNewsSettings = new NewsSettings();

            _nccSettingsService = nccSettingsService;
            _neCategoryService = neCategoryService;
            try
            {
                var tempSettings = _nccSettingsService.GetByKey("Ncc_News_Settings");
                if (tempSettings != null)
                {
                    nccNewsSettings = JsonConvert.DeserializeObject<NewsSettings>(tempSettings.Value);
                }
            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
        #endregion

        #region Admin Panel
        [AdminMenuItem(Name = "Manage Category", Url = "/NeCategory/Manage", IconCls = "", Order = 4)]
        public ActionResult Manage()
        {
            var itemList = _neCategoryService.LoadAll().OrderByDescending(x => x.Id).ToList(); ;
            return View(itemList);
        }


        [AdminMenuItem(Name = "New Category", Url = "/NeCategory/CreateEdit", IconCls = "fa-plus", Order = 5)]
        public ActionResult CreateEdit(long Id = 0)
        {
            NeCategory item = new NeCategory();

            if (Id > 0)
            {
                item = _neCategoryService.Get(Id);
            }
            return View(item);
        }

        [HttpPost]
        public ActionResult CreateEdit(NeCategory model, string save)
        {
            bool isSuccess = false;
            ViewBag.MessageType = "ErrorMessage";
            ViewBag.Message = "Error occoured. Please fill up all field correctly.";

            if (ModelState.IsValid)
            {
                //unique name check
                model.Name = model.Name.Trim();
                var itemCount = _neCategoryService.LoadAll(true, 0, model.Name).Where(x => x.Id != model.Id).ToList().Count();
                if (itemCount > 0)
                {
                    ViewBag.Message = "Duplicate name found.";
                }
                else
                {
                    if (model.Id > 0)
                    {
                        _neCategoryService.Update(model);
                        isSuccess = true;
                        ViewBag.MessageType = "SuccessMessage";
                        ViewBag.Message = "Data updated successfull.";
                    }
                    else
                    {
                        _neCategoryService.Save(model);
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
            return View(model);
        }

        public ActionResult StatusUpdate(long Id = 0)
        {
            if (Id > 0)
            {
                var item = _neCategoryService.Get(Id);
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

                _neCategoryService.Update(item);
                ViewBag.MessageType = "SuccessMessage";
            }
            return RedirectToAction("Manage");
        }

        public ActionResult Delete(long Id)
        {
            NeCategory item = _neCategoryService.Get(Id);            
            return View(item);
        }

        [HttpPost]
        public ActionResult Delete(long Id, int status)
        {
            _neCategoryService.DeletePermanently(Id);
            ViewBag.MessageType = "SuccessMessage";
            ViewBag.Message = "Item deleted successful";
            return RedirectToAction("Manage");
        }
        #endregion

        #region User Panel
        #endregion
    }
}
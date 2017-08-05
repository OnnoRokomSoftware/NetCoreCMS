using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.LinkShare.Models;
using NetCoreCMS.LinkShare.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCoreCMS.LinkShare.Controllers
{

    [Authorize(Roles = "SuperAdmin,Administrator,Editor")]
    [AdminMenu(Name = "Link Share", IconCls = "fa-link", Order = 100)]
    public class LsLinkController : NccController
    {
        #region Initialization
        private NccSettingsService _nccSettingsService;
        private LsLinkService _lsLinkService;
        private LsCategoryService _lsCategoryService;

        private LinkShareSettings nccLinkShareSettings;

        public LsLinkController(NccSettingsService nccSettingsService, ILoggerFactory factory, LsLinkService lsLinkService, LsCategoryService lsCategoryService)
        {
            _logger = factory.CreateLogger<LsLinkController>();
            nccLinkShareSettings = new LinkShareSettings();

            _nccSettingsService = nccSettingsService;
            _lsLinkService = lsLinkService;
            _lsCategoryService = lsCategoryService;
            try
            {
                var tempSettings = _nccSettingsService.GetByKey("NccLinkShare_Settings");
                if (tempSettings != null)
                {
                    nccLinkShareSettings = JsonConvert.DeserializeObject<LinkShareSettings>(tempSettings.Value);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
        #endregion

        #region Admin Panel
        [AdminMenuItem(Name = "Manage Links", Url = "/LsLink/Manage", IconCls = "", Order = 2)]
        public ActionResult Manage()
        {
            var itemList = _lsLinkService.LoadAll().OrderByDescending(x => x.Id).ToList(); ;
            return View(itemList);
        }


        [AdminMenuItem(Name = "New Link", Url = "/LsLink/CreateEdit", IconCls = "fa-plus", Order = 1)]
        public ActionResult CreateEdit(long Id = 0)
        {
            LsLink item = new LsLink();
            ViewBag.LsCategoryList = _lsCategoryService.LoadAllActive();

            if (Id > 0)
            {
                item = _lsLinkService.Get(Id);
            }
            return View(item);
        }

        [HttpPost]
        public ActionResult CreateEdit(LsLink model, string save, long[] LsCategory)
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
                    model.Categories = new List<LsLinkCategory>();
                    foreach (var item in LsCategory)
                    {
                        model.Categories.Add(new LsLinkCategory() { LsLink = model, LsCategoryId = item });
                    }
                    if (model.Id > 0)
                    {
                        _lsLinkService.Update(model);
                        isSuccess = true;
                        ViewBag.MessageType = "SuccessMessage";
                        ViewBag.Message = "Data updated successfull.";
                    }
                    else
                    {
                        _lsLinkService.Save(model);
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
            ViewBag.LsCategoryList = _lsCategoryService.LoadAllActive();
            return View(model);
        }

        public ActionResult StatusUpdate(long Id = 0)
        {
            if (Id > 0)
            {
                var item = _lsLinkService.Get(Id);
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

                _lsLinkService.Update(item);
                ViewBag.MessageType = "SuccessMessage";
            }
            return RedirectToAction("Manage");
        }

        public ActionResult Delete(long Id)
        {
            LsLink item = _lsLinkService.Get(Id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Delete(long Id, int status)
        {
            _lsLinkService.DeletePermanently(Id);
            ViewBag.MessageType = "SuccessMessage";
            ViewBag.Message = "Item deleted successful";
            return RedirectToAction("Manage");
        }
        #endregion

        #region User Panel
        #endregion
    }
}
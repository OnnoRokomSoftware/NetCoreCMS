using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
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
    public class LinkShareCategoryController : NccController
    {
        #region Initialization
        private NccSettingsService _nccSettingsService;
        private NccLinkShareCategoryService _nccLinkShareCategoryService;

        private NccLinkShareSettings nccLinkShareSettings;

        public LinkShareCategoryController(NccSettingsService nccSettingsService, ILoggerFactory factory, NccLinkShareCategoryService nccLinkShareCategoryService)
        {
            _logger = factory.CreateLogger<LinkShareHomeController>();
            nccLinkShareSettings = new NccLinkShareSettings();

            _nccSettingsService = nccSettingsService;
            _nccLinkShareCategoryService = nccLinkShareCategoryService;
            try
            {
                var tempSettings = _nccSettingsService.GetByKey("NccLinkShare_Settings");
                if (tempSettings != null)
                {
                    nccLinkShareSettings = JsonConvert.DeserializeObject<NccLinkShareSettings>(tempSettings.Value);
                }
            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
        #endregion

        #region Admin Panel
        [AdminMenuItem(Name = "Manage Category", Url = "/LinkShareCategory/Manage", IconCls = "", Order = 5)]
        public ActionResult Manage()
        {
            var itemList = _nccLinkShareCategoryService.LoadAll().OrderByDescending(x => x.Id).ToList(); ;
            return View(itemList);
        }


        [AdminMenuItem(Name = "New Category", Url = "/LinkShareCategory/CreateEdit", IconCls = "fa-plus", Order = 4)]
        public ActionResult CreateEdit(long Id = 0)
        {
            NccCategory item = new NccCategory();

            if (Id > 0)
            {
                item = _nccLinkShareCategoryService.Get(Id);
            }
            return View(item);
        }

        [HttpPost]
        public ActionResult CreateEdit(NccCategory model, string save)
        {
            bool isSuccess = false;
            ViewBag.MessageType = "ErrorMessage";
            ViewBag.Message = "Error occoured. Please fill up all field correctly.";

            if (ModelState.IsValid)
            {
                //unique name check
                model.Name = model.Name.Trim();
                var itemCount = _nccLinkShareCategoryService.LoadAllByName(model.Name).Where(x => x.Id != model.Id).ToList().Count();
                if (itemCount > 0)
                {
                    ViewBag.Message = "Duplicate name found.";
                }
                else
                {
                    if (model.Id > 0)
                    {
                        _nccLinkShareCategoryService.Update(model);
                        isSuccess = true;
                        ViewBag.MessageType = "SuccessMessage";
                        ViewBag.Message = "Data updated successfull.";
                    }
                    else
                    {
                        _nccLinkShareCategoryService.Save(model);
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

        public ActionResult Delete(long Id)
        {
            NccCategory item = _nccLinkShareCategoryService.Get(Id);            
            return View(item);
        }

        [HttpPost]
        public ActionResult Delete(long Id, int status)
        {
            _nccLinkShareCategoryService.DeletePermanently(Id);
            ViewBag.MessageType = "SuccessMessage";
            ViewBag.Message = "Item deleted successful";
            return RedirectToAction("Manage");
        }
        #endregion

        #region User Panel
        #endregion
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.MatGallery.Models;
using NetCoreCMS.MatGallery.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCoreCMS.MatGallery.Controllers
{

    [Authorize(Roles = "SuperAdmin,Administrator,Editor")]
    [AdminMenu(Name = "Gallery Management", IconCls = "", Order = 100)]
    public class MatGalleryController : NccController
    {
        #region Initialization
        private NccSettingsService _nccSettingsService;
        private NccUserModuleService _nccUserModuleService;

        private Settings settings;

        public MatGalleryController(NccSettingsService nccSettingsService, ILoggerFactory factory, NccUserModuleService nccUserModuleService)
        {
            _logger = factory.CreateLogger<MatGalleryController>();
            settings = new Settings();

            _nccSettingsService = nccSettingsService;
            _nccUserModuleService = nccUserModuleService;
            try
            {
                var tempSettings = _nccSettingsService.GetByKey("Ncc_MatGallery_Settings");
                if (tempSettings != null)
                {
                    settings = JsonConvert.DeserializeObject<Settings>(tempSettings.Value);
                }
            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
        #endregion

        #region Admin Panel
        [AdminMenuItem(Name = "Module Manage", Url = "/MatGallery/Manage", IconCls = "", Order = 1)]
        public ActionResult Manage()
        {
            var itemList = _nccUserModuleService.LoadAll().OrderByDescending(x => x.Id).ToList(); ;
            return View(itemList);
        }
        
        public ActionResult CreateEdit(long Id = 0)
        {
            NccUserModule item = new NccUserModule();

            if (Id > 0)
            {
                item = _nccUserModuleService.Get(Id);
            }
            return View(item);
        }

        [HttpPost]
        public ActionResult CreateEdit(NccUserModule model, string save, string[] itemPath, string[] description)
        {
            bool isSuccess = false;
            ViewBag.MessageType = "ErrorMessage";
            ViewBag.Message = "Error occoured. Please fill up all field correctly.";

            if (ModelState.IsValid)
            {
                //unique name check
                model.Name = model.Name.Trim();
                var itemCount = _nccUserModuleService.LoadAllByName(model.Name).Where(x => x.Id != model.Id).ToList().Count();
                if (itemCount > 0)
                {
                    ViewBag.Message = "Duplicate name found.";
                }
                else
                {
                    if (model.Id > 0)
                    {
                        _nccUserModuleService.Update(model);
                        isSuccess = true;
                        ViewBag.MessageType = "SuccessMessage";
                        ViewBag.Message = "Data updated successfull.";
                    }
                    else
                    {
                        _nccUserModuleService.Save(model);
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
            NccUserModule item = _nccUserModuleService.Get(Id);            
            return View(item);
        }

        [HttpPost]
        public ActionResult Delete(long Id, int status)
        {
            _nccUserModuleService.DeletePermanently(Id);
            ViewBag.MessageType = "SuccessMessage";
            ViewBag.Message = "Item deleted successful";
            return RedirectToAction("Manage");
        }
        #endregion

        #region User Panel
        #endregion
    }
}
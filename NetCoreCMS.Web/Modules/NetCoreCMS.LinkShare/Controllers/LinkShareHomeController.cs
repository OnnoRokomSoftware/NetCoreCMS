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
    [AdminMenu(Name = "Link Share", IconCls = "", Order = 100)]
    public class LinkShareHomeController : NccController
    {
        #region Initialization
        private NccSettingsService _nccSettingsService;
        private NccLinkShareService _nccLinkShareService;

        private NccLinkShareSettings nccLinkShareSettings;

        public LinkShareHomeController(NccSettingsService nccSettingsService, ILoggerFactory factory, NccLinkShareService nccImageSliderService)
        {
            _logger = factory.CreateLogger<LinkShareHomeController>();
            nccLinkShareSettings = new NccLinkShareSettings();

            _nccSettingsService = nccSettingsService;
            _nccLinkShareService = nccImageSliderService;
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
        [AdminMenuItem(Name = "Manage", Url = "/LinkShareHome/Manage", IconCls = "", Order = 2)]
        public ActionResult Manage()
        {
            var itemList = _nccLinkShareService.LoadAll().OrderByDescending(x => x.Id).ToList(); ;
            return View(itemList);
        }


        [AdminMenuItem(Name = "New", Url = "/LinkShareHome/CreateEdit", Order = 1)]
        public ActionResult CreateEdit(long Id = 0)
        {
            NccLinkShare item = new NccLinkShare();

            if (Id > 0)
            {
                item = _nccLinkShareService.Get(Id);
            }
            return View(item);
        }

        [HttpPost]
        public ActionResult CreateEdit(NccLinkShare model, string save, string[] itemPath, string[] description)
        {
            bool isSuccess = false;
            ViewBag.MessageType = "ErrorMessage";
            ViewBag.Message = "Error occoured. Please fill up all field correctly.";

            if (ModelState.IsValid)
            {
                //unique name check
                model.Name = model.Name.Trim();
                var itemCount = _nccLinkShareService.LoadAllByName(model.Name).Where(x => x.Id != model.Id).ToList().Count();
                if (itemCount > 0)
                {
                    ViewBag.Message = "Duplicate name found.";
                }
                else
                {
                    //for (int i = 0; i < itemPath.Count(); i++)
                    //{
                    //    var tempItemPath = string.IsNullOrEmpty(itemPath[i]) ? "" : itemPath[i];
                    //    var tempDescription = string.IsNullOrEmpty(description[i]) ? "" : description[i];
                    //}

                    if (model.Id > 0)
                    {
                        _nccLinkShareService.Update(model);
                        isSuccess = true;
                        ViewBag.MessageType = "SuccessMessage";
                        ViewBag.Message = "Data updated successfull.";
                    }
                    else
                    {
                        _nccLinkShareService.Save(model);
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
            NccLinkShare item = _nccLinkShareService.Get(Id);            
            return View(item);
        }

        [HttpPost]
        public ActionResult Delete(long Id, int status)
        {
            _nccLinkShareService.DeletePermanently(Id);
            ViewBag.MessageType = "SuccessMessage";
            ViewBag.Message = "Item deleted successful";
            return RedirectToAction("Manage");
        }
        #endregion

        #region User Panel
        #endregion
    }
}
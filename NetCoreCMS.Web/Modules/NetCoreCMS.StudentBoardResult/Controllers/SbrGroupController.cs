using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Modules.StudentBoardResult.Models;
using NetCoreCMS.Modules.StudentBoardResult.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCoreCMS.Modules.StudentBoardResult.Controllers
{

    [Authorize(Roles = "SuperAdmin,Administrator,Editor")]
    [AdminMenu(Name = "Board Result", IconCls = "fa-database", Order = 110)]
    public class SbrGroupController : NccController
    {
        #region Initialization
        private NccSettingsService _nccSettingsService;
        private SbrGroupService _sbrGroupService;

        private SbrSettings sbrSettings;

        public SbrGroupController(NccSettingsService nccSettingsService, ILoggerFactory factory, SbrGroupService sbrGroupService)
        {
            _logger = factory.CreateLogger<SbrGroupController>();
            sbrSettings = new SbrSettings();

            _nccSettingsService = nccSettingsService;
            _sbrGroupService = sbrGroupService;
            try
            {
                var tempSettings = _nccSettingsService.GetByKey("Ncc_Sbr_Settings");
                if (tempSettings != null)
                {
                    sbrSettings = JsonConvert.DeserializeObject<SbrSettings>(tempSettings.Value);
                }
            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
        #endregion

        #region Admin Panel
        [AdminMenuItem(Name = "Group", Url = "/SbrGroup/Manage", IconCls = "fa-arrow-right", Order = 12)]
        public ActionResult Manage()
        {
            var itemList = _sbrGroupService.LoadAll().OrderBy(x => x.Order).ThenByDescending(x => x.Id).ToList(); ;
            return View(itemList);
        }
        
        public ActionResult CreateEdit(long Id = 0)
        {
            SbrGroup item = new SbrGroup();

            if (Id > 0)
            {
                item = _sbrGroupService.Get(Id);
            }
            return View(item);
        }

        [HttpPost]
        public ActionResult CreateEdit(SbrGroup model, string save)
        {
            bool isSuccess = false;
            ViewBag.MessageType = "ErrorMessage";
            ViewBag.Message = "Error occoured. Please fill up all field correctly.";

            if (ModelState.IsValid)
            {
                //unique name check
                model.Name = model.Name.Trim();
                var itemCount = _sbrGroupService.LoadAll(true, 0, model.Name).Where(x => x.Id != model.Id).ToList().Count();
                if (itemCount > 0)
                {
                    ViewBag.Message = "Duplicate name found.";
                }
                else
                {
                    if (model.Id > 0)
                    {
                        _sbrGroupService.Update(model);
                        isSuccess = true;
                        ViewBag.MessageType = "SuccessMessage";
                        ViewBag.Message = "Data updated successfull.";
                    }
                    else
                    {
                        _sbrGroupService.Save(model);
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
                var item = _sbrGroupService.Get(Id);
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

                _sbrGroupService.Update(item);
                ViewBag.MessageType = "SuccessMessage";
            }
            return RedirectToAction("Manage");
        }

        public ActionResult Delete(long Id)
        {
            SbrGroup item = _sbrGroupService.Get(Id);            
            return View(item);
        }

        [HttpPost]
        public ActionResult Delete(long Id, int status)
        {
            _sbrGroupService.DeletePermanently(Id);
            ViewBag.MessageType = "SuccessMessage";
            ViewBag.Message = "Item deleted successful";
            return RedirectToAction("Manage");
        }
        #endregion

        #region User Panel
        #endregion
    }
}
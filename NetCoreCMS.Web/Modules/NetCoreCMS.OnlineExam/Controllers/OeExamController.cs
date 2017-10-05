using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Modules.OnlineExam.Models;
using NetCoreCMS.Modules.OnlineExam.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCoreCMS.Modules.OnlineExam.Controllers
{

    [Authorize(Roles = "SuperAdmin,Administrator,Editor")]
    [AdminMenu(Name = "Online Exam", IconCls = "fa-link", Order = 100)]
    public class OeExamController : NccController
    {
        #region Initialization
        private NccSettingsService _nccSettingsService;
        private OeExamService _oeExamService;

        private OnlineExamSettings nccOnlineExamSettings;

        public OeExamController(NccSettingsService nccSettingsService, ILoggerFactory factory, OeExamService oeExamService)
        {
            _logger = factory.CreateLogger<OeExamController>();
            nccOnlineExamSettings = new OnlineExamSettings();

            _nccSettingsService = nccSettingsService;
            _oeExamService = oeExamService;
            try
            {
                var tempSettings = _nccSettingsService.GetByKey("Ncc_OnlineExam_Settings");
                if (tempSettings != null)
                {
                    nccOnlineExamSettings = JsonConvert.DeserializeObject<OnlineExamSettings>(tempSettings.Value);
                }
            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
        #endregion

        #region Admin Panel
        [AdminMenuItem(Name = "Manage Exam", Url = "/OeExam/Manage", IconCls = "", Order = 1)]
        public ActionResult Manage()
        {
            var itemList = _oeExamService.LoadAll(false).OrderByDescending(x => x.Id).ToList();
            return View(itemList);
        }


        //[AdminMenuItem(Name = "New Exam", Url = "/OeExam/CreateEdit", IconCls = "fa-plus", Order = 2)]
        public ActionResult CreateEdit(long Id = 0)
        {
            OeExam item = new OeExam();

            if (Id > 0)
            {
                item = _oeExamService.Get(Id);
            }
            return View(item);
        }

        [HttpPost]
        public ActionResult CreateEdit(OeExam model, string save)
        {
            bool isSuccess = false;
            ViewBag.MessageType = "ErrorMessage";
            ViewBag.Message = "Error occoured. Please fill up all field correctly.";

            if (ModelState.IsValid)
            {
                //unique name check
                model.Name = model.Name.Trim();
                var itemCount = _oeExamService.LoadAll(true, 0, model.Name).Where(x => x.Id != model.Id).ToList().Count();
                if (itemCount > 0)
                {
                    ViewBag.Message = "Duplicate name found.";
                }
                else
                {
                    if (model.Id > 0)
                    {
                        _oeExamService.Update(model);
                        isSuccess = true;
                        ViewBag.MessageType = "SuccessMessage";
                        ViewBag.Message = "Data updated successfull.";
                    }
                    else
                    {
                        _oeExamService.Save(model);
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
            else if(isSuccess == true)
            {
                model = new OeExam();
            }
            return View(model);
        }

        public ActionResult StatusUpdate(long Id = 0)
        {
            if (Id > 0)
            {
                var item = _oeExamService.Get(Id);
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

                _oeExamService.Update(item);
                ViewBag.MessageType = "SuccessMessage";
            }
            return RedirectToAction("Manage");
        }

        public ActionResult Delete(long Id)
        {
            OeExam item = _oeExamService.Get(Id);            
            return View(item);
        }

        [HttpPost]
        public ActionResult Delete(long Id, int status)
        {
            _oeExamService.DeletePermanently(Id);
            ViewBag.MessageType = "SuccessMessage";
            ViewBag.Message = "Item deleted successful";
            return RedirectToAction("Manage");
        }
        #endregion

        #region User Panel
        #endregion
    }
}
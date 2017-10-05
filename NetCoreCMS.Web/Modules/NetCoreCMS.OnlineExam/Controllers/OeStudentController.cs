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
    public class OeStudentController : NccController
    {
        #region Initialization
        private NccSettingsService _nccSettingsService;
        private OeStudentService _oeStudentService;

        private OnlineExamSettings nccOnlineExamSettings;

        public OeStudentController(NccSettingsService nccSettingsService, ILoggerFactory factory, OeStudentService oeStudentService)
        {
            _logger = factory.CreateLogger<OeSubjectController>();
            nccOnlineExamSettings = new OnlineExamSettings();

            _nccSettingsService = nccSettingsService;
            _oeStudentService = oeStudentService;
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
        [AdminMenuItem(Name = "Manage Student", Url = "/OeStudent/Manage", IconCls = "fa-users", Order = 7)]
        public ActionResult Manage()
        {
            var itemList = _oeStudentService.LoadAll(false).OrderByDescending(x => x.Id).ToList(); ;
            return View(itemList);
        }


        //[AdminMenuItem(Name = "New Subject", Url = "/OeSubject/CreateEdit", IconCls = "fa-plus", Order = 0)]
        public ActionResult CreateEdit(long Id = 0)
        {
            OeStudent item = new OeStudent();

            if (Id > 0)
            {
                item = _oeStudentService.Get(Id);
            }
            return View(item);
        }

        [HttpPost]
        public ActionResult CreateEdit(OeStudent model, string save)
        {
            bool isSuccess = false;
            ViewBag.MessageType = "ErrorMessage";
            ViewBag.Message = "Error occoured. Please fill up all field correctly.";

            if (ModelState.IsValid)
            {
                //unique name check
                model.PrnNo = model.PrnNo.Trim();
                var itemCount = _oeStudentService.LoadAll().Where(x => x.Id != model.Id && x.PrnNo==model.PrnNo).ToList().Count();
                if (itemCount > 0)
                {
                    ViewBag.Message = "Duplicate Roll number found.";
                }
                else
                {
                    if (model.Id > 0)
                    {
                        _oeStudentService.Update(model);
                        isSuccess = true;
                        ViewBag.MessageType = "SuccessMessage";
                        ViewBag.Message = "Data updated successfull.";
                    }
                    else
                    {
                        _oeStudentService.Save(model);
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
                var item = _oeStudentService.Get(Id);
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

                _oeStudentService.Update(item);
                ViewBag.MessageType = "SuccessMessage";
            }
            return RedirectToAction("Manage");
        }

        public ActionResult Delete(long Id)
        {
            OeStudent item = _oeStudentService.Get(Id);            
            return View(item);
        }

        [HttpPost]
        public ActionResult Delete(long Id, int status)
        {
            _oeStudentService.DeletePermanently(Id);
            ViewBag.MessageType = "SuccessMessage";
            ViewBag.Message = "Item deleted successful";
            return RedirectToAction("Manage");
        }
        #endregion

        #region User Panel
        #endregion
    }
}
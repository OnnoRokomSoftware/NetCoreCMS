using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class SbrExamSubjectController : NccController
    {
        #region Initialization
        private NccSettingsService _nccSettingsService;
        private SbrExamService _sbrExamService;
        private SbrExamSubjectService _sbrExamSubjectService;

        private SbrSettings sbrSettings;

        public SbrExamSubjectController(NccSettingsService nccSettingsService, ILoggerFactory factory, SbrExamService sbrExamService, SbrExamSubjectService sbrExamSubjectService)
        {
            _logger = factory.CreateLogger<SbrExamSubjectController>();
            sbrSettings = new SbrSettings();

            _nccSettingsService = nccSettingsService;
            _sbrExamService = sbrExamService;
            _sbrExamSubjectService = sbrExamSubjectService;
            try
            {
                var tempSettings = _nccSettingsService.GetByKey("Ncc_Sbr_Settings");
                if (tempSettings != null)
                {
                    sbrSettings = JsonConvert.DeserializeObject<SbrSettings>(tempSettings.Value);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
        #endregion

        #region Admin Panel
        [AdminMenuItem(Name = "Exam Subject", Url = "/SbrExamSubject/Manage", IconCls = "fa-arrow-right", Order = 14)]
        public ActionResult Manage()
        {
            var itemList = _sbrExamSubjectService.LoadAll().OrderBy(x => x.Order).ThenByDescending(x => x.Id).ToList(); ;
            return View(itemList);
        }

        public ActionResult CreateEdit(long Id = 0)
        {
            ViewBag.ExamList = new SelectList(_sbrExamService.LoadAll(true), "Id", "Name", 0);
            SbrExamSubject item = new SbrExamSubject();

            if (Id > 0)
            {
                item = _sbrExamSubjectService.Get(Id);
                ViewBag.ExamList = new SelectList(_sbrExamService.LoadAll(true), "Id", "Name", item.Exam.Id);
            }
            return View(item);
        }

        [HttpPost]
        public ActionResult CreateEdit(SbrExamSubject model, long examId, string save)
        {
            bool isSuccess = false;
            ViewBag.MessageType = "ErrorMessage";
            ViewBag.Message = "Error occoured. Please fill up all field correctly.";
            if (ModelState.IsValid)
            {
                //unique name check                
                model.Name = model.Name.Trim();
                var itemCount = _sbrExamSubjectService.LoadAll(true, 0, model.Name).Where(x => x.Id != model.Id).ToList().Count();
                if (itemCount > 0)
                {
                    ViewBag.Message = "Duplicate name found.";
                }
                else if (examId == 0)
                {
                    ViewBag.Message = "Please select an exam.";
                }
                else
                {
                    model.Exam = _sbrExamService.Get(examId);
                    if (model.Id > 0)
                    {
                        _sbrExamSubjectService.Update(model);
                        isSuccess = true;
                        ViewBag.MessageType = "SuccessMessage";
                        ViewBag.Message = "Data updated successfull.";
                    }
                    else
                    {
                        _sbrExamSubjectService.Save(model);
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
            ViewBag.ExamList = new SelectList(_sbrExamService.LoadAll(true), "Id", "Name", examId);
            return View(model);
        }

        public ActionResult StatusUpdate(long Id = 0)
        {
            if (Id > 0)
            {
                var item = _sbrExamSubjectService.Get(Id);
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

                _sbrExamSubjectService.Update(item);
                ViewBag.MessageType = "SuccessMessage";
            }
            return RedirectToAction("Manage");
        }

        public ActionResult Delete(long Id)
        {
            SbrExamSubject item = _sbrExamSubjectService.Get(Id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Delete(long Id, int status)
        {
            _sbrExamSubjectService.DeletePermanently(Id);
            ViewBag.MessageType = "SuccessMessage";
            ViewBag.Message = "Item deleted successful";
            return RedirectToAction("Manage");
        }
        #endregion

        #region User Panel
        #endregion
    }
}
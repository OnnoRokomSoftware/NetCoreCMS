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
    public class OeSubjectController : NccController
    {
        #region Initialization
        private NccSettingsService _nccSettingsService;
        private OeSubjectService _oeSubjectService;

        private OnlineExamSettings nccOnlineExamSettings;

        public OeSubjectController(NccSettingsService nccSettingsService, ILoggerFactory factory, OeSubjectService oeSubjectService)
        {
            _logger = factory.CreateLogger<OeSubjectController>();
            nccOnlineExamSettings = new OnlineExamSettings();

            _nccSettingsService = nccSettingsService;
            _oeSubjectService = oeSubjectService;
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
        [AdminMenuItem(Name = "Manage Subject", Url = "/OeSubject/Manage", IconCls = "fa-gear", Order = 9)]
        public ActionResult Manage()
        {
            var itemList = _oeSubjectService.LoadAll(false).OrderByDescending(x => x.Id).ToList(); ;
            return View(itemList);
        }


        //[AdminMenuItem(Name = "New Subject", Url = "/OeSubject/CreateEdit", IconCls = "fa-plus", Order = 0)]
        public ActionResult CreateEdit(long Id = 0)
        {
            OeSubject item = new OeSubject();

            if (Id > 0)
            {
                item = _oeSubjectService.Get(Id);
            }
            return View(item);
        }

        [HttpPost]
        public ActionResult CreateEdit(OeSubject model, string save)
        {
            bool isSuccess = false;
            ViewBag.MessageType = "ErrorMessage";
            ViewBag.Message = "Error occoured. Please fill up all field correctly.";

            if (ModelState.IsValid)
            {
                //unique name check
                model.Name = model.Name.Trim();
                var itemCount = _oeSubjectService.LoadAll(true, 0, model.Name).Where(x => x.Id != model.Id).ToList().Count();
                if (itemCount > 0)
                {
                    ViewBag.Message = "Duplicate name found.";
                }
                else
                {
                    if (model.Id > 0)
                    {
                        _oeSubjectService.Update(model);
                        isSuccess = true;
                        ViewBag.MessageType = "SuccessMessage";
                        ViewBag.Message = "Data updated successfull.";
                    }
                    else
                    {
                        _oeSubjectService.Save(model);
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
                var item = _oeSubjectService.Get(Id);
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

                _oeSubjectService.Update(item);
                ViewBag.MessageType = "SuccessMessage";
            }
            return RedirectToAction("Manage");
        }

        public ActionResult Delete(long Id)
        {
            OeSubject item = _oeSubjectService.Get(Id);            
            return View(item);
        }

        [HttpPost]
        public ActionResult Delete(long Id, int status)
        {
            _oeSubjectService.DeletePermanently(Id);
            ViewBag.MessageType = "SuccessMessage";
            ViewBag.Message = "Item deleted successful";
            return RedirectToAction("Manage");
        }
        #endregion

        #region User Panel
        #endregion
    }
}
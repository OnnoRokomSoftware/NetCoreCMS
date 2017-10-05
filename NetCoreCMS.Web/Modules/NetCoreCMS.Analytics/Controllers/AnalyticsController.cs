using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Modules.Analytics.Models;
using NetCoreCMS.Modules.Analytics.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCoreCMS.Modules.Analytics.Controllers
{

    [Authorize(Roles = "SuperAdmin,Administrator,Editor")]
    [AdminMenu(Name = "Analytics", IconCls = "fa-link", Order = 100)]
    public class AnalyticsController : NccController
    {
        #region Initialization
        private NccSettingsService _nccSettingsService;
        private AnalyticsService _analyticsService;
        private AnalyticsLogService _analyticsLogService;

        private AnalyticsSettings nccLinkShareSettings;

        public AnalyticsController(NccSettingsService nccSettingsService, ILoggerFactory factory, AnalyticsService analyticsService, AnalyticsLogService analyticsLogService)
        {
            _logger = factory.CreateLogger<AnalyticsController>();
            nccLinkShareSettings = new AnalyticsSettings();

            _nccSettingsService = nccSettingsService;
            _analyticsService = analyticsService;
            _analyticsLogService = analyticsLogService;
            try
            {
                var tempSettings = _nccSettingsService.GetByKey("Ncc_Analytics_Settings");
                if (tempSettings != null)
                {
                    nccLinkShareSettings = JsonConvert.DeserializeObject<AnalyticsSettings>(tempSettings.Value);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
        #endregion

        #region Admin Panel
        [AdminMenuItem(Name = "Manage Analytics", Url = "/Analytics/Manage", IconCls = "", Order = 5)]
        public ActionResult Manage()
        {
            //var itemList = _analyticsService.LoadAll().OrderByDescending(x => x.Id).ToList();
            var itemList = _analyticsService.LoadAllWithCount().OrderByDescending(x => x.TotalLogLast24).ToList();

            return View(itemList);
        }

        public ActionResult LogDetails(long Id = 0)
        {
            var itemList = _analyticsLogService.LoadAll().Where(x => x.AnalyticsModel.Id == Id).OrderByDescending(x => x.Id).Take(100).ToList();
            return View(itemList);
        }

        [AdminMenuItem(Name = "New Analytics", Url = "/Analytics/CreateEdit", IconCls = "fa-plus", Order = 4)]
        public ActionResult CreateEdit(long Id = 0)
        {
            AnalyticsModel item = new AnalyticsModel();

            if (Id > 0)
            {
                item = _analyticsService.Get(Id);
            }
            return View(item);
        }

        [HttpPost]
        public ActionResult CreateEdit(AnalyticsModel model, string save)
        {
            bool isSuccess = false;
            ViewBag.MessageType = "ErrorMessage";
            ViewBag.Message = "Error occoured. Please fill up all field correctly.";

            if (ModelState.IsValid)
            {
                //unique name check
                model.Name = model.Name.Trim();
                var itemCount = _analyticsService.LoadAll(true,0,model.Name).Where(x => x.Id != model.Id).ToList().Count();
                if (itemCount > 0)
                {
                    ViewBag.Message = "Duplicate name found.";
                }
                else
                {
                    if (model.Id > 0)
                    {
                        _analyticsService.Update(model);
                        isSuccess = true;
                        ViewBag.MessageType = "SuccessMessage";
                        ViewBag.Message = "Data updated successfull.";
                    }
                    else
                    {
                        _analyticsService.Save(model);
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
                var item = _analyticsService.Get(Id);
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

                _analyticsService.Update(item);
                ViewBag.MessageType = "SuccessMessage";
            }
            return RedirectToAction("Manage");
        }

        public ActionResult Delete(long Id)
        {
            AnalyticsModel item = _analyticsService.Get(Id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Delete(long Id, int status)
        {
            _analyticsService.DeletePermanently(Id);
            ViewBag.MessageType = "SuccessMessage";
            ViewBag.Message = "Item deleted successful";
            return RedirectToAction("Manage");
        }
        #endregion

        #region User Panel
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index(string key = "", string referrer = "")
        {
            var analyticsModel = _analyticsService.GetByKey(key);
            if (analyticsModel != null)
            {
                AnalyticsLogModel aLog = new AnalyticsLogModel();
                aLog.Name = key;
                aLog.AnalyticsModel = analyticsModel;
                aLog.BrowserAgent = Request.Headers["User-Agent"];
                aLog.ReferrerUrl = referrer;
                aLog.VisitUrl = Request.Headers["Referer"].ToString();
                aLog.IpAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();

                _analyticsLogService.Save(aLog);
                return Content("Done");
            }

            return Content("Un-Authoraizad");
        }


        #endregion
    }
}
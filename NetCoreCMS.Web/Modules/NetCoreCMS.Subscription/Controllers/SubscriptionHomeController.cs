/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.i18n;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Modules.Subscription;
using NetCoreCMS.Modules.Subscription.Models;
using Newtonsoft.Json;
using System.Linq;
using NetCoreCMS.Framework.Core.Network;
using Microsoft.AspNetCore.Authorization;

namespace NetCoreCMS.Subscription.Controllers
{
    [AdminMenu(Name = "Subscription", Order = 100)]
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class SubscriptionHomeController : NccController
    {
        #region Initialization
        private NccSettingsService _nccSettingsService;
        private SubscriptionUserService _subscriptionUserService;

        private SubscriptionSettings nccSubscriptionSettings;

        public SubscriptionHomeController(NccSettingsService nccSettingsService, ILoggerFactory factory, SubscriptionUserService subscriptionUserService)
        {
            _logger = factory.CreateLogger<SubscriptionHomeController>();
            nccSubscriptionSettings = new SubscriptionSettings();

            _nccSettingsService = nccSettingsService;
            _subscriptionUserService = subscriptionUserService;
            try
            {
                var tempSettings = _nccSettingsService.GetByKey("NccSubscription_Settings");
                if (tempSettings != null)
                {
                    nccSubscriptionSettings = JsonConvert.DeserializeObject<SubscriptionSettings>(tempSettings.Value);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
        #endregion

        [AdminMenuItem(Name = "Index", Url = "/SubscriptionHome/Index", Order = 1)]
        public ActionResult Index()
        {
            var nccTranslator = new NccTranslator<SubscriptionHomeController>(CurrentLanguage);
            var itemList = _subscriptionUserService.LoadAll().OrderByDescending(x => x.Id).ToList();
            return View(itemList);
        }

        [HttpPost]
        public JsonResult CreateSubscription(string email, string name, string mobile, string remarks)
        {
            ApiResponse apiResponse = new ApiResponse();
            apiResponse.IsSuccess = true;
            apiResponse.Message = "Thank you for subscription.";
            if (string.IsNullOrEmpty(email))
            {
                apiResponse.IsSuccess = false;
                apiResponse.Message = "Email field is required.";
            }
            else
            {
                try
                {
                    var addr = new System.Net.Mail.MailAddress(email);
                }
                catch
                {
                    apiResponse.IsSuccess = false;
                    apiResponse.Message = "Please enter a valid Email Address.";
                }
            }

            if (apiResponse.IsSuccess)
            {
                SubscriptionUser model = new SubscriptionUser();
                model.Name = name?.Trim();
                model.Email = email.Trim();
                model.Mobile = mobile?.Trim();
                model.Remarks = remarks?.Trim();

                _subscriptionUserService.Save(model);
            }

            return Json(apiResponse);
        }
    }
}
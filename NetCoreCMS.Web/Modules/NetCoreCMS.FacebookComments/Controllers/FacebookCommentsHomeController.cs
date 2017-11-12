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
using NetCoreCMS.Modules.FacebookComments;
using NetCoreCMS.Modules.FacebookComments.Models;
using Newtonsoft.Json;
using System.Linq;
using NetCoreCMS.Framework.Core.Network;
using Microsoft.AspNetCore.Authorization;
using NetCoreCMS.Framework.Core.Mvc.Attributes;

namespace NetCoreCMS.FacebookComments.Controllers
{
    [AdminMenu(Name = "Facebook Comments", Order = 100)]
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class FacebookCommentsHomeController : NccController
    {
        #region Initialization
        private NccSettingsService _nccSettingsService;

        private FacebookCommentsSettings nccFacebookCommentsSettings;

        public FacebookCommentsHomeController(NccSettingsService nccSettingsService, ILoggerFactory factory)
        {
            _logger = factory.CreateLogger<FacebookCommentsHomeController>();
            nccFacebookCommentsSettings = new FacebookCommentsSettings();

            _nccSettingsService = nccSettingsService;
            try
            {
                var tempSettings = _nccSettingsService.GetByKey("NccFacebookComments_Settings");
                if (tempSettings != null)
                {
                    nccFacebookCommentsSettings = JsonConvert.DeserializeObject<FacebookCommentsSettings>(tempSettings.Value);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
        #endregion

        [AdminMenuItem(Name = "Settings", Url = "/FacebookCommentsHome/Index", Order = 1)]
        public ActionResult Index()
        {
            var nccTranslator = new NccTranslator(CurrentLanguage);
            return View(nccFacebookCommentsSettings);
        }

        [HttpPost]
        public ActionResult Index(bool IsActive, string FacebookAppId, bool RemoveLanguageParamenter, string ColorScheme, int NumberOfPost)
        {
            nccFacebookCommentsSettings.IsActive = IsActive;
            nccFacebookCommentsSettings.FacebookAppId = FacebookAppId;
            nccFacebookCommentsSettings.RemoveLanguageParamenter = RemoveLanguageParamenter;
            nccFacebookCommentsSettings.ColorScheme = ColorScheme;
            nccFacebookCommentsSettings.NumberOfPost = NumberOfPost;
            try
            {
                var tempSettings = JsonConvert.SerializeObject(nccFacebookCommentsSettings);
                _nccSettingsService.SetByKey("NccFacebookComments_Settings", tempSettings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return View(nccFacebookCommentsSettings);
        }
    }
}
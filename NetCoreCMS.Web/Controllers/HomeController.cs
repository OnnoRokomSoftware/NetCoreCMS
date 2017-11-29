/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;
using System.Net;

using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

using NetCoreCMS.Framework.i18n;
using NetCoreCMS.Framework.Core.App;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Messages;
using System.Collections.Generic;
using NetCoreCMS.Framework.Resources;
using NetCoreCMS.Framework.Core.Models;
using System.Linq;
using NetCoreCMS.Framework.Core.Services;

namespace NetCoreCMS.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : NccController
    {
        IHostingEnvironment _env;

        public HomeController(IHostingEnvironment env, ILoggerFactory factory)
        {
            _logger = factory.CreateLogger<HomeController>();
            _env = env;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            if (SetupHelper.IsDbCreateComplete && SetupHelper.IsAdminCreateComplete)
            {
                if (GlobalContext.SetupConfig == null)
                {
                    GlobalContext.SetupConfig = SetupHelper.LoadSetup();
                }

                var setupConfig = GlobalContext.SetupConfig;

                if (setupConfig == null)
                {
                    TempData["ErrorMessage"] = "Setup config file is missed. Please reinstall.";
                    return Redirect("~/CmsHome/ResourceNotFound");
                }
                if (setupConfig.StartupData.Trim('/') == "" || setupConfig.StartupData.Trim().ToLower() == "/home")
                {
                    return View();
                }

                var langEnabledUrl = NccUrlHelper.AddLanguageToUrl(CurrentLanguage, NccUrlHelper.EncodeUrl(setupConfig.StartupUrl));

                return Redirect(langEnabledUrl);
            }
            return Redirect("/SetupHome/Index");
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult NotAuthorized()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult RedirectToDefaultLanguage()
        {
            var lang = CurrentLanguage;
            var redirectUrl = Request.Path.Value + "" + Request.QueryString;
            if (Request.Path.Value.StartsWith("/en") || Request.Path.Value.StartsWith("/bn"))
            {
                return Redirect("~" + redirectUrl);
            }
            return Redirect("~/" + lang + redirectUrl);
        }

        //[HttpPost]
        [AllowAnonymous]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            culture = culture.ToLower();
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            returnUrl = WebUtility.UrlDecode(returnUrl);

            if (!string.IsNullOrEmpty(returnUrl) && returnUrl.Length > 4)
            {
                if (!IsContainsLangPrefix(returnUrl))
                {
                    returnUrl = culture + returnUrl;
                }

                if (!IsStartedWithCurrentCulture(returnUrl, culture))
                {
                    if (returnUrl.StartsWith("/"))
                    {
                        returnUrl = returnUrl.Substring(3);
                    }
                    else
                    {
                        returnUrl = returnUrl.Substring(2);
                    }

                    returnUrl = culture + returnUrl;
                }
            }

            if (returnUrl.StartsWith("/") == false)
            {
                returnUrl = "/" + returnUrl;
            }

            returnUrl = NccUrlHelper.EncodeUrl(returnUrl);

            return Redirect(returnUrl);
        }

        private bool IsStartedWithCurrentCulture(string returnUrl, string culture)
        {
            if (returnUrl.ToLower().StartsWith(culture) || returnUrl.ToLower().StartsWith("/" + culture))
            {
                return true;
            }
            return false;
        }

        private bool IsContainsLangPrefix(string returnUrl)
        {
            foreach (var item in SupportedCultures.Cultures)
            {
                if (returnUrl.ToLower().StartsWith(item.TwoLetterISOLanguageName.ToLower()) || returnUrl.ToLower().StartsWith("/" + item.TwoLetterISOLanguageName.ToLower()))
                    return true;
            }
            return false;
        }

        [AllowAnonymous]
        public async System.Threading.Tasks.Task<ActionResult> SetupSuccess()
        {
            string referer = Request.Headers["Referer"].ToString();
            if (referer.EndsWith("/SetupHome/CreateAdmin"))
            {
                Program.RestartAppAsync();
            }
            return View();
        }

        [Authorize(Roles = "SuperAdmin")]
        public async System.Threading.Tasks.Task<IActionResult> RestartHost()
        {
            string referer = Request.Headers["Referer"].ToString();
            NetCoreCmsHost.IsRestartRequired = true;
            Program.RestartAppAsync();
            NetCoreCmsHost.IsRestartRequired = false;
            ViewBag.ReturnUrl = referer;
            ViewBag.ReturnUrlName = referer;

            if (referer.Trim() == "" || referer.Contains("RestartHost"))
            {
                ViewBag.ReturnUrl = "/";
                ViewBag.ReturnUrlName = "Home";
            }
            return View();
        }

        [AllowAnonymous]
        public IActionResult ResourceNotFound()
        {
            if (_env.IsDevelopment())
            {
                ViewData["ErrorMessage"] = "<strong style='color:black;font-family:Monda;'>Possible steps you may try:</strong><br/> 1. Build the modules after change. <br/>2. Restart <br/>3. Delete setup.json and setup the CMS again.";
            }
            return View();
        }
    }
}
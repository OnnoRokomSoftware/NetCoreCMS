using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Extensions.Localization;
using NetCoreCMS.Framework.i18n;
using System.Web;
using System.Text;
using System.Net;
using System.Linq;
using NetCoreCMS.Framework.Core.App;

namespace NetCoreCMS.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : NccController
    {
        IHostingEnvironment _env;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public HomeController(IHostingEnvironment env, ILoggerFactory factory, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _logger = factory.CreateLogger<HomeController>();
            _env = env;
            _sharedLocalizer = sharedLocalizer;
        }

        public IActionResult Index()
        {
            if (SetupHelper.IsDbCreateComplete && SetupHelper.IsAdminCreateComplete)
            {
                if(GlobalConfig.SetupConfig == null)
                {
                    GlobalConfig.SetupConfig = SetupHelper.LoadSetup();
                }

                var setupConfig = GlobalConfig.SetupConfig;

                if (setupConfig == null)
                {
                    TempData["Message"] = "Setup config file is missed. Please reinstall.";
                    return Redirect("~/CmsHome/ResourceNotFound");
                }
                if (setupConfig.StartupData.Trim('/') == "" || setupConfig.StartupData.Trim().ToLower() == "/home")
                {
                    return View();
                }

                var langEnabledUrl = NccUrlHelper.AddLanguageToUrl( CurrentLanguage, NccUrlHelper.EncodeUrl(setupConfig.StartupUrl));

                return Redirect( langEnabledUrl);
            }
            return Redirect("/SetupHome/Index");
        }

       

        public ActionResult About()
        {
            ViewBag.Message = _sharedLocalizer["About"];
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public ActionResult RedirectToDefaultLanguage()
        {
            var lang = CurrentLanguage;
            var redirectUrl = Request.Path.Value+""+Request.QueryString;
            if (Request.Path.Value.StartsWith("/en") || Request.Path.Value.StartsWith("/bn"))
            {
                return Redirect("~" + redirectUrl);
            }
            return Redirect("~/" + lang + redirectUrl);
        }

        [HttpPost]
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
                    returnUrl =  culture + returnUrl;
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

                    returnUrl =  culture + returnUrl;
                }
            }

            returnUrl = NccUrlHelper.EncodeUrl(returnUrl);

            return Redirect(returnUrl);
        }

        private bool IsStartedWithCurrentCulture(string returnUrl, string culture)
        {
            if (returnUrl.ToLower().StartsWith(culture) || returnUrl.ToLower().StartsWith("/"+culture))
            {
                return true;
            }
            return false;
        }

        private bool IsContainsLangPrefix(string returnUrl)
        {
            foreach (var item in SupportedCultures.Cultures)
            {
                if (returnUrl.ToLower().StartsWith(item.TwoLetterISOLanguageName.ToLower()) || returnUrl.ToLower().StartsWith("/"+item.TwoLetterISOLanguageName.ToLower()) )
                    return true;
            }
            return false;
        }

        public async System.Threading.Tasks.Task<ActionResult> SetupSuccess()
        {
            Program.RestartAppAsync();
            return View();
        }

        [Authorize(Roles = "SuperAdmin,Administrator")]
        public async System.Threading.Tasks.Task<IActionResult> RestartHost()
        {
            //TODO: need to secure this restart.
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

        public IActionResult ResourceNotFound()
        {
            if (_env.IsDevelopment())
            {
                ViewData["Message"] = "<strong style='color:black;font-family:Monda;'>Possible steps you may try:</strong><br/> 1. Build the modules after change. <br/>2. Restart <br/>3. Delete setup.json and setup the CMS again.";
            }
            return View();
        }

        public ActionResult Temp()
        {
            var nccTranslator = new NccTranslator<HomeController>(CurrentLanguage);            
            ViewBag.Name = nccTranslator["Name"];

            return View();
        }

    }
}
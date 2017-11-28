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
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.i18n;
using NetCoreCMS.Framework.Core.Mvc.Attributes;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using Microsoft.AspNetCore.Authorization;

namespace NetCoreCMS.HelloWorld.Controllers
{
    [AdminMenu(Name = "Hello Module", Order = 100)]
    [SiteMenu(Name = "Hello Module Site Menu", Order = 100)]
    public class HelloHomeController : NccController
    {
        public HelloHomeController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HelloHomeController>();
        }

        [AdminMenuItem(Name = "Index", Url = "/HelloHome/Index", Order = 1)]
        [SiteMenuItem(Name = "Hello world Home", Url = "/HelloHome/Index", Order = 1)]
        public ActionResult Index()
        {
            var nccTranslator = new NccTranslator(CurrentLanguage);
            return View();
        }

        [SiteMenuItem(Name = "Role Home", Url = "/HelloHome/RoleHome", Order = 2)]
        public ActionResult RoleHome()
        {
            return View();
        }

        public ActionResult SubAction()
        {
            return View();
        }
        
        [AllowAnonymous]
        [SiteMenuItem(Name ="Recaptcha", Order = 3)]
        public ActionResult Recaptcha()
        {
            return View();
        }

        [ValidateRecaptcha]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult RecaptchaPost()
        {
            if (ModelState.IsValid)
            {
                ShowMessage("You were not a robot :)", Framework.Core.Mvc.Views.MessageType.Success, false, true);
            }
            else
            {
                ShowMessage("Are you human ??? :x", Framework.Core.Mvc.Views.MessageType.Error, false, true);
            }
            return RedirectToAction("Recaptcha");
        }
    }
}

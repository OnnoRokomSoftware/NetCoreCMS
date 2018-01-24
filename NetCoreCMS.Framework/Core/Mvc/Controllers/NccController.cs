/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Mvc.Filters;
using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.i18n;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Utility;

namespace NetCoreCMS.Framework.Core.Mvc.Controllers
{
    [ServiceFilter(typeof(NccGlobalExceptionFilter))]
    public class NccController : Controller
    {
        protected ILogger _logger;
        protected IHostingEnvironment Environment { get { return GlobalContext.HostingEnvironment; } }
        
        protected INccSettingsService Settings { get{
                object val;
                HttpContext.Items.TryGetValue("NCC_CONTROLLER_PROPERTY_SETTINGS", out val);
                return (INccSettingsService)val;
            }
        }

        protected INccUserService UserService
        {
            get
            {
                object val;
                HttpContext.Items.TryGetValue("NCC_CONTROLLER_PROPERTY_USER_SERVICE", out val);
                return (INccUserService)val;
            }
        }

        protected INccTranslator _T { get {
                object val;
                HttpContext.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_TRANSLATOR", out val);
                return (INccTranslator)val;                
            }
        }

        protected IMemoryCache Cache
        {
            get
            {
                object val;
                HttpContext.Items.TryGetValue("NCC_CONTROLLER_PROPERTY_CACHE", out val);
                return (IMemoryCache) val;
            }
        }

        public NccController()
        {
            
        }

        public string CurrentLanguage
        {
            get
            {
                object val;
                HttpContext.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_CURRENT_LANGUAGE", out val);
                return (string)val ?? "";
            }
        }

        public string ShowMessage(string message, MessageType messageType, bool appendMessage = false, bool showAfterRedirect = false, int durationSecond = 5, bool showCloseButton = true)
        {
            ViewBag.MessageDuration = durationSecond;
            ViewBag.MessageShowCloseButton = showCloseButton;

            if (showAfterRedirect)
            {
                TempData["MessageDuration"] = durationSecond;
                TempData["MessageShowCloseButton"] = showCloseButton;
            }

            switch (messageType)
            {
                case MessageType.Success:
                    if (appendMessage == true)
                    {
                        ViewBag.SuccessMessage += message;
                        if (showAfterRedirect)
                        {
                            TempData["SuccessMessage"] += message;
                        }
                    }
                    else
                    {
                        ViewBag.SuccessMessage = message;
                        if (showAfterRedirect)
                        {
                            TempData["SuccessMessage"] = message;
                        }
                    }
                    break;
                case MessageType.Info:
                    if (appendMessage == true)
                    {
                        ViewBag.InfoMessage += message;
                        if (showAfterRedirect)
                        {
                            TempData["InfoMessage"] += message;
                        }
                    }
                    else
                    {
                        ViewBag.InfoMessage = message;
                        if (showAfterRedirect)
                        {
                            TempData["InfoMessage"] = message;
                        }
                    }
                    break;
                case MessageType.Warning:
                    if (appendMessage == true)
                    {
                        ViewBag.WarningMessage += message;
                        if (showAfterRedirect)
                        {
                            TempData["WarningMessage"] += message;
                        }
                    }
                    else
                    {
                        ViewBag.WarningMessage = message;
                        if (showAfterRedirect)
                        {
                            TempData["WarningMessage"] += message;
                        }
                    }
                    break;
                case MessageType.Error:
                    if (appendMessage == true)
                    {
                        ViewBag.ErrorMessage += message;
                        if (showAfterRedirect)
                        {
                            TempData["ErrorMessage"] += message;
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = message;
                        if (showAfterRedirect)
                        {
                            TempData["ErrorMessage"] = message;
                        }
                    }
                    break;
                default:
                    var msg = "Invalid Message Type";
                    if (appendMessage == true)
                    {
                        ViewBag.ErrorMessage += msg;
                        if (showAfterRedirect)
                        {
                            TempData["ErrorMessage"] += msg;
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = msg;
                        if (showAfterRedirect)
                        {
                            TempData["ErrorMessage"] = msg;
                        }
                    }
                    break;
            }
            return "";
        }
    }
}

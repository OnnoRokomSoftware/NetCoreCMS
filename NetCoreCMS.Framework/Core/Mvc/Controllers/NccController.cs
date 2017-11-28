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
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Mvc.FIlters;
using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.i18n;
using NetCoreCMS.Framework.Setup;

namespace NetCoreCMS.Framework.Core.Mvc.Controllers
{
    [ServiceFilter(typeof(NccGlobalExceptionFilter))]
    public class NccController : Controller
    {
        protected ILogger _logger;        
        protected NccLanguageDetector _nccLanguageDetector;

        public NccController()
        {
            var language = SetupHelper.Language;            
        }

        public string CurrentLanguage
        {
            get
            {
                if(_nccLanguageDetector == null)
                {
                    _nccLanguageDetector = (NccLanguageDetector)HttpContext.RequestServices.GetService(typeof(NccLanguageDetector));
                }                
                return _nccLanguageDetector.GetCurrentLanguage();
            }
        }

        public string ShowMessage(string message, MessageType messageType, bool appendMessage = false, bool showAfterRedirect = false)
        {
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

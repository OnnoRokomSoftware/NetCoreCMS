/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.AspNetCore.Mvc.Filters;
using NetCoreCMS.Framework.i18n;
using NetCoreCMS.Framework.Setup;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.Mvc.FIlters
{
    public class LanguageFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var language = SetupHelper.Language;
            if (context.HttpContext.Request.Cookies.ContainsKey(SupportedCultures.LanguageCookieKey))
            {
                context.HttpContext.Request.Cookies.TryGetValue(SupportedCultures.LanguageCookieKey, out language);
            }
            
            context.HttpContext.Items.Add(SupportedCultures.SharedResourceTranslatorObjectKey, new NccTranslator<SharedResource>(language));
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}

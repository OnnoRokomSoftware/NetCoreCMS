/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Threading.Tasks;

namespace NetCoreCMS.Framework.i18n
{
    public class NccRequestCultureProvider : RequestCultureProvider
    {
        public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            //Go away and do a bunch of work to find out what culture we should do. 
            await Task.Yield();
            var httpContextAccessor = new HttpContextAccessor();
            httpContextAccessor.HttpContext = httpContext;
            var langDetector = new NccLanguageDetector(httpContextAccessor);
            var lang = langDetector.GetCurrentLanguage();            
            //Return a provider culture result. 
            return new ProviderCultureResult(lang);
            //In the event I can't work out what culture I should use. Return null. 
            //Code will fall to other providers in the list OR use the default. 
            //return null;
        }
    }

}

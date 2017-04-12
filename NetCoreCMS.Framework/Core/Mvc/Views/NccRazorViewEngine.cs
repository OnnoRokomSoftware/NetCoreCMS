using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace NetCoreCMS.Framework.Core.Mvc.Views
{
    public class NccRazorViewEngine : RazorViewEngine
    {
        public NccRazorViewEngine(IRazorPageFactoryProvider pageFactory, IRazorPageActivator pageActivator, HtmlEncoder htmlEncoder, IOptions<RazorViewEngineOptions> optionsAccessor, ILoggerFactory loggerFactory) : base(pageFactory, pageActivator, htmlEncoder, optionsAccessor, loggerFactory)
        {
        }
    }
}

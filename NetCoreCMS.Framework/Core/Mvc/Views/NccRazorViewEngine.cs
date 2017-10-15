using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text.Encodings.Web;

namespace NetCoreCMS.Framework.Core.Mvc.Views
{
    public class NccRazorViewEngine : RazorViewEngine
    {
        public NccRazorViewEngine(IRazorPageFactoryProvider pageFactory, IRazorPageActivator pageActivator, HtmlEncoder htmlEncoder, IOptions<RazorViewEngineOptions> optionsAccessor, RazorProject razorProject, ILoggerFactory loggerFactory, DiagnosticSource diagnosticSource) : 
            base(pageFactory, pageActivator, htmlEncoder, optionsAccessor, razorProject, loggerFactory, diagnosticSource)
        {
           
        }
    }
}

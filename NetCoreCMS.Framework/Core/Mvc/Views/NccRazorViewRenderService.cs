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
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NetCoreCMS.Framework.i18n;
using Microsoft.Extensions.Logging;

namespace NetCoreCMS.Framework.Core.Mvc.Views
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync<T>(string viewName, object model);
    }

    public class NccRazorViewRenderService : IViewRenderService
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private INccTranslator _nccTranslator;
        private NccLanguageDetector _nccLanguageDetector;
        private readonly ILogger _logger;

        public NccRazorViewRenderService(IRazorViewEngine razorViewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider, IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory)
        {
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _httpContextAccessor = httpContextAccessor;
            _logger = loggerFactory.CreateLogger<NccRazorViewRenderService>();
        }
        
        public async Task<string> RenderToStringAsync<T>(string viewName, object model)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext; //new DefaultHttpContext { RequestServices = _serviceProvider };            
                var routeData = httpContext.GetRouteData();
                var cad = new ControllerActionDescriptor();
                var ac = new ActionContext(httpContext, httpContext.GetRouteData(), cad);
                var actionContext = new ControllerContext(ac);

                _nccLanguageDetector = new NccLanguageDetector(_httpContextAccessor);
                var language = _nccLanguageDetector.GetCurrentLanguage();
                _nccTranslator = new NccTranslator(language);

                if (httpContext.Items.ContainsKey("NCC_RAZOR_PAGE_PROPERTY_CURRENT_LANGUAGE"))
                {
                    httpContext.Items["NCC_RAZOR_PAGE_PROPERTY_CURRENT_LANGUAGE"] = language;
                }
                else
                {
                    httpContext.Items.Add("NCC_RAZOR_PAGE_PROPERTY_CURRENT_LANGUAGE", language);
                }

                if (httpContext.Items.ContainsKey("NCC_RAZOR_PAGE_PROPERTY_TRANSLATOR"))
                {
                    httpContext.Items["NCC_RAZOR_PAGE_PROPERTY_TRANSLATOR"] = _nccTranslator;
                }
                else
                {
                    httpContext.Items.Add("NCC_RAZOR_PAGE_PROPERTY_TRANSLATOR", _nccTranslator);
                }

                if (httpContext.Items.ContainsKey("NCC_RAZOR_PAGE_PROPERTY_LOGGER"))
                {
                    httpContext.Items["NCC_RAZOR_PAGE_PROPERTY_LOGGER"] = _logger;
                }
                else
                {
                    httpContext.Items.Add("NCC_RAZOR_PAGE_PROPERTY_LOGGER", _logger);
                }

                var typeInfo = typeof(T).GetTypeInfo();
                //actionContext.ActionDescriptor.ActionName = "Index";
                //actionContext.ActionDescriptor.ControllerName = "Home";
                actionContext.ActionDescriptor.DisplayName = typeInfo.Module.Name;
                actionContext.ActionDescriptor.ControllerTypeInfo = typeInfo;

                using (var sw = new StringWriter())
                {
                    var viewResult = _razorViewEngine.FindView(actionContext, viewName, false);

                    if (viewResult.View == null)
                    {
                        throw new ArgumentNullException($"{viewName} does not match any available view");
                    }

                    var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                    {
                        Model = model
                    };

                    var viewContext = new ViewContext(
                        actionContext,
                        viewResult.View,
                        viewDictionary,
                        new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                        sw,
                        new HtmlHelperOptions()
                    );

                    await viewResult.View.RenderAsync(viewContext);
                    return sw.ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message,ex);
            }

            return string.Empty;
        }
    }
}
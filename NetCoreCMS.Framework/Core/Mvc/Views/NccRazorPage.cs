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
using System.Threading.Tasks;
using System.Collections.Generic;

using MediatR;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

using NetCoreCMS.Framework.Core.Events.Themes;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Core.Messages;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Framework.i18n;
using NetCoreCMS.Framework.Core.Auth.Handlers;
using Microsoft.Extensions.Logging;
using System.Linq;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Modules.Widgets;
using Microsoft.AspNetCore.Http;
using NetCoreCMS.Framework.Core.Auth;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;

namespace NetCoreCMS.Framework.Core.Mvc.Views
{
    public abstract class NccRazorPage<TModel> : RazorPage<TModel>
    {
        public string Title {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_TITLE", out val);
                return (string)val ?? "";
            }
            set {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_TITLE"] = value;
            }
        }
        public string SubTitle
        {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_SUB_TITLE", out val);
                return (string)val ?? "";
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_SUB_TITLE"] = value;
            }
        }
        public string SiteName
        {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_SITE_NAME", out val);
                return (string)val ?? "";
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_SITE_NAME"] = value;
            }
        }
        public string SiteSlogan
        {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_SITE_SLOGAN", out val);
                return (string)val ?? "";
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_SITE_SLOGAN"] = value;
            }
        }
        public string SiteLogo
        {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_SITE_LOGO", out val);
                return (string)val ?? "";
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_SITELOGO"] = value;
            }
        }
        public string Favicon
        {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_FAVICON", out val);
                return (string)val ?? "";
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_FAVICON"] = value;
            }
        }
        public string CurrentLayout {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_CURRENT_LAYOUT", out val);
                return (string)val ?? "";
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_CURRENT_LAYOUT"] = value;
            }
        }
        public string CurrentLanguage {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_CURRENT_LANGUAGE", out val);
                return (string)val ?? "";
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_CURRENT_LANGUAGE"] = value;
            }
        }
        public string CurrentLanguageCode
        {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_CURRENT_LANGUAGE_CODE", out val);
                return (string)val ?? "";
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_CURRENT_LANGUAGE_CODE"] = value;
            }
        }

        public string CurrentLanguageText
        {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_CURRENT_LANGUAGE_TEXT", out val);
                return (string)val ?? "";
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_CURRENT_LANGUAGE_TEXT"] = value;
            }
        }

        public string ControllerName
        {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_CONTROLLER_NAME", out val);
                return (string)val ?? "";
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_CONTROLLER_NAME"] = value;
            }
        }

        public string BaseUrl
        {
            get
            {
                var val = Context.Request.IsHttps ? "https://" : "http://";
                val += Context.Request.Host.Value;
                return val ?? "";
            }
        }
        public string PageUrl
        {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_PAGE_URL", out val);
                return (string)val ?? "";
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_PAGE_URL"] = value;
            }
        }

        public bool HasLeftColumn {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_HAS_LEFT_COLUMN", out val);
                return val == null ? false : (bool)val;
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_HAS_LEFT_COLUMN"] = value;
            }
        }
        public float LeftColumnWidth
        {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_LEFT_COLUMN_WIDTH", out val);
                return val == null ? 0 : (float)val;
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_LEFT_COLUMN_WIDTH"] = value;
            }
        }
        public bool HasRightColumn {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_HAS_RIGHT_COLUMN", out val);
                return val == null ? false : (bool)val;
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_HAS_RIGHT_COLUMN"] = value;
            }
        }
        public float RightColumnWidth
        {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_RIGHT_COLUMN_WIDTH", out val);
                return val == null ? 0 : (float)val;
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_RIGHT_COLUMN_WIDTH"] = value;
            }
        }
        public float BodyWidth {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_BODY_WIDTH", out val);
                return val == null ? 0 : (float)val;
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_BODY_WIDTH"] = value;
            }
        }
        public bool HasFooter
        {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_HAS_FOOTER", out val);
                return val == null ? false : (bool)val;
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_HAS_FOOTER"] = value;
            }
        }
        public string Breadcrumb
        {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_BREADCRUMB", out val);
                return (string)val ?? "";
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_LEFT_COLUMN_BREADCRUMB"] = value;
            }
        }
        public string MetaDescription
        {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_META_DESCRIPTION", out val);
                return (string)val ?? "";
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_META_DESCRIPTION"] = value;
            }
        }
        public string MetaKeyword {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_META_KEYWORD", out val);
                return (string)val ?? "";
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_META_KEYWORD"] = value;
            }
        }
        public string MetaAuthor {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_META_AUTHOR", out val);
                return (string)val ?? "";
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_META_AUTHOR"] = value;
            }
        }
        public string MetaGenerator {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_META_GENERATOR", out val);
                return (string)val ?? "";
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_META_GENERATOR"] = value;
            }
        }
        public string MetaApplicationName {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_META_APPLICATION_NAME", out val);
                return (string)val ?? "";
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_META_APPLICATION_NAME"] = value;
            }
        }
        public string Copyright {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_COPY_RIGHT", out val);
                return (string)val ?? "";
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_COPY_RIGHT"] = value;
            }
        }
        public string PoweredBy {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_POWERED_BY", out val);
                return (string)val ?? "";
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_POWERED_BY"] = value;
            }
        }

        public string Message
        {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_MESSAGE", out val);
                return (string)val ?? "";
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_MESSAGE"] = value;
            }
        }

        public INccTranslator _T {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_TRANSLATOR", out val);
                return (INccTranslator)val;
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_TRANSLATOR"] = value;
            }
        }

        public ILogger _Logger
        {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_LOGGER", out val);
                return (ILogger)val;
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_LOGGER"] = value;
            }
        }

        private IMediator _mediator;
        public INccAuthorizationHandler AuthorizationHandler { get; set; }
        public Dictionary<string, object> PageProperty {
            get
            {
                object val;
                Context.Items.TryGetValue("NCC_RAZOR_PAGE_PROPERTY_DICTIONARY", out val);
                if (val == null)
                {
                    val = new Dictionary<string, object>();
                    PageProperty = (Dictionary<string, object>)val;
                }
                return (Dictionary<string, object>)val;
            }
            set
            {
                Context.Items["NCC_RAZOR_PAGE_PROPERTY_DICTIONARY"] = value;
            }
        }
        
        private List<Widget> _moduleWidgets = new List<Widget>();

        public void SetProperty(string key, object value)
        {
            var property = PageProperty;
            if (property.ContainsKey(key))
            {
                property[key] = value;
            }
            else
            {
                property.Add(key, value);
            }
            PageProperty = property;
        }

        public object GetProperty(string key)
        {
            var property = PageProperty;
            if (property.ContainsKey(key))
            {
                return property[key];
            }
            else
            {
                return null;
            }
        }
        
        private ThemeSection[] FireEvent(string name, string viewFile, string content, TModel model)
        {
            var themeSections = new ThemeSection[] { };

            var themeSection = new ThemeSection()
            {
                Name = name,
                Content = content,
                Model = Model,
                ViewFileName = viewFile,
                Language = CurrentLanguage,
                ViewBag  = ViewBag,
                ViewContext = ViewContext,
                HttpContext = Context,
                TempData = TempData,
                User = User,
                Path = Path
            };

            try
            {
                if (_mediator == null)
                {
                    _mediator = (IMediator)ViewContext.HttpContext.RequestServices.GetService(typeof(IMediator));
                }

                themeSections = _mediator.SendAll(new OnThemeSectionRender(themeSection)).Result;
            }
            catch (Exception ex)
            {
                //If this event does not have any handler then this send will throw an exception. We can ignore this exception.
                _Logger.LogError(ex.Message, ex);
            }

            return themeSections;
        }

        public string RenderToStringAsync(string viewName, object model)
        {
            var viewContent = "";
           
            try
            {
                var ac = new ActionContext(Context, ViewContext.RouteData, ViewContext.ActionDescriptor);
                var actionContext = new ControllerContext(ac);
                var razorViewEngine = (IRazorViewEngine)Context.RequestServices.GetService(typeof(IRazorViewEngine));
                var viewResult = razorViewEngine.FindView(actionContext, viewName, false);

                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"{viewName} does not match any available view");
                }

                using (var sw = new StringWriter())
                {
                    var viewContext = new ViewContext(
                        actionContext,
                        viewResult.View,
                        ViewData,
                        TempData,
                        sw,
                        new HtmlHelperOptions()
                    );
                    viewContext.RouteData = ViewContext.RouteData;
                    viewResult.View.RenderAsync(viewContext).Wait();
                    viewContent = sw.GetStringBuilder().ToString();        
                    //For showing which view file finally used for rendering.
                    viewContent += $"<!-- View: {viewResult.View.Path}-->";                    
                }
            }
            catch (Exception ex)
            {
                _Logger.LogWarning($"ViewFileName: {viewName} throwing exception at render time.");
                _Logger.LogError(ex.Message, ex);
                if (GlobalContext.HostingEnvironment.EnvironmentName.Contains("Development"))
                {
                    viewContent = $"<p style='color:red;'> {ex.Message}</p>"; ;
                }
                else
                {
                    viewContent = $"<p style='color:red;'> Error in view rendering, file name is {viewName}</p>"; ;
                }
            }
            
            return viewContent;
        }

        public string NccRenderPertial(string partialViewFileName, object model)
        {
            var content = RenderToStringAsync(partialViewFileName, model);
            var themeSection = FireEvent(ThemeSection.Sections.PartialView, partialViewFileName, content, Model);
            content = themeSection.LastOrDefault()?.Content;
            ViewContext.Writer.WriteLine(content??"");
            return string.Empty;
        }

        public string NccRenderHead(string headViewFile = "Parts/_Head")
        {
            var content = RenderToStringAsync(headViewFile, Model);
            var themeSections = FireEvent(ThemeSection.Sections.Head,headViewFile, content, Model);
            content = themeSections.LastOrDefault()?.Content??"";
            content += Environment.NewLine + $"<meta name=\"generator\" content=\"NetCoreCMS v{NccInfo.Version}\" />";
            ViewContext.Writer.WriteLine(content);
            return string.Empty;
        }

        public string NccRenderHeaderCss()
        {
            var content = MakeResourceList(NccResource.ResourceType.CssFile, NccResource.IncludePosition.Header);
            var themeSections = FireEvent(ThemeSection.Sections.HeaderCss, "RegistereHeaderCss", content, Model);
            content = themeSections.LastOrDefault()?.Content ?? "";
            ViewContext.Writer.WriteLine(content);
            return string.Empty;
        }

        public string NccRenderHeaderScripts()
        {
            var content = MakeResourceList(NccResource.ResourceType.JsFile, NccResource.IncludePosition.Header);
            var themeSections = FireEvent(ThemeSection.Sections.HeaderScripts, "RegistereHeaderScripts", content, Model);
            content = themeSections.LastOrDefault()?.Content ?? "";
            ViewContext.Writer.WriteLine(content);
            return string.Empty;
        }
        
        public string NccRenderHeader(string headerViewFile = "Parts/_Header")
        {
            var content =  RenderToStringAsync(headerViewFile, Model);
            var themeSections = FireEvent(ThemeSection.Sections.Header, headerViewFile, content, Model);
            content = themeSections.LastOrDefault()?.Content ?? "";
            ViewContext.Writer.WriteLine(content);
            return string.Empty;
        }

        public string NccRenderNavigation(string navigationViewFile = "Parts/_Navigation")
        {
            var content = RenderToStringAsync(navigationViewFile, Model);
            var themeSections = FireEvent(ThemeSection.Sections.Navigation, navigationViewFile, content, Model);
            content = themeSections.LastOrDefault()?.Content ?? "";
            ViewContext.Writer.WriteLine(content);
            return string.Empty;
        }

        public string NccRenderFeaturedSection(string featuredViewFile = "Parts/_Featured")
        {
            var content = RenderToStringAsync(featuredViewFile, Model);
            var themeSections = FireEvent(ThemeSection.Sections.Featured, featuredViewFile, content, Model);
            content = themeSections.LastOrDefault()?.Content ?? "";
            ViewContext.Writer.WriteLine(content);
            return string.Empty;
        }

        public string NccRenderLeftColumn(string leftColumnViewFile = "Parts/_LeftColumn")
        {
            var content = RenderToStringAsync(leftColumnViewFile, Model);
            var themeSections = FireEvent(ThemeSection.Sections.LeftColumn, leftColumnViewFile, content, Model);
            content = themeSections.LastOrDefault()?.Content ?? "";
            ViewContext.Writer.WriteLine(content);
            return string.Empty;
        }
        
        public IHtmlContent NccRenderBody()
        {
            var bodyBuff = (ViewBuffer)RenderBody();
            var content = "";
            using(var sw = new StringWriter())
            {
                bodyBuff.WriteTo(sw, HtmlEncoder);
                content = sw.GetStringBuilder().ToString();
            }            
            var themeSections = FireEvent(ThemeSection.Sections.Body, ViewContext.View.Path, content, Model);
            bodyBuff.Clear();
            bodyBuff.SetHtmlContent(content = themeSections.LastOrDefault()?.Content ?? "");
            return bodyBuff;
        }

        public string NccRenderRightColumn(string rightColumnViewFile = "Parts/_RightColumn")
        {
            var content = RenderToStringAsync(rightColumnViewFile, Model);
            var themeSections = FireEvent(ThemeSection.Sections.RightColumn, rightColumnViewFile, content, Model);
            content = themeSections.LastOrDefault()?.Content ?? "";
            ViewContext.Writer.WriteLine(content);
            return string.Empty;
        }

        public string NccRenderFooter(string footerViewFile = "Parts/_Footer")
        {
            var content = RenderToStringAsync(footerViewFile, Model);
            var themeSections = FireEvent(ThemeSection.Sections.Footer, footerViewFile, content, Model);
            content = themeSections.LastOrDefault()?.Content ?? "";
            ViewContext.Writer.WriteLine(content);
            return string.Empty;
        }

        public string NccRenderFooterCss()
        {
            var content = MakeResourceList(NccResource.ResourceType.CssFile, NccResource.IncludePosition.Footer);
            var themeSections = FireEvent(ThemeSection.Sections.FooterCss, "RegistereFooterCss", content, Model);
            content = themeSections.LastOrDefault()?.Content ?? "";
            ViewContext.Writer.WriteLine(content);
            return string.Empty;
        }

        public string NccRenderFooterScripts()
        {
            var content = MakeResourceList(NccResource.ResourceType.JsFile, NccResource.IncludePosition.Footer);
            var themeSections = FireEvent(ThemeSection.Sections.FooterScripts, "RegistereFooterScripts", content, Model);
            content = themeSections.LastOrDefault()?.Content ?? "";
            ViewContext.Writer.WriteLine(content);
            return string.Empty;
        }

        public string NccRenderLoadingMaskContainer()
        {
            var loadingMaskDiv = "<div id=\"loadingMask\" class=\"loader loader - double\"></div>";
            var themeSections = FireEvent(ThemeSection.Sections.LoadingMask, CurrentLayout, loadingMaskDiv, Model);
            loadingMaskDiv = themeSections.LastOrDefault()?.Content ?? "";
            ViewContext.Writer.WriteLine(loadingMaskDiv);
            return string.Empty;
        }

        public string NccRenderGlobalMessages()
        {
            var globalMessageContainer = "";
            var content = ThemeHelper.GetGlobalMessages(GlobalMessage.MessageFor.Site);
            if(string.IsNullOrEmpty(content) == false)
            {
                globalMessageContainer = "<div id=\"globalMessageContainer\" class=\"ncc-global-message\">" + content + "</div>";
            }
            var themeSections = FireEvent(ThemeSection.Sections.GlobalMessageContainer, CurrentLayout, globalMessageContainer, Model);
            globalMessageContainer = themeSections.LastOrDefault()?.Content ?? "";
            ViewContext.Writer.WriteLine(globalMessageContainer);
            return string.Empty;            
        }
        
        public string NccRenderMessages(string messagesViewFile = "Parts/_Messages")
        {
            var content = RenderToStringAsync(messagesViewFile, Model);
            var themeSections = FireEvent(ThemeSection.Sections.Messages, messagesViewFile, content, Model);
            content = themeSections.LastOrDefault()?.Content ?? "";
            ViewContext.Writer.WriteLine(content);
            return string.Empty;
        }

        private string MakeResourceList(NccResource.ResourceType type, NccResource.IncludePosition position)
        {
            var content = "";
            var resourceList = ThemeHelper.GetAllResources(type, position);
            foreach (var item in resourceList)
            {
                var version = "";
                if (string.IsNullOrEmpty(item.Version) == false)
                {
                    version = $"?{item.Version}";
                }
                if(type == NccResource.ResourceType.JsFile)
                {
                    content += $"<script type=\"text/javascript\" src=\"{item.FilePath}{version}\"></script>" + Environment.NewLine;
                }
                else if(type == NccResource.ResourceType.CssFile)
                {
                    content += $"<link rel=\"stylesheet\" type=\"text/css\" href=\"{item.FilePath}{version}\" />" + Environment.NewLine;
                }
            }
            
            return content;
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
                    if (appendMessage == true) {
                        ViewBag.SuccessMessage += message;
                        if (showAfterRedirect)
                        {
                            TempData["SuccessMessage"] += message;
                        }
                    } else {
                        ViewBag.SuccessMessage = message;
                        if (showAfterRedirect)
                        {
                            TempData["SuccessMessage"] = message;
                        }
                    }
                    break;
                case MessageType.Info:
                    if (appendMessage == true) {
                        ViewBag.InfoMessage += message;
                        if (showAfterRedirect)
                        {
                            TempData["InfoMessage"] += message;
                        }
                    } else {
                        ViewBag.InfoMessage = message;
                        if (showAfterRedirect)
                        {
                            TempData["InfoMessage"] = message;
                        }
                    }
                    break;
                case MessageType.Warning:
                    if (appendMessage == true) {
                        ViewBag.WarningMessage += message;
                        if (showAfterRedirect)
                        {
                            TempData["WarningMessage"] += message;
                        }
                    } else {
                        ViewBag.WarningMessage = message;
                        if (showAfterRedirect)
                        {
                            TempData["WarningMessage"] += message;
                        }
                    }
                    break;
                case MessageType.Error:
                    if (appendMessage == true) {
                        ViewBag.ErrorMessage += message;
                        if (showAfterRedirect)
                        {
                            TempData["ErrorMessage"] += message;
                        }
                    } else {
                        ViewBag.ErrorMessage = message;
                        if (showAfterRedirect)
                        {
                            TempData["ErrorMessage"] = message;
                        }
                    }
                    break;
                default:
                    if (appendMessage == true) {
                        ViewBag.SuccessMessage += message;
                        if (showAfterRedirect)
                        {
                            TempData["SuccessMessage"] += message;
                        }
                    } else {
                        ViewBag.SuccessMessage = message;
                        if (showAfterRedirect)
                        {
                            TempData["SuccessMessage"] = message;
                        }
                    }
                    break;
            }
            return "";
        }

        public string GetCurrentUserName()
        {
            return GlobalContext.GetCurrentUserName();
        }

        public NccUser GetCurrentUser()
        {
            return GlobalContext.GetCurrentUser();
        }

        public List<NccWebSiteWidget> GetWebSiteWidgets(string layout, string zone)
        {
            var webSiteWidgetList = GlobalContext.WebSiteWidgets.Where(x => x.LayoutName == layout && x.Zone == zone).ToList();

            foreach (var item in webSiteWidgetList)
            {
                if(item.Widget == null)
                {
                    var wInstance  = GlobalContext.Widgets.Where(x => x.WidgetId == item.WidgetId).FirstOrDefault();
                    
                    var type = wInstance.GetType();
                    var nWInstance = (Widget) GlobalContext.ServiceProvider.GetService(type);
                    //nWInstance.AddDefaultConfig = wInstance.AddDefaultConfig;
                    //nWInstance.CacheDuration = wInstance.CacheDuration;
                    //nWInstance.ConfigHtml = wInstance.ConfigHtml;
                    //nWInstance.ConfigViewFileName = wInstance.ConfigViewFileName;
                    //nWInstance.Description = wInstance.Description;
                    //nWInstance.DisplayTitle = wInstance.DisplayTitle;
                    //nWInstance.EnableCache = wInstance.EnableCache;
                    //nWInstance.Footer = wInstance.Footer;
                    //nWInstance.Language = wInstance.Language;
                    //nWInstance.ModuleController = wInstance.ModuleController;
                    //nWInstance.Title = wInstance.Title;
                    //nWInstance.ViewFileName = wInstance.ViewFileName;
                    //nWInstance.WebSiteWidgetId = wInstance.WebSiteWidgetId;
                    //nWInstance.WidgetId = wInstance.WidgetId;
                    item.Widget = nWInstance;
                }
            }

            return webSiteWidgetList;
        }
         
        public List<NccMenu> GetMenus(string menuLocation, string language)
        {
            return GlobalContext.Menus.Where(x => x.Position == menuLocation && (x.MenuLanguage == language || string.IsNullOrEmpty(x.MenuLanguage))).ToList();
        }
        
        #region Website Informations

        public static string GetCurrentLanguage()
        {
            var languageDetector = new NccLanguageDetector(new HttpContextAccessor());
            var currentLanguage = languageDetector.GetCurrentLanguage();
            return currentLanguage;
        }

        public static string GetWebSiteName()
        {
            var ret = "";
            if (GlobalContext.WebSite != null)
            {
                var webInfo = GlobalContext.WebSite.WebSiteInfos.Where(x => x.Language.ToLower() == GetCurrentLanguage()).FirstOrDefault();
                if (webInfo != null)
                {
                    ret = webInfo.Name;
                }
            }
            return ret;
        }

        public static string GetWebSiteTitle()
        {
            var ret = "";
            if (GlobalContext.WebSite != null)
            {
                var webInfo = GlobalContext.WebSite.WebSiteInfos.Where(x => x.Language.ToLower() == GetCurrentLanguage()).FirstOrDefault();
                if (webInfo != null)
                {
                    ret = webInfo.SiteTitle;
                }
            }
            return ret;
        }

        public static string GetWebSiteTagline()
        {
            var ret = "";
            if (GlobalContext.WebSite != null)
            {
                var webInfo = GlobalContext.WebSite.WebSiteInfos.Where(x => x.Language.ToLower() == GetCurrentLanguage()).FirstOrDefault();
                if (webInfo != null)
                {
                    ret = webInfo.Tagline;
                }
            }
            return ret;
        }

        public static string GetWebSiteFaviconUrl()
        {
            var ret = "";
            if (GlobalContext.WebSite != null)
            {
                var webInfo = GlobalContext.WebSite.WebSiteInfos.Where(x => x.Language.ToLower() == GetCurrentLanguage()).FirstOrDefault();
                if (webInfo != null)
                {
                    ret = webInfo.FaviconUrl;
                }
            }
            return ret;
        }

        public static string GetWebSiteLogoUrl()
        {
            var ret = "";
            if (GlobalContext.WebSite != null)
            {
                var webInfo = GlobalContext.WebSite.WebSiteInfos.Where(x => x.Language.ToLower() == GetCurrentLanguage()).FirstOrDefault();
                if (webInfo != null)
                {
                    ret = webInfo.SiteLogoUrl;
                }
            }
            return ret;
        }

        public static string GetWebSiteCopyright()
        {
            var ret = "";
            if (GlobalContext.WebSite != null)
            {
                var webInfo = GlobalContext.WebSite.WebSiteInfos.Where(x => x.Language.ToLower() == GetCurrentLanguage()).FirstOrDefault();
                if (webInfo != null)
                {
                    ret = webInfo.Copyrights;
                }
            }
            return ret;
        }

        public static string GetWebSitePrivacyPolicyUrl()
        {
            var ret = "";
            if (GlobalContext.WebSite != null)
            {
                var webInfo = GlobalContext.WebSite.WebSiteInfos.Where(x => x.Language.ToLower() == GetCurrentLanguage()).FirstOrDefault();
                if (webInfo != null)
                {
                    ret = webInfo.PrivacyPolicyUrl;
                }
            }
            return ret;
        }

        public static string GetWebSiteTermsAndConditionsUrl()
        {
            var ret = "";
            if (GlobalContext.WebSite != null)
            {
                var webInfo = GlobalContext.WebSite.WebSiteInfos.Where(x => x.Language.ToLower() == GetCurrentLanguage()).FirstOrDefault();
                if (webInfo != null)
                {
                    ret = webInfo.TermsAndConditionsUrl;
                }
            }
            return ret;
        }
        #endregion

        #region Menu
        public static string PrepareMenuHtml(string position, string currentLanguage)
        {
            var menus = GlobalContext.Menus.Where(x => x.Position == position && (string.IsNullOrEmpty(x.MenuLanguage) || x.MenuLanguage.ToLower() == currentLanguage.ToLower())).OrderBy(x => x.MenuOrder).ToList();
            var menuTxt = "";

            foreach (var item in menus)
            {
                menuTxt += "<div class=\"ncc-main-menu\">";
                menuTxt += PrepareMenu(item.MenuItems, currentLanguage);
                menuTxt += "</div>";
            }

            return menuTxt;
        }

        public static string PrepareMenu(List<NccMenuItem> menuItem, string currentLanguage, string upperSubMenuCls = "nav navbar-nav", string menuItemCls = "")
        {
            var user = GlobalContext.GetCurrentUser();
            var subMenuText = "";

            menuItem = menuItem.OrderBy(m => m.MenuOrder).ToList();

            foreach (var item in menuItem)
            {
                var addItem = false;

                if (item.IsAnonymous)
                {
                    addItem = true;
                }
                else if (ControllerActionCache.ControllerActions.Where(x => x.MainController == item.Controller && x.MainAction == item.Action && x.ModuleName == item.Module).Count() > 0)
                {

                    if (user != null)
                    {
                        if (item.IsAllowAuthenticated || user.Roles.Where(x => x.Role.Name.Equals(NccCmsRoles.SuperAdmin)).Count() > 0)
                        {
                            addItem = true;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(item.Module))
                            {
                                if (user.ExtraDenies.Where(x => string.IsNullOrEmpty(x.ModuleName) && x.Controller == item.Controller && x.Action == item.Action).Count() == 0)
                                {
                                    if (user.Permissions.Where(x => x.Permission.PermissionDetails.Where(y => string.IsNullOrEmpty(y.ModuleName) && y.Controller == item.Controller && y.Action == item.Action).Count() > 0).Count() > 0)
                                    {
                                        addItem = true;
                                    }
                                    else if (user.ExtraPermissions.Where(x => string.IsNullOrEmpty(x.ModuleName) && x.Controller == item.Controller && x.Action == item.Action).Count() > 0)
                                    {
                                        addItem = true;
                                    }
                                }
                            }
                            else
                            {
                                if (user.ExtraDenies.Where(x => x.ModuleName == item.Module && x.Controller == item.Controller && x.Action == item.Action).Count() == 0)
                                {
                                    if (user.Permissions.Where(x => x.Permission.PermissionDetails.Where(y => y.ModuleName == item.Module && y.Controller == item.Controller && y.Action == item.Action).Count() > 0).Count() > 0)
                                    {
                                        addItem = true;
                                    }
                                    else if (user.ExtraPermissions.Where(x => x.ModuleName == item.Module && x.Controller == item.Controller && x.Action == item.Action).Count() > 0)
                                    {
                                        addItem = true;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    addItem = true;
                }

                if (addItem)
                {
                    var hasChildren = item.Childrens.Count > 0;
                    if (hasChildren)
                    {
                        subMenuText += "<li class=\"" + menuItemCls + "\">";

                        if (!string.IsNullOrEmpty(currentLanguage) && GlobalContext.WebSite.IsMultiLangual && !IsExternalUrl(item.Url))
                            subMenuText += "<a href=\"/" + currentLanguage + item.Url + "\" class=\"dropdown-toggle\" data-toggle=\"dropdown\" > " + item.Name + "</a>";
                        else
                            subMenuText += "<a href=\"" + item.Url + "\" class=\"dropdown-toggle\" data-toggle=\"dropdown\" > " + item.Name + "</a>";

                        subMenuText += PrepareMenu(item.Childrens, currentLanguage, "dropdown-menu multi-level", "dropdown-submenu");
                        subMenuText += "</li>";
                    }
                    else
                    {
                        subMenuText += ListItemHtml(item, currentLanguage);
                    }
                }
            }

            var menuTxt = "";
            if (string.IsNullOrEmpty(subMenuText) == false)
            {
                menuTxt = "<ul class=\"" + upperSubMenuCls + "\">";
                menuTxt += subMenuText;
                menuTxt += "</ul>";
            }

            return menuTxt;
        }

        private static string ListItemHtml(NccMenuItem item, string currentLanguage)
        {
            var url = "/";
            var urlPrefix = "";
            var data = "";

            if (item.MenuActionType == NccMenuItem.ActionType.BlogCategory)
            {
                //urlPrefix = "/Category/";
                url = item.Url;
                url = NccUrlHelper.AddLanguageToUrl(currentLanguage, url);
                return "<li><a href=\"" + url + "\" target=\"" + item.Target + "\">" + item.Name + "</a></li>";
            }
            else if (item.MenuActionType == NccMenuItem.ActionType.BlogPost)
            {
                url = item.Url;
                url = NccUrlHelper.AddLanguageToUrl(currentLanguage, url);
                return "<li><a href=\"" + url + "\" target=\"" + item.Target + "\">" + item.Name + "</a></li>";
            }
            else if (item.MenuActionType == NccMenuItem.ActionType.Module)
            {
                //urlPrefix = "/" + item.Controller + "/" + item.Action + "/";
                url = item.Url;
                url = NccUrlHelper.AddLanguageToUrl(currentLanguage, url);
                return "<li><a href=\"" + url + "\" target=\"" + item.Target + "\">" + item.Name + "</a></li>";
            }
            else if (item.MenuActionType == NccMenuItem.ActionType.Page)
            {
                //urlPrefix = "";/*/CmsHome/CmsPage/View/*/
                //item.Url = item.Url.StartsWith("/") == true ? item.Url : "/" + item.Url;
                //item.Url = NccUrlHelper.AddLanguageToUrl(currentLanguage, item.Url);
                //return "<li><a href=\"" + item.Url + "\" target=\"" + item.Target + "\">" + item.Name + "  </a></li>";
                url = item.Url;
                url = NccUrlHelper.AddLanguageToUrl(currentLanguage, url);
                return "<li><a href=\"" + url + "\" target=\"" + item.Target + "\">" + item.Name + "</a></li>";
            }
            else if (item.MenuActionType == NccMenuItem.ActionType.Tag)
            {
                url = item.Url;
                url = NccUrlHelper.AddLanguageToUrl(currentLanguage, url);
                return "<li><a href=\"" + url + "\" target=\"" + item.Target + "\">" + item.Name + "</a></li>";
            }
            else if (item.MenuActionType == NccMenuItem.ActionType.Url)
            {
                urlPrefix = "";
            }

            if (!string.IsNullOrEmpty(item.Data))
            {
                data = "?slug=" + item.Data;
            }

            url = urlPrefix + item.Url + data;
            if (!string.IsNullOrEmpty(currentLanguage) && GlobalContext.WebSite.IsMultiLangual && !IsExternalUrl(url))
            {
                url = "/" + currentLanguage + url;
            }

            var li = "<li><a href=\"" + url + "\" target=\"" + item.Target + "\">" + item.Name + "  </a></li>";
            return li;
        }

        private static bool IsExternalUrl(string url)
        {
            string pattern = @"^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
            Regex reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return reg.IsMatch(url);
        }

        #endregion
    }

    public enum MessageType
    {
        Success,
        Info,
        Warning,
        Error
    }
}

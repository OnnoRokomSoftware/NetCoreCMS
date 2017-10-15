using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;

namespace NetCoreCMS.Framework.Core.Mvc.Views
{
    public abstract class NccRazorPage<TModel> : RazorPage<TModel>
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string SiteName { get; set; }
        public string SiteSlogan { get; set; }
        public string SiteLogo { get; set; }
        public string Favicon { get; set; }
        public string CurrentLayout { get; set; }
        public string CurrentLanguage { get; set; }

        public bool HasLeftColumn { get; set; }
        public float LeftColumnWidth { get; set; }
        public bool HasRightColumn { get; set; }
        public float RightColumnWidth { get; set; }
        public float BodyWidth { get; set; }
        public bool HasFooter { get; set; }

        public string Breadcrumb { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaAuthor { get; set; }
        public string MetaGenerator { get; set; }
        public string MetaApplicationName { get; set; }
        public string Copyright { get; set; }
        public string PoweredBy { get; set; }

        private NccRazorViewRenderService _nccViewRenderer;

        private NccRazorViewRenderService GetViewRenderer()
        {
            if(_nccViewRenderer == null)
            {
                _nccViewRenderer = (NccRazorViewRenderService) ViewContext.HttpContext.RequestServices.GetService(typeof(NccRazorViewRenderService));
            }
            return _nccViewRenderer;
        }

        public string NccRenderHead(string headViewFile = "Parts/_Head")
        {
            var renderService = GetViewRenderer();
            var content = renderService.RenderToStringAsync<TModel>(headViewFile, Model).Result;
            return content;
        }

        public IHtmlContent NccRenderHeaderCss()
        {
            throw new System.Exception();
        }

        public IHtmlContent NccRenderHeaderScripts()
        {
            throw new System.Exception();
        }

        public IHtmlContent NccRenderMessageAria(string messageAriaViewFile = "Parts/_Message")
        {
            var renderService = GetViewRenderer();
            var content = renderService.RenderToStringAsync(typeof(TModel), messageAriaViewFile, Model).Result;
            return new HtmlContentBuilder().Append(content);
        }

        public IHtmlContent NccRenderHeader(string headerViewFile = "Parts/_Header")
        {
            var renderService = GetViewRenderer();
            var content = renderService.RenderToStringAsync(typeof(TModel), headerViewFile, Model).Result;
            return new HtmlContentBuilder().Append(content);
        }

        public IHtmlContent NccRenderNavigation(string navigationViewFile = "Parts/_Navigation")
        {
            var renderService = GetViewRenderer();
            var content = renderService.RenderToStringAsync(typeof(TModel), navigationViewFile, Model).Result;
            return new HtmlContentBuilder().Append(content);
        }

        public IHtmlContent NccRenderFeaturedSection(string featuredViewFile = "Parts/_Featured")
        {
            var renderService = GetViewRenderer();
            var content = renderService.RenderToStringAsync(typeof(TModel), featuredViewFile, Model).Result;
            return new HtmlContentBuilder().Append(content);
        }

        public IHtmlContent NccRenderLeftColumn(string leftColumnViewFile = "Parts/_LeftColumn")
        {
            var renderService = GetViewRenderer();
            var content = renderService.RenderToStringAsync(typeof(TModel), leftColumnViewFile, Model).Result;
            return new HtmlContentBuilder().Append(content);
        }
        
        public IHtmlContent NccRenderBody()
        {
            return RenderBody();
        }

        public IHtmlContent NccRenderRightColumn(string rightColumnViewFile = "Parts/_RightColumn")
        {
            var renderService = GetViewRenderer();
            var content = renderService.RenderToStringAsync(typeof(TModel), rightColumnViewFile, Model).Result;
            return new HtmlContentBuilder().Append(content);
        }

        public IHtmlContent NccRenderFooter(string footerViewFile = "Parts/_Footer")
        {
            var renderService = GetViewRenderer();
            var content = renderService.RenderToStringAsync(typeof(TModel), footerViewFile, Model).Result;
            return new HtmlContentBuilder().Append(content);
        }

        public IHtmlContent NccRenderFooterCss()
        {
            throw new System.Exception();
        }

        public IHtmlContent NccRenderFooterScripts()
        {
            throw new System.Exception();
        }
    }
}

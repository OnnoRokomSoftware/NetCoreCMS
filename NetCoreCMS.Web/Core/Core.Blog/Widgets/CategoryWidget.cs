/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Core.Blog.Controllers;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules.Widgets;
using Core.Blog.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetCoreCMS.Framework.Core.Serialization;

namespace Core.Blog.Widgets
{
    public class CategoryWidget : Widget
    {
        NccCategoryService _nccCategoryService;
        IViewRenderService _viewRenderService;
        NccWebSiteWidgetService _websiteWidgetService;
        bool ShowCategoryHasPost = false;
        bool ShowPostCount = false;
        bool ShowHierarchy = false;
        bool DisplayAsDropdown = false;

        public CategoryWidget(
            IViewRenderService viewRenderService,
            NccWebSiteWidgetService websiteWidgetService,
            NccCategoryService nccCategoryService) : base(                
                "Category",
                "This is a widget to display category.",
                "",
                true
            )
        {
            _viewRenderService = viewRenderService;
            _websiteWidgetService = websiteWidgetService;
            _nccCategoryService = nccCategoryService;
        }

        public override void Init(long websiteWidgetId, bool renderConfig = false)
        {
            WebSiteWidgetId = websiteWidgetId;
            ViewFileName = "Widgets/Category";

            var webSiteWidget = _websiteWidgetService.Get(websiteWidgetId, true);
            if (webSiteWidget != null && !string.IsNullOrEmpty(webSiteWidget.WidgetConfigJson))
            {
                var configJson = webSiteWidget.WidgetConfigJson;
                var config = JsonHelper.Deserilize<dynamic>(configJson);
                DisplayTitle = config.title;
                Footer = config.footer;
                Language = config.language;

                try
                {
                    string temp = config.showCategoryHasPost;
                    ShowCategoryHasPost = (temp == "on") ? true : false;

                    temp = config.showPostCount;
                    ShowPostCount = (temp == "on") ? true : false;

                    temp = config.showHierarchy;
                    ShowHierarchy = (temp == "on") ? true : false;

                    temp = config.displayAsDropdown;
                    DisplayAsDropdown = (temp == "on") ? true : false;
                }
                catch (Exception) { }
            }

            if (renderConfig)
            {
                ConfigViewFileName = "Widgets/CategoryConfig";
                ConfigHtml = _viewRenderService.RenderToStringAsync<BlogController>(ConfigViewFileName, webSiteWidget).Result;
            }
        }

        public override string RenderBody()
        {
            var categoryList = _nccCategoryService.LoadAllWithPost();
            CategoryViewModel item = new CategoryViewModel();
            item.ShowCategoryHasPost = ShowCategoryHasPost;
            item.ShowPostCount = ShowPostCount;
            item.ShowHierarchy = ShowHierarchy;
            item.DisplayAsDropdown = DisplayAsDropdown;
            item.CategoryList = categoryList;
            var body = _viewRenderService.RenderToStringAsync<BlogController>(ViewFileName, item).Result;
            return body;
        }
    }
}
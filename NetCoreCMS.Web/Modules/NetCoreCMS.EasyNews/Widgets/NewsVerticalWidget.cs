/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.EasyNews.Models.ViewModels;
using NetCoreCMS.EasyNews.Services;
using System;
using System.Linq;
using NetCoreCMS.EasyNews.Controllers;

namespace NetCoreCMS.EasyNews.Widgets
{
    public class NewsVerticalWidget : Widget
    {
        NewsService _neNewsService;        
        int newsCount = 10;

        string headerTitle = "";
        string headerColor = "";
        string headerBgColor = "";

        string category = "";
        string columnClass = "";
        string columnColor = "";
        string columnBgColor = "";
        string scrollamount = "5";
        string height = "100";

        string footerTitle = "";
        string footerColor = "";
        string footerBgColor = "";

        public NewsVerticalWidget(NewsService neNewsService) : base(
                typeof(EasyNewsController),
                "Vertical News",
                "This is a widget to scroll news vertically.",
                "",
                "Widgets/NewsVertical",
                "Widgets/NewsConfig",
                false
            )
        {
            _neNewsService = neNewsService;
        }

        public override void InitConfig(dynamic config)
        {
            category = config.category;
            try
            {
                newsCount = Convert.ToInt32(config.newsCount);
            }
            catch (Exception) { newsCount = 10; }

            headerTitle = config.headerTitle;
            headerColor = config.headerColor;
            headerBgColor = config.headerBgColor;
                
            columnClass = config.columnClass;
            columnColor = config.columnColor;
            columnBgColor = config.columnBgColor;
            scrollamount = config.scrollamount;
            height = config.height;

            footerTitle = config.footerTitle;
            footerColor = config.footerColor;
            footerBgColor = config.footerBgColor;            
        }

        public override object PrepareViewModel()
        {
            var itemList = _neNewsService.LoadAllByCategory(category);
            if (category.Trim() == "")
            {
                itemList = _neNewsService.LoadAll()
                    .Where(x => x.Status >= EntityStatus.Active && (x.HasDateRange == false || (x.PublishDate >= DateTime.Now && x.ExpireDate <= DateTime.Now)))
                    .OrderByDescending(x => x.Id)
                    .Take(newsCount)
                    .OrderBy(x => x.Order)
                    .ToList();
            }
            var model = new NewsViewModel()
            {
                HeaderTitle = headerTitle,
                HeaderColor = headerColor,
                HeaderBgColor = headerBgColor,

                ColumnClass = columnClass.Trim() == "" ? "" : columnClass,
                ColumnColor = columnColor,
                ColumnBgColor = columnBgColor,
                Scrollamount = scrollamount,
                Height = height.Trim() == "" ? "100" : height,

                FooterTitle = footerTitle,
                FooterColor = footerColor,
                FooterBgColor = footerBgColor,
                NewsList = itemList
            };            
            return model;
        }
    }
}
/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules.Widgets;
using Core.Blog.Models;
using System;
using Core.Blog.Controllers;

namespace Core.Blog.Widgets
{
    public class RecentPostWidget : Widget
    {
        NccPostService _nccPostService;        
        int PostCount = 5;
        bool IsDateShow = false;

        public RecentPostWidget(NccPostService nccPostService) : base(
                typeof(BlogController),
                "Recent Post",
                "This is a widget to display recent blog posts.",
                "",
                "Widgets/RecentPost",
                "Widgets/RecentPostConfig",
                true )
        {
            _nccPostService = nccPostService;
        }

        public override void InitConfig(dynamic config)
        {
            try
            {
                string pc = config.postCount;
                PostCount = string.IsNullOrEmpty(pc) ? 5 : Convert.ToInt32(pc);
                string ds = config.isDateShow;
                if (ds == "on")
                    IsDateShow = true;
                else
                    IsDateShow = false;
            }
            catch (Exception) { PostCount = 5; }

        }

        public override object PrepareViewModel()
        {
            var postList = _nccPostService.LoadRecentPages(PostCount);
            RecentPostViewModel model = new RecentPostViewModel();
            model.IsDateShow = IsDateShow;
            model.PostList = postList;            
            return model;
        }
    }
}
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
using System.Linq;

namespace Core.Blog.Widgets
{
    public class BlogPostSliderWidget : Widget
    {
        NccCategoryService _nccCategoryService;
        NccPostService _nccPostService;
        public BlogPostSliderViewModel bpsViewModel = new BlogPostSliderViewModel();

        public BlogPostSliderWidget(NccCategoryService nccCategoryService, NccPostService nccPostService) : base(
                typeof(BlogController),
                "Blog Post Slider",
                "This is a widget to display blog posts in slider.",
                "",
                "Widgets/BlogPostSlider",
                "Widgets/BlogPostSliderConfig",
                false)
        {
            _nccCategoryService = nccCategoryService;
            _nccPostService = nccPostService;
        }

        public override void InitConfig(dynamic config)
        {
            bpsViewModel.HeaderText = config.headerText;

            string temp = config.disableAutoScroll;
            bpsViewModel.IsAutoScroll = (string.IsNullOrWhiteSpace(temp) == false && temp == "on") ? false : true;

            temp = config.interval;
            bpsViewModel.Interval = string.IsNullOrWhiteSpace(temp) ? bpsViewModel.Interval : Convert.ToInt32(temp);

            temp = config.totalPost;
            bpsViewModel.TotalPost = string.IsNullOrWhiteSpace(temp) ? bpsViewModel.TotalPost : Convert.ToInt32(temp);

            temp = config.displayPost;
            bpsViewModel.DisplayPost = string.IsNullOrWhiteSpace(temp) ? bpsViewModel.DisplayPost : Convert.ToInt32(temp);

            temp = config.showSideNav;
            bpsViewModel.ShowSideNav = (string.IsNullOrWhiteSpace(temp) == false && temp == "on") ? true : false;

            temp = config.showBottomNav;
            bpsViewModel.ShowBottomNav = (string.IsNullOrWhiteSpace(temp) == false && temp == "on") ? true : false;


            temp = config.isFeatured;
            bpsViewModel.IsFeatured = (string.IsNullOrWhiteSpace(temp) == false && temp == "on") ? true : false;

            temp = config.isSticky;
            bpsViewModel.IsSticky = (string.IsNullOrWhiteSpace(temp) == false && temp == "on") ? true : false;


            temp = config.categoryId;
            bpsViewModel.CategoryId = string.IsNullOrWhiteSpace(temp) ? bpsViewModel.CategoryId : Convert.ToInt32(temp);
        }

        public override object PrepareConfigModel()
        {
            bpsViewModel.CategoryList = _nccCategoryService.LoadAll(true).ToList();
            return bpsViewModel;
        }

        public override object PrepareViewModel()
        {
            if (bpsViewModel.CategoryId > 0)
            {
                if (bpsViewModel.IsSticky || bpsViewModel.IsFeatured)
                {
                    bpsViewModel.PostList = _nccPostService.LoadSpecialPosts(bpsViewModel.IsSticky, bpsViewModel.IsFeatured).Where(x => x.Categories.Any(y => y.CategoryId == bpsViewModel.CategoryId)).ToList();
                }
                else
                {
                    bpsViewModel.PostList = _nccPostService.Load(0, bpsViewModel.TotalPost, true, true, true, true, null, null, bpsViewModel.CategoryId);
                }
            }
            else
            {
                if (bpsViewModel.IsSticky || bpsViewModel.IsFeatured)
                {
                    bpsViewModel.PostList = _nccPostService.LoadSpecialPosts(bpsViewModel.IsSticky, bpsViewModel.IsFeatured);
                }
                else
                {
                    bpsViewModel.PostList = _nccPostService.Load(0, bpsViewModel.TotalPost, true, true);
                }
            }
            return bpsViewModel;
        }
    }
}
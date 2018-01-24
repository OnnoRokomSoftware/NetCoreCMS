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
    public class TagCloudWidget : Widget
    {
        NccTagService _nccTagService;       
        bool ShowTagHasPost = false;
        bool ShowPostCount = false;

        public TagCloudWidget(NccTagService nccTagService) : base(
                typeof(BlogController),
                "Tag Cloud",
                "This is a widget to display Tags Cloud.",
                "",
                "Widgets/TagCloud",
                "Widgets/TagCloudConfig",
                true
            )
        { 
            _nccTagService = nccTagService;
        }

        public override void InitConfig(dynamic config)
        {
            try
            {
                string temp = config.showPostCount;
                ShowPostCount = (temp == "on") ? true : false;
                temp = config.showTagHasPost;
                ShowTagHasPost = (temp == "on") ? true : false;
            }
            catch (Exception) { }            
        }

        public override object PrepareViewModel()
        {
            var itemList = _nccTagService.LoadTagCloud();
            if (itemList != null && itemList.Count > 0)
            {
                TagCloudViewModel model = new TagCloudViewModel();
                model.ShowTagHasPost = ShowTagHasPost;
                model.ShowPostCount = ShowPostCount;
                model.ItemList = itemList;                
                return model;
            }

            return "";
        }
    }
}
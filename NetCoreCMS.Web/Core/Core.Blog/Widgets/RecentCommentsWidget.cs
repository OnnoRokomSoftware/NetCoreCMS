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
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules.Widgets;
using System;
using System.Collections.Generic;

namespace Core.Blog.Widgets
{
    public class RecentCommentsWidget : Widget
    {
        NccCommentsService _nccCommentsService;        
        int CommentsCount = 5;

        public RecentCommentsWidget(            
            NccCommentsService nccCommentsService) : base(
                typeof(BlogController),
                "Recent Comments",
                "This is a widget to display recent blog Comments.",
                "",
                "Widgets/RecentComments",
                "Widgets/RecentCommentsConfig",
                true
            )
        {
            _nccCommentsService = nccCommentsService;
        }

        public override void InitConfig(dynamic config)
        {
            try
            {
                string cc = config.commentsCount;
                CommentsCount = string.IsNullOrEmpty(cc) ? 5 : Convert.ToInt32(cc);
            }
            catch (Exception) { CommentsCount = 5; }
        }

        public override object PrepareViewModel()
        {
            List<NccComment> commentsList = _nccCommentsService.LoadRecentComments(CommentsCount);            
            return commentsList;
        }
    }
}
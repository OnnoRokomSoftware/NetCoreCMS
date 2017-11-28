/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/
using NetCoreCMS.Core.Modules.Blog.Controllers;
using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Core.ShotCodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Core.Modules.Cms.ShortCodes
{
    public class PostShortCode : IShortCode
    {
        NccPostService _nccPostService;
        IViewRenderService _viewRenderService;
        string ViewFileName = "Widgets/PostRender";

        public PostShortCode(NccPostService nccPostService, IViewRenderService viewRenderService)
        {
            _nccPostService = nccPostService;
            _viewRenderService = viewRenderService;
        }
        public string ShortCodeName { get { return "Post"; } }

        public string Render(params object[] paramiters)
        {
            var content = "";
            try
            {
                var id = paramiters[0].ToString().Trim();
                var slider = _nccPostService.Get(long.Parse(id));
                if (slider != null)
                {
                    content = _viewRenderService.RenderToStringAsync<BlogController>(ViewFileName, slider).Result;
                }
            }
            catch (Exception ex) { }
            return content;
        }
    }
}

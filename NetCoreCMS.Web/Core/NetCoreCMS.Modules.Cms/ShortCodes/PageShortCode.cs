/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/
using NetCoreCMS.Core.Modules.Cms.Controllers;
using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Core.ShotCodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Core.Modules.Cms.ShortCodes
{
    public class PageShortCode : IShortCode
    {
        NccPageService _nccPageService;
        IViewRenderService _viewRenderService;
        string ViewFileName = "Widgets/PageRender";

        public PageShortCode(NccPageService nccPageService, IViewRenderService viewRenderService)
        {
            _nccPageService = nccPageService;
            _viewRenderService = viewRenderService;
        }
        public string ShortCodeName { get { return "Page"; } }

        public string Render(params object[] paramiters)
        {
            var content = "";
            try
            {
                var id = paramiters[0].ToString().Trim();
                var slider = _nccPageService.Get(long.Parse(id));

                if (slider != null)
                {
                    content = _viewRenderService.RenderToStringAsync<CmsHomeController>(ViewFileName, slider).Result;
                }
            }
            catch (Exception ex) { }
            return content;
        }
    }
}
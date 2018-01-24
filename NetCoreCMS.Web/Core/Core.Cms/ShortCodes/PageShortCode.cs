/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/
using Core.Cms.Controllers;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules.ShortCode;

namespace NetCoreCCore.Cms.ShortCodes
{
    public class PageShortCode : BaseShortCode
    {
        NccPageService _nccPageService;

        public PageShortCode(NccPageService nccPageService) : base(typeof(CmsPageController), "Page", "ShortCodes/PageRender")
        {
            _nccPageService = nccPageService;
        }

        public override object PrepareViewModel(params object[] paramiters)
        {
            var id = paramiters[0].ToString().Trim();
            var model = _nccPageService.Get(long.Parse(id));
            return model;
        }
    }
}
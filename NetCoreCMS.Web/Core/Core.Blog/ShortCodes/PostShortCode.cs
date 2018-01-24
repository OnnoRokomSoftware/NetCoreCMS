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
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules.ShortCode;

namespace Core.Blog.ShortCodes
{
    public class PostShortCode : BaseShortCode
    {
        NccPostService _nccPostService;
        public PostShortCode(NccPostService nccPostService) : base(
            typeof(BlogController),
            "Post",
            "ShortCodes/PostRender")
        {
            _nccPostService = nccPostService;
        }

        public override object PrepareViewModel(params object[] paramiters)
        {
            var id = paramiters[0].ToString().Trim();
            var post = _nccPostService.Get(long.Parse(id));
            return post;
        }
    }
}

/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.Core.ShotCodes;
using NetCoreCMS.Subscription.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Subscription.ShotCodes
{
    public class SubscriptionShotcode : IShortCode
    {
        IViewRenderService _viewRenderService;
        string ViewFileName = "Widgets/SubscriptionWidget";

        public string ShortCodeName { get { return "Subscription"; } }

        public SubscriptionShotcode(IViewRenderService viewRenderService)
        {
            _viewRenderService = viewRenderService;
        }

        public string Render(params object[] paramiters)
        {
            return _viewRenderService.RenderToStringAsync<SubscriptionWidgetController>(ViewFileName, null).Result;
        }
    }
}

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

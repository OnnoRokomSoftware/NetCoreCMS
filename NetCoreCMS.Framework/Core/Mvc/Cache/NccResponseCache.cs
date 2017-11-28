using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NetCoreCMS.Framework.Core.Mvc.Cache
{
    public class NccResponseCache : ActionFilterAttribute
    {
        public int? ClientDuration { get; set; }
        public int? ServerDuration { get; set; }
        public string Tags { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // validation omitted

            if (ClientDuration.HasValue)
            {
                context.HttpContext.Items[NccCacheKeys.ClientDuration] = ClientDuration.Value;
            }

            if (ServerDuration.HasValue)
            {
                context.HttpContext.Items[NccCacheKeys.ServerDuration] = ServerDuration.Value;
            }

            if (!string.IsNullOrWhiteSpace(Tags))
            {
                context.HttpContext.Items[NccCacheKeys.Tags] = Tags;
            }

            base.OnActionExecuting(context);
        }
    }
}

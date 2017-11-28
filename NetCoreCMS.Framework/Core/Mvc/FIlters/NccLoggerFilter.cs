using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace NetCoreCMS.Framework.Core.Mvc.FIlters
{
    public class NccLoggerFilter : IActionFilter
    {
        public int Order { get; set; }
        private readonly ILogger _logger;

        public NccLoggerFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<NccLoggerFilter>();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Items.ContainsKey("NCC_RAZOR_PAGE_PROPERTY_LOGGER"))
            {
                context.HttpContext.Items["NCC_RAZOR_PAGE_PROPERTY_LOGGER"] = _logger;
            }
            else
            {
                context.HttpContext.Items.Add("NCC_RAZOR_PAGE_PROPERTY_LOGGER", _logger);
            }
        }
    }
}

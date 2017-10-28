using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace NetCoreCMS.Framework.Core.Mvc.FIlters
{
    public class NccGlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public NccGlobalExceptionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<NccGlobalExceptionFilter>();
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception.ToString());
        }
    }
}

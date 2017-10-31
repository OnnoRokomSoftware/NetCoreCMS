using Microsoft.AspNetCore.Mvc.Filters;

namespace NetCoreCMS.Framework.Core.Mvc.FIlters
{
    public interface INccActionFilter : IActionFilter
    {
        int Order { get; }
    }
}

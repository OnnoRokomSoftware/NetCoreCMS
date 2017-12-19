using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using NetCoreCMS.Framework.Utility;

namespace NetCoreCMS.Framework.Core.Mvc.Area
{
    public class AreaRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.ContainsKey("area"))
            {
                return false;
            }
            var area = values["area"].ToString().ToLower();
            var exists = GlobalContext.GetActiveModules().Where(x => x.Area.Equals(area)).Count() > 0;

            return exists;
        }
    }
}

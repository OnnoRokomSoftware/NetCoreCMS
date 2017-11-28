using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.Auth
{
    public class ControllerActionCache
    {
        internal static List<ControllerAction> ControllerActions { get; set; } = new List<ControllerAction>();
    }
}

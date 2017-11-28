using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.Mvc.Attributes
{
    /// <summary>
    /// This attribute on any controller action will allow access to that controller action to any logged user.
    /// </summary>
    public class AllowAuthenticated : Attribute
    {

    }
}

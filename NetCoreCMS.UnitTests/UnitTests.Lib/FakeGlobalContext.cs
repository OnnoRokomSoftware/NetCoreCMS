using NetCoreCMS.Framework.Utility;
using System;

namespace UnitTests.Lib
{
    public class FakeGlobalContext
    {
        public static void EnableMultiLanguage()
        {
            GlobalContext.WebSite = FakeNccWebSite.GetNccWebsite();
            GlobalContext.WebSite.IsMultiLangual = true;
        }
        public static void DisableMultiLanguage()
        {
            GlobalContext.WebSite = FakeNccWebSite.GetNccWebsite();
            GlobalContext.WebSite.IsMultiLangual = false;
        }

    }
}

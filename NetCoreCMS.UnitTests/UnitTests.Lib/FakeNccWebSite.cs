using NetCoreCMS.Framework.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.Lib
{
    public class FakeNccWebSite
    {
        public static NccWebSite GetNccWebsite()
        {
            var nccWebSite = new NccWebSite() {
                AllowRegistration = true,
                CreateBy = 1,
                CreationDate = DateTime.Now,
                DateFormat = "dd/MM/yyyy",
                DomainName = "http://dotnetcorecms.com",
                EmailAddress = "info@onnorokomsoftware.com",
                IsMultiLangual = true,
            };
            return nccWebSite;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using NetCoreCMS.Framework.Utility;
using UnitTests.Lib;

namespace NetCoreCMS.Framework.Test
{
    [Trait("NetCoreCMS.Framework", "Utility Test" )]
    public class UtilityTest
    {
        [Fact]
        public void Constant_values_are_remain_same_Test()
        {
            Assert.Equal(Constants.AdminLayoutName, "_AdminLayout");
            Assert.Equal(Constants.AdminUrl, "Admin");
            Assert.Equal(Constants.ModuleConfigFileName, "Module.json");
            Assert.Equal(Constants.NccSiteKey, "E546C8DF278CD5931069B522E695D4F2");
            Assert.Equal(Constants.NotFoundUrl, "/CmsHome/ResourceNotFound");
            Assert.Equal(Constants.SimpleLayoutName, "_SimpleLayout");
            Assert.Equal(Constants.SiteLayoutName, "_SiteLayout");
            Assert.Equal(Constants.SiteUrl, "CmsHome");
            Assert.Equal(Constants.SMTPSettingsKey, "NetCoreCMS_SMTP_Settings");
            Assert.Equal(Constants.ThemeConfigFileName, "Theme.json");
        }

        [Fact]
        public void Add_language_to_url_should_work()
        {
            var url = NccUrlHelper.AddLanguageToUrl("en", "/CmsHome");
            Assert.Equal("/CmsHome", url);

            FakeGlobalContext.EnableMultiLanguage();

            var url2 = NccUrlHelper.AddLanguageToUrl("en", "/CmsHome");
            Assert.Equal("/en/CmsHome", url2);

        }

        [Fact]
        public void Add_language_to_url_should_work_2()
        {
            var url = NccUrlHelper.AddLanguageToUrl("en", "CmsHome");
            Assert.Equal("/CmsHome", url);

            FakeGlobalContext.EnableMultiLanguage();

            var url2 = NccUrlHelper.AddLanguageToUrl("en", "CmsHome");
            Assert.Equal("/en/CmsHome", url2);

            var url3 = NccUrlHelper.AddLanguageToUrl("en", "http://localhost:5000/CmsHome/");
            Assert.Equal("http://localhost:5000/en/CmsHome/", url3);

        }
    }
}

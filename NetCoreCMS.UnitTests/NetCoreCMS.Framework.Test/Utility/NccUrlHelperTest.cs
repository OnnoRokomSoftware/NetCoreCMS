using NetCoreCMS.Framework.Utility;
using UnitTests.Lib;
using Xunit;

namespace NetCoreCMS.Framework.Test.Utility
{
    public class NccUrlHelperTest
    {

        [Fact]
        public void NccUrlHelper_Add_language_to_url_should_work()
        {
            var url = NccUrlHelper.AddLanguageToUrl("en", "/CmsHome");
            Assert.Equal("/CmsHome", url);

            FakeGlobalContext.EnableMultiLanguage();

            var url2 = NccUrlHelper.AddLanguageToUrl("en", "/CmsHome");
            Assert.Equal("/en/CmsHome", url2);

            FakeGlobalContext.DisableMultiLanguage();

            var url3 = NccUrlHelper.AddLanguageToUrl("en", "CmsHome");
            Assert.Equal("CmsHome", url3);

            FakeGlobalContext.EnableMultiLanguage();

            var url4 = NccUrlHelper.AddLanguageToUrl("en", "CmsHome");
            Assert.Equal("/en/CmsHome", url4);

            var url5 = NccUrlHelper.AddLanguageToUrl("en", "http://localhost:5000/CmsHome/");
            Assert.Equal("http://localhost:5000/en/CmsHome/", url5);

            var url6 = NccUrlHelper.AddLanguageToUrl("en", "https://localhost:5000/CmsHome/");
            Assert.Equal("https://localhost:5000/en/CmsHome/", url6);

            var url7 = NccUrlHelper.AddLanguageToUrl("bn", "http://localhost:5000/CmsHome/?query=netcorecms&version=0.4.4");
            Assert.Equal("http://localhost:5000/bn/CmsHome/?query=netcorecms&version=0.4.4", url7);
        }

        [Fact]
        public void NccUrlHelper_Url_encode_should_skip_url_seperators()
        {
            var encUrl = NccUrlHelper.EncodeUrl("https://localhost:44346/bn/Post/টেস্ট-পোস্ট-1");
            Assert.Equal("https://localhost:44346/bn/Post/%e0%a6%9f%e0%a7%87%e0%a6%b8%e0%a7%8d%e0%a6%9f-%e0%a6%aa%e0%a7%8b%e0%a6%b8%e0%a7%8d%e0%a6%9f-1", encUrl);
        }
    }
}

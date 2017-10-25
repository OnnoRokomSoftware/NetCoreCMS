using Xunit;
using NetCoreCMS.Framework.Utility;

namespace NetCoreCMS.Framework.Test
{
    [Trait("NetCoreCMS.Framework", "Utility Test")]
    public class ConstantsTest
    {
        [Fact]
        public void Constant_values_are_remain_same()
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
    }
}

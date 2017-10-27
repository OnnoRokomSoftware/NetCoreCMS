using NetCoreCMS.Framework.Utility;
using UnitTests.Lib;
using Xunit;

namespace NetCoreCMS.Framework.Test.Utility
{
    public class GlobalContextTest
    {
        [Fact]
        public void Should_provide_properties()
        {
            FakeGlobalContext.SetGlobalContextProperties();
            Assert.Null(GlobalContext.App);
            Assert.NotNull(GlobalContext.ContentRootPath);
            Assert.False(GlobalContext.IsRestartRequired);
            Assert.NotNull(GlobalContext.Menus);
            Assert.NotNull(GlobalContext.Modules);
            Assert.Null(GlobalContext.ServiceProvider);
            Assert.Null(GlobalContext.Services);
            Assert.NotNull(GlobalContext.SetupConfig);
            Assert.NotNull(GlobalContext.ShortCodes);
            Assert.NotNull(GlobalContext.Themes);
            Assert.NotNull(GlobalContext.WebRootPath);
            Assert.NotNull(GlobalContext.WebSite);
            Assert.NotNull(GlobalContext.WebSiteWidgets);
            Assert.NotNull(GlobalContext.Widgets);
            Assert.Null(GlobalContext.Configuration);
            Assert.Null(GlobalContext.ConfigurationRoot);
            Assert.Null(GlobalContext.HostingEnvironment);
        }
    }
}

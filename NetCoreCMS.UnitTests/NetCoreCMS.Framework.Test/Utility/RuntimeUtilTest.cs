using NetCoreCMS.Framework.Utility;
using Xunit;

namespace NetCoreCMS.Framework.Test.Utility
{
    public class RuntimeUtilTest
    {
        [Fact]
        public void RuntimeUtil_Net_core_version_should_remain_same()
        {
            var version = RuntimeUtil.GetNetCoreVersion();
            Assert.Equal("2.0.0", version);
        }
    }
}

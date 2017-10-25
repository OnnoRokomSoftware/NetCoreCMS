using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Utility;
using Xunit;

namespace NetCoreCMS.Framework.Test.Utility
{
    public class TypeConverterTest
    {
        public void TypeConverter_Should_parse_values()
        {
            int val1 = TypeConverter.TryParseInt("100");
            Assert.Equal(100, val1);

            var val2 = TypeConverter.TryParseActionTypeEnum("Url");
            Assert.True(val2 == NccMenuItem.ActionType.Url);

            var val3 = TypeConverter.TryParseActionTypeEnum("Tag");
            Assert.True(val3 == NccMenuItem.ActionType.Tag);

            var val4 = TypeConverter.TryParseActionTypeEnum("Page");
            Assert.True(val4 == NccMenuItem.ActionType.Page);

            var val5 = TypeConverter.TryParseActionTypeEnum("Page");
            Assert.True(val5 == NccMenuItem.ActionType.Page);

            var val6 = TypeConverter.TryParseActionTypeEnum("Module");
            Assert.True(val6 == NccMenuItem.ActionType.Module);

            var val7 = TypeConverter.TryParseActionTypeEnum("BlogPost");
            Assert.True(val7 == NccMenuItem.ActionType.BlogPost);

            var val8 = TypeConverter.TryParseActionTypeEnum("BlogCategory");
            Assert.True(val8 == NccMenuItem.ActionType.BlogCategory);

        }
    }
}

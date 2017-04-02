using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Utility
{
    public class TypeConverter
    {
        public static int TryParseInt(string input)
        {
            int value = 0;
            int.TryParse(input, out value);
            return value;
        }
    }
}

/*
*Author: Xonaki
*Website: http://xonaki.com
*Copyright (c) xonaki.com
*License: BSD (3 Clause)
*/

namespace NetCoreCMS.Framework.Core.Mvc.Models
{
    public class EntityStatus
    {
        public static int New { get { return 0; } }
        public static int Modified { get { return 2; } }
        public static int Active { get { return 4; } }
        public static int Inactive { get { return 8; } }
        public static int Deleted { get { return 16; } }
    }
}

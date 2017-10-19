/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

namespace NetCoreCMS.Framework.Core.Mvc.Models
{
    public class EntityStatus
    {
        public static int Deleted { get { return -404; } }
        public static int Inactive { get { return -1; } }        
        public static int Active { get { return 0; } }     
    }
}

/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/
 
using NetCoreCMS.Framework.Core.Models;

namespace NetCoreCMS.Framework.Core.Events.App
{
    public class UserActivity
    {
        public NccUser User{ get; set; }
        public string Ip { get; set; }
        public Type ActivityType { get; set; }

        public enum Type
        {
            Registered,
            Logedin,
            Logedout,
            RoleChanged,
            InActivated,
            EmailConfirmed,
            Activated            
        }
    }
}

/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/
 
using NetCoreCMS.Framework.Core.Models;
using System.Collections.Generic;

namespace NetCoreCMS.Framework.Core.Events.App
{
    public class UserActivity
    {
        public UserActivity()
        {
            NewRoles = new List<string>();
        }

        public NccUser User{ get; set; }
        public string Email { get; set; }
        public IList<string> NewRoles { get; set; }
        public string Ip { get; set; }
        public string ReturnUrl { get; set; }
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

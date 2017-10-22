/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.AspNetCore.Authorization;

namespace NetCoreCMS.Framework.Core.Auth
{
    public class AuthRequire : IAuthorizationRequirement
    {
        public string Role { get; private set; }
        public string User { get; private set; }
        public bool CheckUrlAccess{ get; private set; }
        
        public AuthRequire(string role = "", string user="", bool checkUrlAccess = false)
        {
            Role = role;
            User = user;
            CheckUrlAccess = checkUrlAccess;
        }
    }
}

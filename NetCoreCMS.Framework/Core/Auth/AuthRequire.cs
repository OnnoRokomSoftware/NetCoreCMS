using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

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

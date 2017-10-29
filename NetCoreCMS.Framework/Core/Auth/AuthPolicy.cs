using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.Auth
{
    public class AuthPolicies
    {
        public string NccAuthRequireHandler { get { return "NccAuthRequireHandler"; } }
        public string NccAuthRequireWithValueHandler { get { return "NccAuthRequireWithValueHandler"; } }
    }
}

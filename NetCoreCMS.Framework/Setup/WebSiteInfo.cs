/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Core.Data;

namespace NetCoreCMS.Framework.Setup
{
    public class WebSiteInfo
    {
        public string SiteName { get; set; }
        public string Tagline { get; set; }
        public string AdminUserName { get; set; }
        public string Email { get; set; }
        public string AdminPassword { get; set; }
        public SupportedDatabases Database { get; set; }
        public string ConnectionString { get; set; }
        public string Language { get; set; }
        public string TablePrefix { get; set; }
        public bool EnableCache { get; set; }
    }
}

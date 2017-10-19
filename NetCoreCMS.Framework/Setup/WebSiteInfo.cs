/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
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
        public DatabaseEngine Database { get; set; }
        public string ConnectionString { get; set; }
        public string Language { get; set; }
    }
}

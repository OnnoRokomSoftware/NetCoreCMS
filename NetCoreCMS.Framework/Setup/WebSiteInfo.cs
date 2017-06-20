/*
 * Author: TecRT
 * Website: http://tecrt.com
 * Copyright (c) tecrt.com
 * License: BSD (3 Clause)
*/
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
    }
}

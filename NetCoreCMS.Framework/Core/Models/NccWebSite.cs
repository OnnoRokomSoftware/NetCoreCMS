/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/
 
using NetCoreCMS.Framework.Core.Mvc.Models;
using System.Collections.Generic;
using System;

namespace NetCoreCMS.Framework.Core.Models
{
    [Serializable]
    public class NccWebSite : BaseModel<long>
    {
        public NccWebSite()
        {
            WebSiteInfos = new List<NccWebSiteInfo>();
            WebSitePageSize = 10;
            AdminPageSize = 10;
            TablePrefix = "ncc_";
        }
        
        public string DomainName { get; set; }
        public string EmailAddress { get; set; }
        public bool AllowRegistration { get; set; }
        public string NewUserRole { get; set; }
        public string TimeZone { get; set; }
        public string DateFormat { get; set; }
        public string TimeFormat { get; set; }
        //[Required(ErrorMessage = "Language is Required")]
        public string Language { get; set; }
        public bool IsMultiLangual { get; set; }
        public string GoogleAnalyticsId { get; set; }
        public int WebSitePageSize { get; set; }
        public int AdminPageSize { get; set; }
        public bool IsShowFullPost { get; set; }
        public string TablePrefix { get; set; }

        public bool EnableCache { get; set; }

        public List<NccWebSiteInfo> WebSiteInfos { get; set; }
    }
}

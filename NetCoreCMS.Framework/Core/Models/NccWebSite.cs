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

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccWebSite : BaseModel, IBaseModel<long>
    {
        public NccWebSite()
        {
            WebSiteInfos = new List<NccWebSiteInfo>();
            PerPagePostSize = 10;
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
        public int PerPagePostSize { get; set; }
        public bool IsShowFullPost { get; set; }


        public List<NccWebSiteInfo> WebSiteInfos { get; set; }
    }
}

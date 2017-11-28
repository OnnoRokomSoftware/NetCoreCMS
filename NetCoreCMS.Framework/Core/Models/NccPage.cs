/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;
using System.Collections.Generic;
using NetCoreCMS.Framework.Core.Mvc.Models;

namespace NetCoreCMS.Framework.Core.Models
{
    [Serializable]
    public class NccPage : BaseModel<long>
    { 
        public NccPage()
        {
            PageDetails = new List<NccPageDetails>();
        }
        
        public string Layout { get; set; }
        public DateTime PublishDate { get; set; }
        public int PageOrder { get; set; }

        public NccPageStatus PageStatus { get; set; }
        public NccPageType PageType { get; set; }

        public NccPage Parent { get; set; }

        public List<NccPageDetails> PageDetails { get; set; }

        public enum NccPageStatus
        {
            Draft,
            Reviewed,
            Published,
            UnPublished,
            Archived
        }

        public enum NccPageType
        {
            Public,
            Private,
            PasswordProtected
        }
    }
}

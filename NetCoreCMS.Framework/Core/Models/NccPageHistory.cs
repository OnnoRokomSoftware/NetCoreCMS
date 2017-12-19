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
using static NetCoreCMS.Framework.Core.Models.NccPage;

namespace NetCoreCMS.Framework.Core.Models
{
    [Serializable]
    public class NccPageHistory : BaseModel<long>
    { 
        public NccPageHistory()
        {
            PageDetailsHistory = new List<NccPageDetailsHistory>();
        }
        
        public string Layout { get; set; }
        public DateTime PublishDate { get; set; }
        public int PageOrder { get; set; }

        public NccPageStatus PageStatus { get; set; }
        public NccPageType PageType { get; set; }

        public NccPage Parent { get; set; }
        public long PageId { get; set; }
        public List<NccPageDetailsHistory> PageDetailsHistory { get; set; }
    }
}

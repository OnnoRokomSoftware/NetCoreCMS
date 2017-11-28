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
using System;
using System.Collections.Generic;

namespace NetCoreCMS.Modules.News.Models.Entity
{
    public class NeNews : BaseModel<long>
    {
        public NeNews()
        {
            Order = 1;
            PublishDate = DateTime.Now;
            ExpireDate = DateTime.Now.AddDays(7);
            Details = new List<NeNewsDetails>();
            CategoryList = new List<NeNewsCategory>();
        }

        public bool HasDateRange { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public int Order { get; set; }
        public List<NeNewsDetails> Details { get; set; }
        public List<NeNewsCategory> CategoryList { get; set; }
    }

    public class NeNewsDetails : BaseModel<long>
    {    
        public string Content { get; set; }
        public string Excerpt { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDescription { get; set; }

        public string Language { get; set; }
        public NeNews NeNews { get; set; }
    }
}
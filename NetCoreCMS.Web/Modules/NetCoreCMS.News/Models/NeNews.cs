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

namespace NetCoreCMS.Modules.News.Models
{
    public class NeNews : BaseModel<long>
    {
        public NeNews()
        {
            Order = 1;
            PublishDate = DateTime.Now;
            ExpireDate = DateTime.Now.AddDays(7);
            CategoryList = new List<NeNewsCategory>();
        } 

        public string Content { get; set; }
        public string Excerpt { get; set; }
        public bool HasDateRange { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime ExpireDate{ get; set; }
        public int Order { get; set; }
        public List<NeNewsCategory> CategoryList { get; set; }
    }
}
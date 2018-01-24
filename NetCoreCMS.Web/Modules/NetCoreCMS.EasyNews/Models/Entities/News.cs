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
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.EasyNews.Models.Entities
{
    public class News : BaseModel<long>
    {
        public News()
        {
            Order = 1;
            PublishDate = DateTime.Now;
            ExpireDate = DateTime.Now.AddDays(7);
            Details = new List<NewsDetails>();
            CategoryList = new List<NewsCategory>();
        }

        public bool HasDateRange { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public int Order { get; set; }
        public List<NewsDetails> Details { get; set; }
        public List<NewsCategory> CategoryList { get; set; }
    }

    public class NewsDetails : BaseModel<long>
    {    
        [MaxLength(int.MaxValue)]
        public string Content { get; set; }
        public string Excerpt { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDescription { get; set; }

        public string Language { get; set; }
        public News News { get; set; }
    }
}
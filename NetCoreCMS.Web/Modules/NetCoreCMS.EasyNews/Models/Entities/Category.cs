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

namespace NetCoreCMS.EasyNews.Models.Entities
{
    public class Category : BaseModel<long>
    {
        public Category()
        {
            Details = new List<CategoryDetails>();
            NewsList = new List<NewsCategory>();
        }

        public List<CategoryDetails> Details { get; set; }
        public List<NewsCategory> NewsList { get; set; }
    }

    public class CategoryDetails : BaseModel<long>
    {
        public string Language { get; set; }
        public Category Category { get; set; }
    }
}
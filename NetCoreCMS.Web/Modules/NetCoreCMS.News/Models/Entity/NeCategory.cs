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

namespace NetCoreCMS.Modules.News.Models.Entity
{
    public class NeCategory : BaseModel<long>
    {
        public NeCategory()
        {
            Details = new List<NeCategoryDetails>();
            NewsList = new List<NeNewsCategory>();
        }

        public List<NeCategoryDetails> Details { get; set; }
        public List<NeNewsCategory> NewsList { get; set; }
    }

    public class NeCategoryDetails : BaseModel<long>
    {
        public string Language { get; set; }
        public NeCategory NeCategory { get; set; }
    }
}
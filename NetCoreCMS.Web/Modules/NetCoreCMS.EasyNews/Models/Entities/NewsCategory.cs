/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

namespace NetCoreCMS.EasyNews.Models.Entities
{
    public class NewsCategory
    {
        public long CategoryId { get; set; }
        public Category Category { get; set; }
        public long NewsId { get; set; }
        public News News { get; set; }
    }
}
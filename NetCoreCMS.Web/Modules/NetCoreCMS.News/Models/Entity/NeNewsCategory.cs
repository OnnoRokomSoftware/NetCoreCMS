/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreCMS.Modules.News.Models.Entity
{
    public class NeNewsCategory
    {
        public long NeCategoryId { get; set; }
        //[NotMapped]
        public NeCategory NeCategory { get; set; }
        public long NeNewsId { get; set; }
        //[NotMapped]
        public NeNews NeNews { get; set; }
    }
}
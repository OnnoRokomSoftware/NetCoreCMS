/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccPostTag
    {
        public long PostId { get; set; }
        public long TagId { get; set; }

        public NccPost Post { get; set; }

        public NccTag Tag { get; set; }
    }
}

/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.Modules.Subscription.Models;

namespace NetCoreCMS.Modules.Subscription
{
    public class SubscriptionUserRepository : BaseRepository<SubscriptionUser, long>
    {
        public SubscriptionUserRepository(NccDbContext context) : base(context)
        {
        }
    }
}

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

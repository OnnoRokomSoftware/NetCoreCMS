using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Modules.Subscription.Models;

namespace NetCoreCMS.Modules.Subscription.Models
{
    public class SubscriptionModelBuilder : INccModuleBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubscriptionUser>().ToTable("Ncc_Subscription_User");

        }
    }
}

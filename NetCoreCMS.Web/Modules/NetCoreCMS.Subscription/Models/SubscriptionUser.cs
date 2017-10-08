using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Modules.Subscription.Models
{
    public class SubscriptionUser : BaseModel, IBaseModel<long>
    {
        public SubscriptionUser()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.Active;
            VersionNumber = 1;
        } 

        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Remarks { get; set; }
    }
}
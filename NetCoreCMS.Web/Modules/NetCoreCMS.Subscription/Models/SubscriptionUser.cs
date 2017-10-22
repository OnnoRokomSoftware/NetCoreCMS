/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Utility;

namespace NetCoreCMS.Modules.Subscription.Models
{
    public class SubscriptionUser : BaseModel<long>
    {
        public SubscriptionUser()
        {
        } 

        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Remarks { get; set; }
    }
}
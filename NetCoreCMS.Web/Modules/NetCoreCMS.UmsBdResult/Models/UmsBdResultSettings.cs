/*
 * Author: OnnoRokom Software Ltd
 * Website: http://onnorokomsoftware.com
 * Copyright (c) onnorokomsoftware.com
 * License: Commercial
*/
using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.UmsBdResult.Models
{
    public class UmsBdResultSettings : BaseModel, IBaseModel<long>
    {
        public UmsBdResultSettings()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.Active;
            VersionNumber = 1;
        }
        
        [Required]
        public string ApiKey { get; set; }
        [Required]
        public string BaseApi { get; set; }
        [Required]
        public string OrgBusinessId { get; set; }        
    }
}

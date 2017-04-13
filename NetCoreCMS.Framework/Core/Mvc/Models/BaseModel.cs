/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using NetCoreCMS.Framework.Core.Mvc.Extensions;

namespace NetCoreCMS.Framework.Core.Mvc.Models
{
    public class BaseModel : ValidateableModel, IBaseModel<long>
    {
        public BaseModel()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = GetCurrentUserId();
            Status = EntityStatus.New;
            VersionNumber = 1;
        }

        [Key]
        public long Id { get; set; }
        public int VersionNumber { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public long CreateBy { get; set; }
        public long ModifyBy { get; set; }
        public int Status { get; set; }

        public long GetCurrentUserId()
        {
            HttpContextAccessor hca = new HttpContextAccessor();
            long? userId = hca.HttpContext?.User?.GetUserId();
            if (userId == null)
                return 0;
            return userId.Value;
        }
    }
}
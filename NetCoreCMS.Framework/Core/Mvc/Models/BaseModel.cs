/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
using System;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Framework.Core.Mvc.Models
{
    public class BaseModel : ValidateableModel, IBaseModel<long>
    {
        [Key]
        public long Id { get; set; }
        public int VersionNumber { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public long CreateBy { get; set; }
        public long ModifyBy { get; set; }
        public int Status { get; set; }
    }
}
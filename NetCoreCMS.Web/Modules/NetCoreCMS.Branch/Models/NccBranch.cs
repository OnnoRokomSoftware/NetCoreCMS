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

namespace NetCoreCMS.Branch.Models
{
    public class NccBranch : BaseModel, IBaseModel<long>
    {
        public NccBranch()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.Active;
            VersionNumber = 1;
        } 

        public string Address { get; set; }
        [MaxLength(int.MaxValue)]
        public string Description { get; set; }
        public string Phone { get; set; }
        public string LatLon { get; set; }
        public string GoogleLocation { get; set; }
        public int ZoomLevel { get; set; }
        public bool IsAdmissionOnly { get; set; }
        public int Order { get; set; }
    }
}

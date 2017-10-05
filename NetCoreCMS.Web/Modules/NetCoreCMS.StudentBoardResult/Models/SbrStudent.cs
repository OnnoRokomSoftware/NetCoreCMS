using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Modules.StudentBoardResult.Models
{
    public class SbrStudent : BaseModel, IBaseModel<long>
    {
        public SbrStudent()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.Active;
            VersionNumber = 1;
        }

        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string MobileNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Imagepath { get; set; }
        public Gender Gender { get; set; }
        public Religion Religion { get; set; }
    }

    public enum Religion
    {
        Islam,
        Christianity,
        Buddhism,
        Hinduism,
        Others
    }

    public enum Gender
    {
        Male,
        Female,
        Other
    }
}
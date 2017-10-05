using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Modules.StudentBoardResult.Models
{
    public class SbrStudentResult : BaseModel, IBaseModel<long>
    {
        public SbrStudentResult()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.Active;
            VersionNumber = 1;
        }

        public string RollNo { get; set; }
        public string RegistrationNo { get; set; }
        public string Year { get; set; }
        public double Gpa { get; set; }
        public double GpaWithout4th { get; set; }
        public string Grade { get; set; }

        public SbrExam Exam { get; set; }
        public SbrBoard Board { get; set; }
        public SbrGroup Group { get; set; }
        public SbrStudent Student { get; set; }
    }    
}
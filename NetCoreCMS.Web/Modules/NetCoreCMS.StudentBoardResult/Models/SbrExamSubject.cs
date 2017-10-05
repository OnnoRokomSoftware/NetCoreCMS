using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Modules.StudentBoardResult.Models
{
    public class SbrExamSubject : BaseModel, IBaseModel<long>
    {
        public SbrExamSubject()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.Active;
            VersionNumber = 1;
        }

        [Required]
        public string SubjectCode { get; set; }
        public int Order { get; set; }
        public SbrExam Exam { get; set; }
    }
}
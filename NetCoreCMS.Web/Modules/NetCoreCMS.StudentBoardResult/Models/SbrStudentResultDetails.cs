using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Modules.StudentBoardResult.Models
{
    public class SbrStudentResultDetails : BaseModel, IBaseModel<long>
    {
        public SbrStudentResultDetails()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.Active;
            VersionNumber = 1;
        }
                
        public double Gpa { get; set; }        
        public string Grade { get; set; }

        public SbrStudentResult StudentResult { get; set; }
        public SbrExamSubject ExamSubject { get; set; }
    }    
}
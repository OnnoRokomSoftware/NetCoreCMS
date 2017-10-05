using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Modules.OnlineExam.Models
{
    public class OeStudentQuestionSet : BaseModel, IBaseModel<long>
    {
        public OeStudentQuestionSet()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.Active;
            VersionNumber = 1;
            Correct = 0;
            Incorrect = 0;
            NotAnswered = 0;
            TotalMarks = 0;
        }

        public OeStudent Student { get; set; }
        public OeExam Exam { get; set; }
        public int Version { get; set; }
        public int Correct { get; set; }
        public int Incorrect { get; set; }
        public int NotAnswered { get; set; }
        public double TotalMarks { get; set; }
    }
}
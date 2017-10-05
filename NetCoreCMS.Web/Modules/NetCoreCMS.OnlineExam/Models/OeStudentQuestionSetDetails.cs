using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Modules.OnlineExam.Models
{
    public class OeStudentQuestionSetDetails : BaseModel, IBaseModel<long>
    {
        public OeStudentQuestionSetDetails()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.Active;
            VersionNumber = 1;
        }
        
        public OeStudentQuestionSet StudentQuestionSet{ get; set; }
        public OeQuestion Question { get; set; }
        public int SetSerial { get; set; }
        public string StudentAnswer { get; set; }
        public string CorrectAnswer { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string OptionE { get; set; }
    }
}
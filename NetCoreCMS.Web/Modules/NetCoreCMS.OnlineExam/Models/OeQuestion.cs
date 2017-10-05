using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Modules.OnlineExam.Models
{
    public class OeQuestion : BaseModel, IBaseModel<long>
    {
        public OeQuestion()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.Active;
            VersionNumber = 1;
        }

        public OeExam Exam { get; set; }
        public OeSubject Subject { get; set; }
        public int Version { get; set; }
        public OeUddipok Uddipok { get; set; }
        public int UniqueSetNumber { get; set; }
        public int QuestionSerial { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string OptionE { get; set; }
        public string CorrectAnswer { get; set; }
        public string Solve { get; set; }
        public bool IsShuffle { get; set; }
    }
}
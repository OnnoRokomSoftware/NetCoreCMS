using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Modules.OnlineExam.Models
{
    public class OeExam : BaseModel, IBaseModel<long>
    {
        public OeExam()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.Active;
            VersionNumber = 1;
            TotalTimeInMin = 10;
            IsPublished = false;
            IsRetake = true;
            PublishDateTime = DateTime.Now;
            ExpireDateTime = DateTime.Now.AddDays(7);
        }

        public bool IsBangla { get; set; }
        public bool IsEnglish { get; set; }
        public double TotalMarks { get; set; }
        public int TotalQuestion { get; set; }
        public double MarksPerQuestion { get; set; }
        public double NegativeMarks { get; set; }
        public int TotalUniqueSet { get; set; }
        public int TotalTimeInMin { get; set; }
        public bool IsSubjectWise { get; set; }
        public bool IsCombinedShuffle { get; set; }
        public bool IsPublished { get; set; }
        public bool IsRetake { get; set; }
        public bool HasDateRange { get; set; }
        public DateTime PublishDateTime { get; set; }
        public DateTime ExpireDateTime { get; set; }
    }
}
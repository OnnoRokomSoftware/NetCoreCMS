using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Modules.OnlineExam.Models
{
    public class OeUddipok : BaseModel, IBaseModel<long>
    {
        public OeUddipok()
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
        public int UniqueSetNumber { get; set; }

        public List<OeQuestion> QuestionList { get; set; }
    }
}
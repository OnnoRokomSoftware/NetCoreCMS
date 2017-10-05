using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Modules.OnlineExam.Models
{
    public class OeSubject : BaseModel, IBaseModel<long>
    {
        public OeSubject()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.Active;
            VersionNumber = 1;
        }
    }
}
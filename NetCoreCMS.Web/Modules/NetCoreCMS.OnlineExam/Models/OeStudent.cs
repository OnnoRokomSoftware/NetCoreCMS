using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Modules.OnlineExam.Models
{
    public class OeStudent : BaseModel, IBaseModel<long>
    {
        public OeStudent()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.Active;
            VersionNumber = 1;
        }

        public string PrnNo { get; set; }
        public string RegistrationNo { get; set; }
        public string FullName { get; set; }
    }
}
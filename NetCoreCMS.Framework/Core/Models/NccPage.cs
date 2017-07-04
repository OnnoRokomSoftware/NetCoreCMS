using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccPage : IBaseModel<long>
    {
        public NccPage()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.New;
            VersionNumber = 1;
        }

        [Key]
        public long Id { get; set; }
        public int VersionNumber { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public long CreateBy { get; set; }
        public long ModifyBy { get; set; }
        public int Status { get; set; }
        public NccPage Parent { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Slug { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeyword { get; set; }
        [MaxLength(int.MaxValue)]        
        public string Content { get; set; }
        public List<NccPage> LinkedPages { get; set; }
        public bool AddToNavigationMenu { get; set; }
        public NccPageStatus PageStatus { get; set; }
        public NccPageType PageType { get; set; }
        public string Layout { get; set; }
        public DateTime PublishDate { get; set; }
        public int PageOrder { get; set; }

        public enum NccPageStatus
        {
            Draft,
            Review,
            Published,
            UnPublished,
            Archived
        }

        public enum NccPageType
        {
            Public,
            Private,
            PasswordProtected
        }
    }
}

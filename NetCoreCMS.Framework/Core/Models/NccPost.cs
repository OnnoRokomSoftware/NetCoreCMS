/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccPost : IBaseModel<long>
    {
        public NccPost()
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
        public NccPost Parent { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeyword { get; set; }
        public byte[] Content { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsStiky { get; set; }
        public bool AllowComment { get; set; }
        public string ThumImage { get; set; }
        public NccUser Author { get; set; }
        public NccPostStatus PostStatus { get; set; }
        public List<NccPostCategory> Categories { get; set; }
        public List<NccTag> Tags { get; set; }
        public List<NccPostComment> PostComments { get; set; }
        public NccPostType PostType { get; set; }
        public DateTime PublishDate { get; set; }

        public enum NccPostStatus
        {
            Draft,
            Review,
            Published,
            UnPublished,
            Archived
        }
        public enum NccPostType
        {
            Public,
            Privet,
            PasswordProtected
        }

    }
}

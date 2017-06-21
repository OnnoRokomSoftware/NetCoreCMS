/*
 * Author: TecRT
 * Website: http://tecrt.com
 * Copyright (c) tecrt.com
 * License: BSD (3 Clause)
*/
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccUser : IdentityUser<long, IdentityUserClaim<long>, NccUserRole, IdentityUserLogin<long>>, IBaseModel<long>
    {
        public NccUser()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override long Id { get; set; }
        public string Slug { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public int VersionNumber { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public long CreateBy { get; set; }
        public long ModifyBy { get; set; }
        public int Status { get; set; }
    }
}

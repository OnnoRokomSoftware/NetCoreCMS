/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;
using Microsoft.AspNetCore.Identity;
using NetCoreCMS.Framework.Core.Mvc.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace NetCoreCMS.Framework.Core.Models
{
    [Serializable]
    public class NccUser : IdentityUser<long>, IBaseModel<long>
    {
        public NccUser()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override long Id { get; set; }
        public int VersionNumber { get; set; }
        public string Metadata { get; set; }
        public string Slug { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public long CreateBy { get; set; }
        public long ModifyBy { get; set; }
        public int Status { get; set; }

        /// <summary>
        /// Navigation property for the roles this user belongs to.
        /// </summary>
        public virtual ICollection<NccUserRole> Roles { get; } = new List<NccUserRole>();

        /// <summary>
        /// Navigation property for the claims this user possesses.
        /// </summary>
        public virtual ICollection<IdentityUserClaim<long>> Claims { get; } = new List<IdentityUserClaim<long>>();

        /// <summary>
        /// Navigation property for this users login accounts.
        /// </summary>
        public virtual ICollection<IdentityUserLogin<long>> Logins { get; } = new List<IdentityUserLogin<long>>();

        public virtual List<NccUserPermission> Permissions { get; set; } = new List<NccUserPermission>();
        public virtual List<NccPermissionDetails> ExtraPermissions { get; set; } = new List<NccPermissionDetails>();
        public virtual List<NccPermissionDetails> ExtraDenies { get; set; } = new List<NccPermissionDetails>();

    }
}

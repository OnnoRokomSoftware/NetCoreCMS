/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;

namespace NetCoreCMS.Framework.Core.Mvc.Models
{
    public interface IBaseModel<TId>
    {
        TId Id { get; set; }
        int VersionNumber { get; set; }
        string Metadata { get; set; }
        string Name { get; set; }
        DateTime CreationDate { get; set; }
        DateTime ModificationDate { get; set; }
        long CreateBy { get; set; }
        long ModifyBy { get; set; }
        int Status { get; set; }
    }
}
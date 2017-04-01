using System;

namespace NetCoreCMS.Framework.Core.Mvc.Models
{
    public interface IBaseModel<TId>
    {
        TId Id { get; set; }
        int VersionNumber { get; set; }
        string Name { get; set; }
        DateTime CreationDate { get; set; }
        DateTime ModificationDate { get; set; }
        long CreateBy { get; set; }
        long ModifyBy { get; set; }
        int Status { get; set; }
    }
}
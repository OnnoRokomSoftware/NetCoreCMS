using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.ImageSlider.Models
{
    public class NccImageSlider : IBaseModel<long>
    {
        public NccImageSlider()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.New;
            VersionNumber = 1;
            TotalSlide = 2;
            Interval = 6000;
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

        public string Style { get; set; }
        public int TotalSlide { get; set; }
        public int Interval { get; set; }
        public List<NccImageSliderItem> ImageItems { get; set; }
    }

    public class NccImageSliderItem
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Path { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
    }
}
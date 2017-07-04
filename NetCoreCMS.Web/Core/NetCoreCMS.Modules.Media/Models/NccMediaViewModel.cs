using NetCoreCMS.Framework.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetCoreCMS.Core.Modules.Media.Models
{
    public class NccMediaViewModel
    {
        public string FileName { get; set; }
        public bool IsDir { get; set; }
        public bool IsImage { get; set; }
        public int TotalSubDir { get; set; }
        public int TotalFile { get; set; }
        public string ItemSize { get; set; }
        public string ItemDim { get; set; }
        public string FullPath { get; set; }
        public string ParrentDir { get; set; }
        public DateTime CreationTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetCoreCMS.UmsBdResult.Models
{
    public class UmsBdResultViewModel
    {
        [Required]
        [Display(Name = "Program Roll :")]
        public string PrnNo { get; set; }
        [Required]
        [Display(Name = "Course :")]
        public long SelectedCourse { get; set; }

        [Required]
        [Display(Name = "Exam :")]
        public long ExamId { get; set; }
    }

    public class UmsBdResultIndivisualViewModel
    {
        public virtual string Program { get; set; }
        public virtual string Session { get; set; }
        public virtual string PrnNo { get; set; }
        public virtual string StudentName { get; set; }
        public virtual string ExamName { get; set; }
        public virtual string McqMarks { get; set; }
        public virtual string WrittenMarks { get; set; }
        public virtual string TotalMarks { get; set; }
        public virtual string HighestMarks { get; set; }
        public virtual string FullMarks { get; set; }
        public virtual string Bmp { get; set; }
        public virtual string Cmp { get; set; }
        public virtual bool IsWord { get; set; }
    }

    public class UmsBdResultSolveSheetViewModel
    {
        public virtual string ExamName { get; set; }
        public virtual string ExamVersion { get; set; }
        public virtual string FileContent { get; set; }
        public virtual string Content { get; set; }
    }
}

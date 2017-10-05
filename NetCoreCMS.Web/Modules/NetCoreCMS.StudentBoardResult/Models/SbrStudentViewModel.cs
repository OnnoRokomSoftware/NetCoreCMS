using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Modules.StudentBoardResult.Models
{
    public class SbrStudentViewModel
    {
        public SbrStudentViewModel()
        {
            StudentId = 0;
            SscResultId = 0;
            HscResultId = 0;
            GenderId = 0;
            ReligionId = 0;
            Status = EntityStatus.Active;
            SscResultId = 0;
            SscBoardId = 0;
            SscGroupId = 0;
            HscResultId = 0;
            HscBoardId = 0;
            HscGroupId = 0;
        }

        public long StudentId { get; set; }
        [Required]
        public string Name { get; set; }
        public int Status { get; set; }

        [Required]
        public string FatherName { get; set; }
        [Required]
        public string MotherName { get; set; }
        public string MobileNumber { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        public string Imagepath { get; set; }
        [Required]
        public int GenderId { get; set; }
        public int ReligionId { get; set; }

        [Required]
        public long SscResultId { get; set; }
        [Required]
        public long SscBoardId { get; set; }
        [Required]
        public long SscGroupId { get; set; }
        [Required]
        public string SscRollNo { get; set; }
        [Required]
        public string SscRegistrationNo { get; set; }
        [Required]
        public string SscYear { get; set; }
        [Required]
        public double SscGpa { get; set; }
        [Required]
        public double SscGpaWithout4th { get; set; }
        [Required]
        public string SscGrade { get; set; }

        [Required]
        public long HscResultId { get; set; }
        [Required]
        public long HscBoardId { get; set; }
        [Required]
        public long HscGroupId { get; set; }
        [Required]
        public string HscRollNo { get; set; }
        [Required]
        public string HscRegistrationNo { get; set; }
        [Required]
        public string HscYear { get; set; }
        [Required]
        public double HscGpa { get; set; }
        [Required]
        public double HscGpaWithout4th { get; set; }
        [Required]
        public string HscGrade { get; set; }
    }

}
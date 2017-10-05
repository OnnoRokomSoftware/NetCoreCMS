using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Modules.StudentBoardResult.Models;

namespace NetCoreCMS.Modules.StudentBoardResult.Models
{
    public class SbrModelBuilder : INccModuleBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SbrBoard>().ToTable("Ncc_Sbr_Board");
            modelBuilder.Entity<SbrGroup>().ToTable("Ncc_Sbr_Group");
            modelBuilder.Entity<SbrExam>().ToTable("Ncc_Sbr_Exam");
            modelBuilder.Entity<SbrExamSubject>().ToTable("Ncc_Sbr_Exam_Subject");
            modelBuilder.Entity<SbrStudent>().ToTable("Ncc_Sbr_Student");
            modelBuilder.Entity<SbrStudentResult>().ToTable("Ncc_Sbr_Student_Result");
            modelBuilder.Entity<SbrStudentResultDetails>().ToTable("Ncc_Sbr_Student_Result_Details");

            //modelBuilder.Entity<NeNewsCategory>(b =>
            //{
            //    b.ToTable("Ncc_Ne_News_Category");
            //    b.HasKey(bc => new { bc.NeCategoryId, bc.NeNewsId });
            //});

            //modelBuilder.Entity<NeNewsCategory>()
            //    .HasOne(bc => bc.NeCategory)
            //    .WithMany(b => b.NewsList)
            //    .HasForeignKey(bc => bc.NeCategoryId);

            //modelBuilder.Entity<NeNewsCategory>()
            //    .HasOne(bc => bc.NeNews)
            //    .WithMany(c => c.CategoryList)
            //    .HasForeignKey(bc => bc.NeNewsId);
        }
    }
}

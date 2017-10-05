using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Data;

namespace NetCoreCMS.Modules.OnlineExam.Models
{
    public class OeModelBuilder : INccModuleBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OeSubject>().ToTable("Ncc_OE_Subject");
            modelBuilder.Entity<OeExam>().ToTable("Ncc_OE_Exam");
            modelBuilder.Entity<OeUddipok>().ToTable("Ncc_OE_Uddipok");
            modelBuilder.Entity<OeQuestion>().ToTable("Ncc_OE_Question");
            modelBuilder.Entity<OeStudent>().ToTable("Ncc_OE_Student");
            modelBuilder.Entity<OeStudentQuestionSet>().ToTable("Ncc_OE_Student_Question_Set");
            modelBuilder.Entity<OeStudentQuestionSetDetails>().ToTable("Ncc_OE_Student_Question_Set_Details");
        }
    }
}

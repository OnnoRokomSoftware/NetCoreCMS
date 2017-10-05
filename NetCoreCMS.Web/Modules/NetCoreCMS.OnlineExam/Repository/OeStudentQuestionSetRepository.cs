using System;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.Modules.OnlineExam.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace NetCoreCMS.Modules.OnlineExam.Repository
{
    public class OeStudentQuestionSetRepository : BaseRepository<OeStudentQuestionSet, long>
    {
        public OeStudentQuestionSetRepository(NccDbContext context) : base(context)
        {
        }

        internal OeStudentQuestionSet Get(long studentId, long examId, int version)
        {
            return DbSet.Include("Exam").Include("Student").Where(x => x.Student.Id == studentId && x.Exam.Id == examId && x.Version == version).OrderByDescending(x => x.Id).FirstOrDefault();
        }
    }
}
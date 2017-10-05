using System;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.Modules.StudentBoardResult.Models;
using System.Linq;

namespace NetCoreCMS.Modules.StudentBoardResult.Repository
{
    public class SbrStudentResultRepository : BaseRepository<SbrStudentResult, long>
    {
        public SbrStudentResultRepository(NccDbContext context) : base(context)
        {
        }

        internal SbrStudentResult Get(long studentId, long examId)
        {
            return DbSet.Where(x => x.Student.Id == studentId && x.Exam.Id == examId).FirstOrDefault();
        }
    }
}

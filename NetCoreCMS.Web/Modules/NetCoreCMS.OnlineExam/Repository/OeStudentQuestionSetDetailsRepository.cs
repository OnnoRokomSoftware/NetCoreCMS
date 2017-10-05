using System;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.Modules.OnlineExam.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace NetCoreCMS.Modules.OnlineExam.Repository
{
    public class OeStudentQuestionSetDetailsRepository : BaseRepository<OeStudentQuestionSetDetails, long>
    {
        public OeStudentQuestionSetDetailsRepository(NccDbContext context) : base(context)
        {
        }

        internal List<OeStudentQuestionSetDetails> Load(long studentQuestionSetId)
        {
            var tempDbSet = DbSet.Include("StudentQuestionSet").Include("Question");

            if (studentQuestionSetId > 0)
                tempDbSet = tempDbSet.Where(x => x.StudentQuestionSet.Id == studentQuestionSetId);

            return tempDbSet.ToList();
        }
    }
}
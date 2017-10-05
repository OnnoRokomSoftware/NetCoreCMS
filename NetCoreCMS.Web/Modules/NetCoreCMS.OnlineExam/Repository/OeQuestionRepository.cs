using System;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.Modules.OnlineExam.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace NetCoreCMS.Modules.OnlineExam.Repository
{
    public class OeQuestionRepository : BaseRepository<OeQuestion, long>
    {
        public OeQuestionRepository(NccDbContext context) : base(context)
        {
        }

        internal OeQuestion Get(long examId, long subjectId, int uniqueSetNumber, int questionSerial, int version)
        {
            return DbSet.Include("Exam").Include("Subject").Include("Uddipok").Where(x => x.Exam.Id == examId && x.Subject.Id == subjectId && x.UniqueSetNumber == uniqueSetNumber && x.QuestionSerial == questionSerial && x.Version == version).FirstOrDefault();
        }

        internal List<OeQuestion> Load(long examId = 0, long subjectId = 0, int uniqueSetNumber = 0, int questionSerial = 0, int version = 0, string searchKey = "")
        {
            var tempDbSet = DbSet.Include("Exam").Include("Subject").Include("Uddipok");

            if (examId > 0)
                tempDbSet = tempDbSet.Where(x => x.Exam.Id == examId);
            if (subjectId > 0)
                tempDbSet = tempDbSet.Where(x => x.Subject.Id == subjectId);
            if (uniqueSetNumber > 0)
                tempDbSet = tempDbSet.Where(x => x.UniqueSetNumber == uniqueSetNumber);
            if (questionSerial > 0)
                tempDbSet = tempDbSet.Where(x => x.QuestionSerial == questionSerial);
            if (version > 0)
                tempDbSet = tempDbSet.Where(x => x.Version == version);
            if (!string.IsNullOrEmpty(searchKey))
                tempDbSet = tempDbSet.Where(x => x.Name.Contains(searchKey) || x.OptionA.Contains(searchKey) || x.OptionB.Contains(searchKey) || x.OptionC.Contains(searchKey) || x.OptionD.Contains(searchKey) || x.Solve.Contains(searchKey));
            return tempDbSet.ToList();
        }
    }
}
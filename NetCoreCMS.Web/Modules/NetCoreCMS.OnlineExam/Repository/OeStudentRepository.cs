using System;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.Modules.OnlineExam.Models;
using System.Linq;

namespace NetCoreCMS.Modules.OnlineExam.Repository
{    
    public class OeStudentRepository : BaseRepository<OeStudent, long>
    {
        public OeStudentRepository(NccDbContext context) : base(context)
        {
        }

        internal OeStudent Get(string prn, string reg)
        {
            return DbSet.Where(x => x.PrnNo == prn && x.RegistrationNo == reg).FirstOrDefault();
        }
    }
}

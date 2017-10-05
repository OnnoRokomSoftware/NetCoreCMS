using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Modules.OnlineExam.Repository;
using NetCoreCMS.Modules.OnlineExam.Models;

namespace NetCoreCMS.Modules.OnlineExam.Services
{
    public class OeStudentQuestionSetService : IBaseService<OeStudentQuestionSet>
    {
        private readonly OeStudentQuestionSetRepository _entityRepository;

        public OeStudentQuestionSetService(OeStudentQuestionSetRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public OeStudentQuestionSet Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId, isAsNoTracking, new List<string> { "Exam", "Student" });
        }

        public OeStudentQuestionSet Save(OeStudentQuestionSet entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public OeStudentQuestionSet Update(OeStudentQuestionSet entity)
        {
            var oldEntity = _entityRepository.Get(entity.Id, false);
            if (oldEntity != null)
            {
                using (var txn = _entityRepository.BeginTransaction())
                {
                    CopyNewData(oldEntity, entity);
                    _entityRepository.Edit(oldEntity);
                    _entityRepository.SaveChange();
                    txn.Commit();
                }
            }

            return entity;
        }

        public void Remove(long entityId)
        {
            var entity = _entityRepository.Query().FirstOrDefault(x => x.Id == entityId);
            if (entity != null)
            {
                entity.Status = EntityStatus.Deleted;
                _entityRepository.Edit(entity);
                _entityRepository.SaveChange();
            }
        }

        public List<OeStudentQuestionSet> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch, new List<string> { "Exam", "Student" });
        }

        public void DeletePermanently(long entityId)
        {
            var entity = _entityRepository.Query().FirstOrDefault(x => x.Id == entityId);
            if (entity != null)
            {
                _entityRepository.Remove(entity);
                _entityRepository.SaveChange();
            }
        }

        private void CopyNewData(OeStudentQuestionSet oldEntity, OeStudentQuestionSet entity)
        {
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Name = entity.Name;
            oldEntity.Status = entity.Status;

            oldEntity.Exam = entity.Exam;
            oldEntity.Student = entity.Student;
            oldEntity.Version = entity.Version;
            oldEntity.Correct = entity.Correct;
            oldEntity.Incorrect = entity.Incorrect;
            oldEntity.NotAnswered = entity.NotAnswered;
            oldEntity.TotalMarks = entity.TotalMarks;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Modules.OnlineExam.Repository;
using NetCoreCMS.Modules.OnlineExam.Models;

namespace NetCoreCMS.Modules.OnlineExam.Services
{
    public class OeExamService : IBaseService<OeExam>
    {
        private readonly OeExamRepository _entityRepository;

        public OeExamService(OeExamRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public OeExam Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId, isAsNoTracking);
        }

        public OeExam Save(OeExam entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public OeExam Update(OeExam entity)
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

        public List<OeExam> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch);
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

        private void CopyNewData(OeExam oldEntity, OeExam entity)
        {
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Name = entity.Name;
            oldEntity.Status = entity.Status;

            oldEntity.IsBangla = entity.IsBangla;
            oldEntity.IsEnglish = entity.IsEnglish;
            oldEntity.TotalMarks = entity.TotalMarks;
            oldEntity.TotalQuestion = entity.TotalQuestion;
            oldEntity.MarksPerQuestion = entity.MarksPerQuestion;
            oldEntity.NegativeMarks = entity.NegativeMarks;
            oldEntity.TotalUniqueSet = entity.TotalUniqueSet;
            oldEntity.TotalTimeInMin = entity.TotalTimeInMin;
            oldEntity.IsSubjectWise = entity.IsSubjectWise;
            oldEntity.IsCombinedShuffle = entity.IsCombinedShuffle;
            oldEntity.IsPublished = entity.IsPublished;
            oldEntity.IsRetake = entity.IsRetake;
            oldEntity.HasDateRange = entity.HasDateRange;
            oldEntity.PublishDateTime = entity.PublishDateTime;
            oldEntity.ExpireDateTime = entity.ExpireDateTime;
        }
    }
}
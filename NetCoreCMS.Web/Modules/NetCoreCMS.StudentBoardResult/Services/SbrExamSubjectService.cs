using System;
using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Modules.StudentBoardResult.Repository;
using NetCoreCMS.Modules.StudentBoardResult.Models;
using Microsoft.EntityFrameworkCore;

namespace NetCoreCMS.Modules.StudentBoardResult.Services
{
    public class SbrExamSubjectService : IBaseService<SbrExamSubject>
    {
        private readonly SbrExamSubjectRepository _entityRepository;

        public SbrExamSubjectService(SbrExamSubjectRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public SbrExamSubject Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId, isAsNoTracking, new List<string>() { "Exam" });
            //return _entityRepository.Query().Include("Exam").FirstOrDefault(x => x.Id == entityId);
        }

        public SbrExamSubject Save(SbrExamSubject entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public SbrExamSubject Update(SbrExamSubject entity)
        {
            var oldEntity = _entityRepository.Get(entity.Id, false, new List<string>() { "Exam" });
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

        public List<SbrExamSubject> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch, new List<string>() { "Exam" });
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

        private void CopyNewData(SbrExamSubject oldEntity, SbrExamSubject entity)
        {
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Name = entity.Name;
            oldEntity.Status = entity.Status;

            oldEntity.SubjectCode = entity.SubjectCode;
            oldEntity.Exam = entity.Exam;
            oldEntity.Order = entity.Order;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Framework.Core.Repository;
using Newtonsoft.Json;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccScheduleTaskHistoryService : IBaseService<NccScheduleTaskHistory>
    {
        private readonly NccScheduleTaskHistoryRepository _entityRepository;
         
        public NccScheduleTaskHistoryService(NccScheduleTaskHistoryRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }
         
        public NccScheduleTaskHistory Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId);
        }

        public List<NccScheduleTaskHistory> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch);
        }

        public NccScheduleTaskHistory Save(NccScheduleTaskHistory entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NccScheduleTaskHistory Update(NccScheduleTaskHistory entity)
        {
            var oldEntity = _entityRepository.Get(entity.Id);
            if(oldEntity != null)
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
            var entity = _entityRepository.Get(entityId );
            if (entity != null)
            {
                entity.Status = EntityStatus.Deleted;
                _entityRepository.Edit(entity);
                _entityRepository.SaveChange();
            }
        }

        public void DeletePermanently(long entityId)
        {
            var entity = _entityRepository.Get(entityId);
            if (entity != null)
            {
                _entityRepository.Remove(entity);
                _entityRepository.SaveChange();
            }
        }

        private void CopyNewData(NccScheduleTaskHistory oldEntity, NccScheduleTaskHistory entity)
        {             
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Name = entity.Name; 
            oldEntity.Status = entity.Status; 
            oldEntity.CreateBy = entity.CreateBy;
            oldEntity.CreationDate = entity.CreationDate;              
            oldEntity.Status = entity.Status; 
            oldEntity.VersionNumber = entity.VersionNumber;
            oldEntity.Metadata = entity.Metadata;

            oldEntity.Data = entity.Data;
            oldEntity.TaskCreator = entity.TaskCreator;
            oldEntity.TaskId = entity.TaskId;
            oldEntity.TaskOf = entity.TaskOf;
            oldEntity.TaskType = entity.TaskType; 
        }   
    }
}

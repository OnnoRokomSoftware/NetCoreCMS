using System;
using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.MatGallery.Repository;
using NetCoreCMS.MatGallery.Models;
using Microsoft.EntityFrameworkCore;

namespace NetCoreCMS.MatGallery.Services
{
    public class NccUserModuleService : IBaseService<NccUserModule>
    {
        private readonly NccUserModuleRepository _entityRepository;
        private readonly NccUserModuleLogRepository _logRepository;

        public NccUserModuleService(NccUserModuleRepository entityRepository, NccUserModuleLogRepository logRepository)
        {
            _entityRepository = entityRepository;
            _logRepository = logRepository;
        }

        public NccUserModule Get(long entityId)
        {
            return _entityRepository.Query().FirstOrDefault(x => x.Id == entityId);
        }

        public NccUserModule Save(NccUserModule entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NccUserModule Update(NccUserModule entity)
        {
            var oldEntity = _entityRepository.Query().FirstOrDefault(x => x.Id == entity.Id);
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

        public List<NccUserModule> LoadAll()
        {
            return _entityRepository.Query().ToList();
        }

        public List<NccUserModule> LoadAllActive()
        {
            return _entityRepository.LoadAllActive();
        }

        public List<NccUserModule> LoadAllByStatus(int status)
        {
            return _entityRepository.Query().Where(x => x.Status == status).ToList();
        }

        public List<NccUserModule> LoadAllByName(string name)
        {
            return _entityRepository.Query().Where(x => x.Name == name).ToList();
        }

        public List<NccUserModule> LoadAllByNameContains(string name)
        {
            return _entityRepository.Query().Where(x => x.Name.Contains(name)).ToList();
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

        private void CopyNewData(NccUserModule oldEntity, NccUserModule entity)
        {
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Name = entity.Name;
            oldEntity.Status = entity.Status;

            oldEntity.ModuleVersion = entity.ModuleVersion;
            oldEntity.NetCoreCMSVersion = entity.NetCoreCMSVersion;
            oldEntity.ModuleId = entity.ModuleId;
            oldEntity.ModuleTitle = entity.ModuleTitle;
            oldEntity.Description = entity.Description;
            oldEntity.CategoryList = entity.CategoryList;
            oldEntity.Author = entity.Author;
            oldEntity.Email = entity.Email;
            oldEntity.Website = entity.Website;
        }
    }
}
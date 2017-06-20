using System;
using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Framework.Core.Repository;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccModuleService : IBaseService<NccModule>
    {
        private readonly NccModuleRepository _entityRepository;
         
        public NccModuleService(NccModuleRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }
         
        public NccModule Get(long entityId)
        {
            return _entityRepository.Query().FirstOrDefault(x => x.Id == entityId);
        }

        internal NccModule GetByModuleId(string moduleId)
        {
            return _entityRepository.Query().FirstOrDefault(x => x.ModuleId == moduleId);
        }

        public NccModule Save(NccModule entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NccModule Update(NccModule entity)
        {
            var oldEntity = _entityRepository.Query().FirstOrDefault(x => x.Id == entity.Id);
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
            var entity = _entityRepository.Query().FirstOrDefault(x => x.Id == entityId );
            if (entity != null)
            {
                entity.Status = EntityStatus.Deleted;
                _entityRepository.Edit(entity);
                _entityRepository.SaveChange();
            }
        }

        public List<NccModule> LoadAll()
        {
            return _entityRepository.Query().ToList();
        }

        public List<NccModule> LoadAllByStatus(int status)
        {
            return _entityRepository.Query().Where(x => x.Status == status).ToList();
        }

        public List<NccModule> LoadAllByName(string name)
        {
            return _entityRepository.Query().Where(x => x.Name == name).ToList();
        }

        public List<NccModule> LoadAllByNameContains(string name)
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

        private void CopyNewData(NccModule oldEntity, NccModule entity)
        {
             
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Name = entity.Name; 
            oldEntity.Status = entity.Status;

            oldEntity.AntiForgery = entity.AntiForgery;
            oldEntity.ModuleId = entity.ModuleId;
            oldEntity.CreateBy = entity.CreateBy;
            oldEntity.CreationDate = entity.CreationDate;
            oldEntity.Dependencies = entity.Dependencies;
            
            oldEntity.ModuleStatus = entity.ModuleStatus;
            oldEntity.NetCoreCMSVersion = entity.NetCoreCMSVersion;
            oldEntity.Path = entity.Path;
            
            oldEntity.Status = entity.Status;
            oldEntity.Version = entity.Version;
            oldEntity.VersionNumber = entity.VersionNumber;
            
        }
        
    }
}

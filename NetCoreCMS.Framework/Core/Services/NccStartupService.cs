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
    public class NccStartupService : IBaseService<NccStartup>
    {
        private readonly NccStartupRepository _entityRepository;
         
        public NccStartupService(NccStartupRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }
         
        public NccStartup Get(long entityId)
        {
            return _entityRepository.Get(entityId);
        }

        public NccStartup Get(long roleId, StartupFor startupFor)
        {
            var query = _entityRepository.Query().Where(x => x.RoleId == roleId && x.StartupFor == startupFor);            
            return query.LastOrDefault();
        }

        public NccStartup Save(NccStartup entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NccStartup Update(NccStartup entity)
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

        public List<NccStartup> LoadAll()
        {
            return _entityRepository.LoadAll();
        }

        public List<NccStartup> LoadAllActive()
        {
            return _entityRepository.LoadAllActive();
        }

        public List<NccStartup> LoadAllByStatus(int status)
        {
            return _entityRepository.LoadAllByStatus(status);
        }

        public List<NccStartup> LoadAllByName(string name)
        {
            return _entityRepository.LoadAllByName(name);
        }

        public List<NccStartup> LoadAllByNameContains(string name)
        {
            return _entityRepository.LoadAllByNameContains(name);
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

        private void CopyNewData(NccStartup oldEntity, NccStartup entity)
        {             
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Name = entity.Name; 
            oldEntity.Status = entity.Status; 
            oldEntity.CreateBy = entity.CreateBy;
            oldEntity.CreationDate = entity.CreationDate;  
            
            oldEntity.Status = entity.Status; 
            oldEntity.VersionNumber = entity.VersionNumber;
            oldEntity.Role = entity.Role;
            oldEntity.StartupFor = entity.StartupFor;
            oldEntity.StartupType = entity.StartupType;
            oldEntity.StartupUrl = entity.StartupUrl;
            oldEntity.User = entity.User;            
        }
        
    }
}

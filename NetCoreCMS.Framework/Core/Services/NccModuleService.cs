/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Framework.Core.Repository;
using NetCoreCMS.Framework.Core.Data;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccModuleService : IBaseService<NccModule>
    {
        private readonly NccModuleRepository _entityRepository;
         
        public NccModuleService(NccModuleRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }
         
        public NccModule Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId);
        }

        public List<NccModule> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch);
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

        public List<NccModule> LoadByModuleStatus(NccModule.NccModuleStatus active)
        {
            return _entityRepository.LoadByModuleStatus(active);
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
            oldEntity.MaxNccVersion = entity.MaxNccVersion;
            oldEntity.MinNccVersion = entity.MinNccVersion;
            oldEntity.Path = entity.Path;
            oldEntity.Folder = entity.Folder;
            oldEntity.Status = entity.Status;
            oldEntity.Version = entity.Version;
            oldEntity.VersionNumber = entity.VersionNumber;
            oldEntity.Metadata = entity.Metadata;

        }

        public string ExecuteQuery(NccDbQueryText query)
        {
            string retVal = "";
            using(var txn = _entityRepository.BeginTransaction())
            {
                try
                {
                    var ret = _entityRepository.ExecuteSqlCommand(query);
                    retVal = ret.ToString();
                    txn.Commit();
                }
                catch (Exception ex)
                {
                    txn.Rollback();
                }
            }
            
            return retVal;
        }
    }
}

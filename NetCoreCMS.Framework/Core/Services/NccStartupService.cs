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
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Models.ViewModels;
using NetCoreCMS.Framework.Setup;
using Microsoft.AspNetCore.Identity;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccStartupService : IBaseService<NccStartup>
    {
        private readonly NccStartupRepository _entityRepository;
        private readonly NccPermissionRepository _nccPermissionRepository;
         
        public NccStartupService(NccStartupRepository entityRepository, NccPermissionRepository nccPermissionRepository)
        {
            _entityRepository = entityRepository;
            _nccPermissionRepository = nccPermissionRepository;
        }
         
        public NccStartup Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId);
        }

        public List<NccStartup> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch,new List<string>() { "Permission" });
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
            oldEntity.Metadata = entity.Metadata;

            oldEntity.Status = entity.Status; 
            oldEntity.VersionNumber = entity.VersionNumber;
            oldEntity.Permission = entity.Permission;
            oldEntity.StartupFor = entity.StartupFor;
            oldEntity.StartupType = entity.StartupType;
            oldEntity.StartupUrl = entity.StartupUrl;
            //oldEntity.User = entity.User;            
        }
         
        public void SaveOrUpdate(string startupUrl, string roleStartupType, long[] roles)
        {
            using (var txn = _entityRepository.BeginTransaction())
            {
                foreach (var item in roles)
                {
                    var startup = new NccStartup();
                    var startupType = (StartupType)Enum.Parse(typeof(StartupType), roleStartupType);
                    var existingStartup = Get(item, StartupFor.Role);
                    var permission = _nccPermissionRepository.Get(item);

                    if (existingStartup == null)
                    {
                        startup.Permission = permission;
                        startup.StartupFor = StartupFor.Role;
                        startup.StartupType = startupType;
                        startup.StartupUrl = startupUrl;
                        _entityRepository.Add(startup);
                    }
                    else
                    {
                        existingStartup.Permission = permission;
                        existingStartup.StartupFor = StartupFor.Role;
                        existingStartup.StartupType = startupType;
                        existingStartup.StartupUrl = startupUrl;
                        _entityRepository.Edit(existingStartup);
                    }
                }
                _entityRepository.SaveChange();
                txn.Commit();
            }   
        }
         
        public NccStartup Get(long roleId, StartupFor startupFor)
        {
            var query = _entityRepository.Query().Where(x => x.RoleId == roleId && x.StartupFor == startupFor);            
            return query.LastOrDefault();
        }
    }
}

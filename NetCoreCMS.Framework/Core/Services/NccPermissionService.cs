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
using NetCoreCMS.Framework.Core.IoC;
using NetCoreCMS.Framework.Core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace NetCoreCMS.Framework.Core.Services
{
    /// <summary>
    /// Service for user authorization policy. 
    /// </summary>
    public class NccPermissionService : IBaseService<NccPermission>
    {
        private readonly NccPermissionRepository _entityRepository;

        public NccPermissionService(NccPermissionRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public NccPermission Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId, isAsNoTracking, new List<string>() { "Users", "PermissionDetails" });
        }

        public NccPermission Get(string permissionName)
        {
            var query = _entityRepository.Query()
                .Include("Users")
                .Include("PermissionDetails")
                .Where(x => x.Name == permissionName);
            return query.FirstOrDefault();
        }

        public List<NccPermission> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch, new List<string>() { "Users", "PermissionDetails" });
        } 

        public NccPermission Save(NccPermission entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public List<NccPermission> Save(List<NccPermission> entities)
        {
            using(var txn = _entityRepository.BeginTransaction())
            {
                foreach (var entity in entities)
                {
                    _entityRepository.Add(entity);                    
                }
                _entityRepository.SaveChange();
                txn.Commit();
            }
            
            return entities;
        }

        public NccPermission Update(NccPermission entity)
        {
            var oldEntity = _entityRepository.Get(entity.Id);
            if (oldEntity != null)
            {
                using (var txn = _entityRepository.BeginTransaction())
                {
                    CopyNewData(entity, oldEntity);
                    _entityRepository.Edit(oldEntity);
                    _entityRepository.SaveChange();
                    txn.Commit();
                }
            }

            return entity;
        }

        public void Remove(long entityId)
        {
            var entity = _entityRepository.Get(entityId);
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

        private void CopyNewData(NccPermission copyFrom, NccPermission copyTo)
        {
            copyTo.ModificationDate = copyFrom.ModificationDate;
            copyTo.ModifyBy = copyFrom.ModifyBy;
            copyTo.Name = copyFrom.Name;
            copyTo.Status = copyFrom.Status;
            copyTo.VersionNumber = copyFrom.VersionNumber;
            copyTo.Metadata = copyFrom.Metadata;

            copyTo.Group = copyFrom.Group;
            copyTo.Description = copyFrom.Description;
        }

        public (bool,string) SaveOrUpdate(NccPermission permission, List<long> removePermissionDetailsIdList)
        {
            try
            {
                DoBeforeSave(permission);
                using (var txn = _entityRepository.BeginTransaction())
                {
                    if (permission.Id > 0)
                    {
                        _entityRepository.Edit(permission);
                    }
                    else
                    {
                        _entityRepository.Add(permission);
                    }

                    if (removePermissionDetailsIdList.Count > 0)
                    {
                        var count = _entityRepository.RemoveById(removePermissionDetailsIdList);
                        if (count != removePermissionDetailsIdList.Count) {
                            txn.Rollback();
                            return (false, "Could not remove all disselected menus.");
                        }
                    }

                    _entityRepository.SaveChange();
                    txn.Commit();
                    return (true, "Save successful");
                }
            }
            catch (Exception ex)
            {

                return (false, ex.Message);
            }
        }

        private void DoBeforeSave(NccPermission permission)
        {
            if (permission.Id == 0)
            {
                var existingPermission = _entityRepository.Get(permission.Name);
                if (existingPermission != null)
                {
                    throw new DuplicateRecordException("Name already exist. Please use different name.");
                }
            }
        } 
    }
}
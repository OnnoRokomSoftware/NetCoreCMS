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
using NetCoreCMS.Framework.Core.Repository;
using NetCoreCMS.Framework.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Mvc.Service;

namespace NetCoreCMS.Framework.Core.Services
{
    /// <summary>
    /// Service for user authorization policy. 
    /// </summary>
    public class NccPermissionService : BaseService<NccPermission>
    {
        private readonly NccPermissionRepository _entityRepository;
        private readonly NccPermissionDetailsRepository _permissionDetailsRepository;

        public NccPermissionService(NccPermissionRepository entityRepository, NccPermissionDetailsRepository nccPermissionDetailsRepository) : base(entityRepository, new List<string>() { "Users", "PermissionDetails" })
        {
            _entityRepository = entityRepository;
            _permissionDetailsRepository = nccPermissionDetailsRepository;
        }

        #region New Method
        public NccPermission Get(string permissionName)
        {
            var query = _entityRepository.Query()
                .Include("Users")
                .Include("PermissionDetails")
                .Where(x => x.Name == permissionName);
            return query.FirstOrDefault();
        }

        public List<NccPermission> Save(List<NccPermission> entities)
        {
            using (var txn = _entityRepository.BeginTransaction())
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

        public (bool, string) SaveOrUpdate(NccPermission permission, List<NccPermissionDetails> addedPermissionDetails, List<long> removePermissionDetailsIdList)
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

                    foreach (var item in addedPermissionDetails)
                    {
                        _permissionDetailsRepository.Add(item);
                    }

                    _permissionDetailsRepository.RemoveByIds(removePermissionDetailsIdList);
                    _permissionDetailsRepository.SaveChange();
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

        public void DeletePermission(long id)
        {
            var permission = _entityRepository.Get(id, false, new List<string>() { "PermissionDetails" });
            if (permission != null)
            {
                foreach (var item in permission.PermissionDetails)
                {
                    _permissionDetailsRepository.DeletePermanently(item);
                }
                _permissionDetailsRepository.SaveChange();
                _entityRepository.DeletePermanently(permission);
                _entityRepository.SaveChange();
            }
        } 
        #endregion
    }
}
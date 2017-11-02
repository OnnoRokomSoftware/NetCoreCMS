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
using NetCoreCMS.AdvancedPermission.Models;
using NetCoreCMS.Framework.Core.IoC;

namespace NetCoreCMS.Framework.Core.Services
{
    /// <summary>
    /// Service for user authorization policy. 
    /// </summary>
    public class NccUserAuthorizationService : IBaseService<NccUserAuthorization>, ITransient
    {
        private readonly NccUserAuthorizationRepository _entityRepository;

        public NccUserAuthorizationService(NccUserAuthorizationRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public NccUserAuthorization Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId, isAsNoTracking, new List<string>() { "User" });
        } 

        public List<NccUserAuthorization> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch, new List<string>() { "User" });
        } 

        public NccUserAuthorization Save(NccUserAuthorization entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NccUserAuthorization Update(NccUserAuthorization entity)
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

        private void CopyNewData(NccUserAuthorization copyFrom, NccUserAuthorization copyTo)
        {
            copyTo.ModificationDate = copyFrom.ModificationDate;
            copyTo.ModifyBy = copyFrom.ModifyBy;
            copyTo.Name = copyFrom.Name;
            copyTo.Status = copyFrom.Status;
            copyTo.VersionNumber = copyFrom.VersionNumber;
            copyTo.Metadata = copyFrom.Metadata;
            copyTo.Controller = copyFrom.Controller;
            copyTo.Action= copyFrom.Action;
            copyTo.ModuleId= copyFrom.ModuleId;
            copyTo.User = copyFrom.User;
            copyTo.RequirementName = copyFrom.RequirementName;
            copyTo.RequirementValue = copyFrom.RequirementValue;
        }

        //public List<NccUserAuthPolicy> Search(string name, int count = 20)
        //{
        //    return _entityRepository.Search(name, count);
        //}
    }
}
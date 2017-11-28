/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Framework.Core.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Logging;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccUserService : IBaseService<NccUser>
    {
        private readonly NccUserRepository _entityRepository;
        private readonly NccPermissionRepository _nccPermissionRepository;
        private readonly List<string> userRelations;

        public NccUserService(NccUserRepository entityRepository, NccPermissionRepository nccPermissionRepository)
        {
            _entityRepository = entityRepository;
            _nccPermissionRepository = nccPermissionRepository;
            userRelations = new List<string>() {
                    "Roles",
                    "Roles.Role",
                    "Permissions",
                    "ExtraDenies",
                    "ExtraPermissions",
                    "Permissions.Permission",
                    "Permissions.Permission.PermissionDetails",
                    "Permissions.User"
                };
        }
         
        public NccUser Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(
                entityId,
                isAsNoTracking,
                userRelations
                );

        }

        public List<NccUser> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch);
        }

        public NccUser GetByUserName(string userName)
        {
            var query =  _entityRepository.Query();
            foreach (var item in userRelations)
            {
                query = query.Include(item);
            }
            return query.FirstOrDefault(x => x.UserName == userName);
        }

        public NccUser Save(NccUser entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NccUser Update(NccUser entity)
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

        private void CopyNewData(NccUser oldEntity, NccUser entity)
        { 
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Name = entity.Name; 
            oldEntity.Status = entity.Status; 
            oldEntity.CreateBy = entity.CreateBy;
            oldEntity.CreationDate = entity.CreationDate;  
            
            oldEntity.Status = entity.Status; 
            oldEntity.VersionNumber = entity.VersionNumber;
            oldEntity.AccessFailedCount = entity.AccessFailedCount;
            
            oldEntity.ConcurrencyStamp = entity.ConcurrencyStamp;
            oldEntity.Email = entity.Email;
            oldEntity.EmailConfirmed = entity.EmailConfirmed;
            oldEntity.FullName = entity.FullName;
            oldEntity.LockoutEnabled = entity.LockoutEnabled;
            oldEntity.LockoutEnd = entity.LockoutEnd;
            
            oldEntity.Mobile = entity.Mobile;
            oldEntity.NormalizedEmail = entity.NormalizedEmail;
            oldEntity.NormalizedUserName = entity.NormalizedUserName;
            oldEntity.PasswordHash = entity.PasswordHash;
            oldEntity.PhoneNumber = entity.PhoneNumber;
            oldEntity.PhoneNumberConfirmed = entity.PhoneNumberConfirmed;
            oldEntity.SecurityStamp = entity.SecurityStamp;
            oldEntity.Slug = entity.Slug;
            oldEntity.Status = entity.Status;
            oldEntity.TwoFactorEnabled = entity.TwoFactorEnabled;
            oldEntity.UserName = entity.UserName;
            oldEntity.Metadata = entity.Metadata;
        }

        public List<NccUser> Search(string searchKey)
        {
            return _entityRepository.Search(searchKey);
        }

        public List<(bool IsSuccess,string Message)> UpdateUsersPermission(List<long> userIds, long[] roles)
        {
            var response = new List<(bool,string)>();
            using (var txn = _entityRepository.BeginTransaction())
            {
                foreach (var userId in userIds)
                {
                    var user = Get(userId);
                    if(user != null)
                    {                        
                        //_entityRepository.RemoveUserPermission(user.Permissions);
                        user.Permissions.RemoveAll(x=>x.UserId == user.Id);
                        _entityRepository.Edit(user);
                        _entityRepository.SaveChange();

                        user = Get(userId);

                        foreach (var item in roles)
                        {
                            var permission = _nccPermissionRepository.Get(item);
                            if(permission != null)
                            {
                                user.Permissions.Add(new NccUserPermission() { Permission = permission, User = user });
                            }
                            else
                            {
                                response.Add((false, "Roles not found."));
                            }
                        }

                        var result = _entityRepository.Edit(user);                        
                        response.Add((true,"Roles have been successfully assigned into user " + user.UserName + ". Please refresh page to see update."));
                    }
                    else
                    {
                        response.Add((false,"User not found."));
                    }                    
                }

                _entityRepository.SaveChange();
                txn.Commit();                
            }
            return response;
        }
    }
}

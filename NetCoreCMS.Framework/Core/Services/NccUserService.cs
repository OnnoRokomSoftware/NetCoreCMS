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
using NetCoreCMS.Framework.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NetCoreCMS.Framework.Core.Mvc.Cache;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Framework.Core.Mvc.Service;

namespace NetCoreCMS.Framework.Core.Services
{
    public interface INccUserService
    {
        void DeletePermanently(long entityId);
        NccUser Get(long entityId, bool isAsNoTracking = false, bool withDeleted = false);
        NccUser GetByUserName(string userName);
        NccUser GetNccUser(long entityId);
        List<NccUser> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false, bool withDeleted = false);
        void Remove(long entityId);
        NccUser Save(NccUser entity);
        List<NccUser> Search(string searchKey);
        NccUser Update(NccUser entity);
        List<(bool IsSuccess, string Message)> UpdateUsersPermission(List<long> userIds, long[] roles, string roleOperation);
    }

    public class NccUserService : BaseService<NccUser>, INccUserService
    {
        private readonly NccUserRepository _entityRepository;
        private readonly NccPermissionRepository _nccPermissionRepository;
        private readonly IMemoryCache _cache;
        private readonly List<string> userRelations;

        public NccUserService(NccUserRepository entityRepository, NccPermissionRepository nccPermissionRepository, IMemoryCache memoryCache) : base(entityRepository)
        {
            _entityRepository = entityRepository;
            _nccPermissionRepository = nccPermissionRepository;
            _cache = memoryCache;
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

        #region Method Override
        public override NccUser Get(long entityId, bool isAsNoTracking = false, bool withDeleted = false)
        {
            NccUser user = null;
            if (GlobalContext.WebSite.EnableCache)
            {
                user = GlobalContext.GlobalCache.GetNccUser(entityId);
                if (user == null)
                {
                    user = _entityRepository.Get(entityId, isAsNoTracking, userRelations);
                    GlobalContext.GlobalCache.SetNccUser(user);
                }
            }
            else
            {
                user = _entityRepository.Get(entityId, isAsNoTracking, userRelations);
            }

            return user;
        }
        #endregion

        #region New Method
        public NccUser GetNccUser(long entityId)
        {
            var user = _entityRepository.Get(entityId, false, userRelations);
            return user;
        }

        public NccUser GetByUserName(string userName)
        {
            var query = _entityRepository.Query();
            foreach (var item in userRelations)
            {
                query = query.Include(item);
            }
            return query.FirstOrDefault(x => x.UserName == userName);
        }

        public List<NccUser> Search(string searchKey)
        {
            return _entityRepository.Search(searchKey);
        }

        public List<(bool IsSuccess, string Message)> UpdateUsersPermission(List<long> userIds, long[] roles, string roleOperation)
        {
            var response = new List<(bool, string)>();
            using (var txn = _entityRepository.BeginTransaction())
            {
                foreach (var userId in userIds)
                {
                    var user = GetNccUser(userId);
                    if (user != null)
                    {
                        if (roleOperation.Equals("Add"))
                        {
                            foreach (var item in roles)
                            {
                                if (user.Permissions.Where(x => x.PermissionId == item).Count() == 0)
                                {
                                    var permission = _nccPermissionRepository.Get(item);
                                    if (permission != null)
                                    {
                                        user.Permissions.Add(new NccUserPermission() { Permission = permission, User = user });
                                    }
                                }
                            }
                            var result = _entityRepository.Edit(user);
                            response.Add((true, "Roles have been successfully assigned into user " + user.UserName + ". Please refresh page to see update."));
                        }
                        else if (roleOperation.Equals("Remove"))
                        {
                            foreach (var item in roles)
                            {
                                var permission = user.Permissions.Where(x => x.PermissionId == item).FirstOrDefault();
                                if (permission != null)
                                {
                                    user.Permissions.RemoveAt(user.Permissions.IndexOf(permission));
                                }
                            }

                            var result = _entityRepository.Edit(user);
                            response.Add((true, "Roles have been successfully assigned into user " + user.UserName + ". Please refresh page to see update."));
                        }
                    }
                    else
                    {
                        response.Add((false, "User not found."));
                    }
                }

                _entityRepository.SaveChange();
                txn.Commit();
            }
            return response;
        }
        #endregion
    }
}

/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using NetCoreCMS.Framework.Core.Data;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreCMS.Framework.Core.Mvc.Repository
{
    public interface IBaseRepository<EntityT, IdT>
    {
        IQueryable<EntityT> Query();
        EntityEntry GetEntityEntry(EntityT T);
        EntityT Add(EntityT entity);
        EntityT Edit(EntityT entity);        
        EntityT Get(IdT id, bool isAsNoTracking = false, List<string> includeChilds = null);
        EntityT Get(string name, bool isAsNoTracking = false, List<string> includeRelationalProperties = null);
        List<EntityT> Load(string name, List<string> includeRelationalProperties = null);
        List<EntityT> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false, List<string> includeRelationalProperties = null);        
        IDbContextTransaction BeginTransaction();
        void Remove(EntityT entity);
        void DeletePermanently(EntityT entity);
        void SaveChange();
        int ExecuteSqlCommand(NccDbQueryText query);
    }
}
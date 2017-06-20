/*
*Author: TecRT
*Website: http://tecrt.com
*Copyright (c) tecrt.com
*License: BSD (3 Clause)
*/

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;

namespace NetCoreCMS.Framework.Core.Mvc.Repository
{
    public interface IBaseRepository<EntityT, IdT>
    {
        IQueryable<EntityT> Query();
        EntityEntry GetEntityEntry(EntityT T);
        EntityT Add(EntityT entity);
        EntityT Edit(EntityT entity);
        IDbContextTransaction BeginTransaction();        
        void Remove(EntityT entity);
        void DeletePermanently(EntityT entity);
        void SaveChange();
    }
}

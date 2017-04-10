/*
*Author: Xonaki
*Website: http://xonaki.com
*Copyright (c) xonaki.com
*License: BSD (3 Clause)
*/

using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;

namespace NetCoreCMS.Framework.Core.Mvc.Repository
{
    public interface IBaseRepository<EntityT, IdT>
    {
        IQueryable<EntityT> Query();
        EntityT Add(EntityT entity);
        EntityT Eidt(EntityT entity);
        IDbContextTransaction BeginTransaction();        
        void Remove(EntityT entity);
        void DeletePermanently(EntityT entity);
        void SaveChange();
    }
}

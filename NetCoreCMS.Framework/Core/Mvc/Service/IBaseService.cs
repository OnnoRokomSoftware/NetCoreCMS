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

namespace NetCoreCMS.Framework.Core.Mvc.Services
{
    public interface IBaseService<EntityT> 
    {
        EntityT Get(long entityId, bool isAsNoTracking = false);
        List<EntityT> LoadAll(bool isActive = true, int status = 0, string name = "", bool isLikeSearch = false);
        EntityT Save(EntityT model);
        EntityT Update(EntityT model);
        void Remove(long entityId);
        void DeletePermanently(long entityId);        

    }
}

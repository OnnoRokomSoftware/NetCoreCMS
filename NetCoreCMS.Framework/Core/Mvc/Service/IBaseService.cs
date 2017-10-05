using System.Collections.Generic;
using NetCoreCMS.Framework.Core.Mvc.Models;

namespace NetCoreCMS.Framework.Core.Mvc.Services
{
    public interface IBaseService<EntityT>
    {
        EntityT Get(long entityId, bool isAsNoTracking = false);
        List<EntityT> LoadAll(bool isActive = true, int status = 0, string name = "", bool isLikeSearch = false);
        //List<EntityT> LoadAll();
        //List<EntityT> LoadAllActive();
        //List<EntityT> LoadAllByStatus(int status);
        //List<EntityT> LoadAllByName(string name);
        //List<EntityT> LoadAllByNameContains(string name);
        EntityT Save(EntityT model);
        EntityT Update(EntityT model);
        void Remove(long entityId);
        void DeletePermanently(long entityId);        

    }
}

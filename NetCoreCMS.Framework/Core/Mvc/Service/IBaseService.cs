using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.Mvc.Services
{
    public interface IBaseService<EntityT>
    {
        EntityT Get(long entityId);
        List<EntityT> LoadAll();
        List<EntityT> LoadAllActive();
        List<EntityT> LoadAllByStatus(int status);
        List<EntityT> LoadAllByName(string name);
        List<EntityT> LoadAllByNameContains(string name);
        EntityT Save(EntityT model);
        EntityT Update(EntityT model);
        void Remove(long entityId);
        void DeletePermanently(long entityId);        

    }
}

using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.Services
{
    public interface IBaseService<EntityT>
    {
        EntityT Get(long entityId);
        List<EntityT> GetAll();
        List<EntityT> GetAllByStatus(int status);
        List<EntityT> GetAllByName(string name);
        List<EntityT> GetAllByNameContains(string name);
        EntityT Save(EntityT model);
        EntityT Update(EntityT model);
        void Remove(long entityId);
        void DeletePermanently(long entityId);
        string ToUniqueSlug(string slug, long entityId);

    }
}

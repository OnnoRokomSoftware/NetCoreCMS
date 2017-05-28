using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Framework.Core.Repository;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccWebSiteWidgetService : IBaseService<NccWebSiteWidget>
    {
        private readonly NccWebSiteWidgetRepository _entityRepository;

        public NccWebSiteWidgetService()
        {
        }
        public NccWebSiteWidgetService(NccWebSiteWidgetRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }
         
        public NccWebSiteWidget Get(long entityId)
        {
            return _entityRepository.Query().FirstOrDefault(x => x.Id == entityId);
        }

        public NccWebSiteWidget Save(NccWebSiteWidget entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NccWebSiteWidget Update(NccWebSiteWidget entity)
        {
            var oldEntity = _entityRepository.Query().FirstOrDefault(x => x.Id == entity.Id);
            if(oldEntity != null)
            {
                using (var txn = _entityRepository.BeginTransaction())
                {
                    CopyNewData(oldEntity, entity);
                    _entityRepository.Eidt(oldEntity);
                    _entityRepository.SaveChange();
                    txn.Commit();
                }
            }
            
            return entity;
        }
        
        public void Remove(long entityId)
        {
            var entity = _entityRepository.Query().FirstOrDefault(x => x.Id == entityId );
            if (entity != null)
            {
                entity.Status = EntityStatus.Deleted;
                _entityRepository.Eidt(entity);
                _entityRepository.SaveChange();
            }
        }

        public List<NccWebSiteWidget> LoadAll()
        {
            return _entityRepository.Query().ToList();
        }

        public List<NccWebSiteWidget> LoadAllByStatus(int status)
        {
            return _entityRepository.Query().Where(x => x.Status == status).ToList();
        }

        public List<NccWebSiteWidget> LoadAllByName(string name)
        {
            return _entityRepository.Query().Where(x => x.Name == name).ToList();
        }

        public List<NccWebSiteWidget> LoadAllByNameContains(string name)
        {
            return _entityRepository.Query().Where(x => x.Name.Contains(name)).ToList();
        }

        public void DeletePermanently(long entityId)
        {
            var entity = _entityRepository.Query().FirstOrDefault(x => x.Id == entityId);
            if (entity != null)
            {
                _entityRepository.Remove(entity);
                _entityRepository.SaveChange();
            }
        }

        private void CopyNewData(NccWebSiteWidget oldEntity, NccWebSiteWidget entity)
        {   
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Name = entity.Name; 
            oldEntity.Status = entity.Status;
            oldEntity.CreateBy = entity.CreateBy;
            oldEntity.LayoutName = entity.LayoutName;
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.Name = entity.Name;
            oldEntity.Status = entity.Status;
            oldEntity.WebSite = entity.WebSite;
            oldEntity.WidgetConfigJson = entity.WidgetConfigJson;
            oldEntity.WidgetData = entity.WidgetData;
            oldEntity.WidgetId = entity.WidgetId;
            oldEntity.WidgetOrder = entity.WidgetOrder;
            oldEntity.WidgetSection = oldEntity.WidgetSection;
        }
    }
}

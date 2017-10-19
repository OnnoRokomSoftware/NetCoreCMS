/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System.Linq;
using System.Collections.Generic;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Framework.Core.Repository;
using System;

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

        public NccWebSiteWidget Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId, isAsNoTracking);
        }

        public List<NccWebSiteWidget> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch);
        }

        public NccWebSiteWidget Save(NccWebSiteWidget entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NccWebSiteWidget Update(NccWebSiteWidget entity)
        {
            var oldEntity = _entityRepository.Get(entity.Id);
            if (oldEntity != null)
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

        public void RemoveByModuleThemeLayoutZoneWidget(string module, string theme, string layout, string zone, string widget)
        {
            var entity = _entityRepository.Query().FirstOrDefault(
                x => x.ModuleId == module
                && x.ThemeId == theme
                && x.LayoutName == layout
                && x.Zone == zone
                && x.WidgetId == widget
                );

            if (entity != null)
            {
                _entityRepository.Remove(entity);
                _entityRepository.SaveChange();
            }
        }

        public string RemoveByZoneWidgetId(long zoneWidget)
        {
            var entity = _entityRepository.Get(zoneWidget);
            if(entity == null)
            {
                return "Error: No zone widget found";
            }
            _entityRepository.DeletePermanently(entity);
            _entityRepository.SaveChange();
            return "Success: Removed Successfully";
        }

        public void Remove(long entityId)
        {
            var entity = _entityRepository.Get(entityId);
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
            oldEntity.Zone = oldEntity.Zone;
            oldEntity.Metadata = entity.Metadata;
        }

        public string DownOrder(long zoneWidgetId, int oldOrder)
        {
            var entity = _entityRepository.Get(zoneWidgetId);
            if (entity == null)
            {
                return "Error: No zone widget found";
            }

            var widgetOrder = entity.WidgetOrder;                        
            var upEntityList = GetNext(entity.ModuleId, entity.ThemeId, entity.LayoutName, entity.Zone, widgetOrder);
            entity.WidgetOrder += 1;

            foreach (var item in upEntityList)
            {
                //Skip entity new order number
                if (widgetOrder == entity.WidgetOrder)
                {
                    widgetOrder++;
                }
                item.WidgetOrder = widgetOrder++;
                _entityRepository.Edit(item);
            }
            
            _entityRepository.Edit(entity);
            _entityRepository.SaveChange();
            return "Success: update successful";
        }

        public string UpOrder(long zoneWidgetId, int oldOrder)
        {
            var entity = _entityRepository.Get(zoneWidgetId);
            if (entity == null)
            {
                return "Error: No zone widget found";
            }

            var widgetOrder = entity.WidgetOrder;            
            entity.WidgetOrder -= 1;
            var upEntityList = GetPrevious(entity.ModuleId, entity.ThemeId, entity.LayoutName, entity.Zone, widgetOrder);
            foreach (var item in upEntityList)
            {
                if (item.WidgetOrder == entity.WidgetOrder)
                {
                    item.WidgetOrder += 1;
                    _entityRepository.Edit(item);
                }
                //if(widgetOrder == entity.WidgetOrder)
                //{
                //    widgetOrder--;
                //}
                //item.WidgetOrder = widgetOrder--;
                //_entityRepository.Edit(item);
            }
            
            _entityRepository.Edit(entity);
            _entityRepository.SaveChange();
            return "Success: update successful";
        }

        public NccWebSiteWidget Get(string module, string theme, string layout, string zone, string widget)
        {
            var entity = _entityRepository.Query().OrderBy(x=>x.WidgetOrder).FirstOrDefault(
                x => x.ModuleId == module
                && x.ThemeId == theme
                && x.LayoutName == layout
                && x.Zone == zone
                && x.WidgetId == widget
            );
            return entity;
        }

        public List<NccWebSiteWidget> GetNext(string module, string theme, string layout, string zone, int order)
        {
            var entityList = _entityRepository.Query().OrderBy(x => x.WidgetOrder).Where(
                x => x.ModuleId == module
                && x.ThemeId == theme
                && x.LayoutName == layout
                && x.Zone == zone
                && x.WidgetOrder > order
            ).ToList();
            return entityList;            
        }

        public List<NccWebSiteWidget> GetPrevious(string module, string theme, string layout, string zone, int order)
        {
            var entityList = _entityRepository.Query().OrderByDescending(x => x.WidgetOrder).Where(
                x => x.ModuleId == module
                && x.ThemeId == theme
                && x.LayoutName == layout
                && x.Zone == zone
                && x.WidgetOrder < order
            ).ToList();
            return entityList;            
        }
    }
}

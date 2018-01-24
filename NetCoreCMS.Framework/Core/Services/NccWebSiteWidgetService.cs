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
using NetCoreCMS.Framework.Core.Repository;
using Microsoft.Extensions.Caching.Memory;
using NetCoreCMS.Framework.Core.Mvc.Service;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccWebSiteWidgetService : BaseService<NccWebSiteWidget>
    {
        private readonly NccWebSiteWidgetRepository _entityRepository;
        private readonly IMemoryCache _cache;

        public NccWebSiteWidgetService(NccWebSiteWidgetRepository entityRepository, IMemoryCache memoryCache) : base(entityRepository)
        {
            _entityRepository = entityRepository;
            _cache = memoryCache;
        }

        #region New Method
        public void RemoveByModuleThemeLayoutZoneWidget(string moduleName, string theme, string layout, string zone, string widget)
        {
            var entity = _entityRepository.Query().FirstOrDefault(
                x => x.ModuleName == moduleName
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
            if (entity == null)
            {
                return "Error: No zone widget found";
            }
            _entityRepository.DeletePermanently(entity);
            _entityRepository.SaveChange();
            return "Success: Removed Successfully";
        }

        public string DownOrder(long zoneWidgetId, int oldOrder)
        {
            var entity = _entityRepository.Get(zoneWidgetId);
            if (entity == null)
            {
                return "Error: No zone widget found";
            }

            var widgetOrder = entity.WidgetOrder;

            var upEntityList = Load(entity.ThemeId, entity.LayoutName, entity.Zone);
            foreach (var item in upEntityList)
            {
                if (item.WidgetOrder == widgetOrder + 1)
                {
                    item.WidgetOrder -= 1;
                    _entityRepository.Edit(item);
                }
            }
            entity.WidgetOrder += 1;
            //var upEntityList = LoadNext(entity.ModuleName, entity.ThemeId, entity.LayoutName, entity.Zone, widgetOrder);
            //foreach (var item in upEntityList)
            //{
            //    //Skip entity new order number
            //    if (widgetOrder == entity.WidgetOrder)
            //    {
            //        widgetOrder++;
            //    }
            //    item.WidgetOrder = widgetOrder++;
            //    _entityRepository.Edit(item);
            //}

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
            var upEntityList = Load(entity.ThemeId, entity.LayoutName, entity.Zone);

            foreach (var item in upEntityList)
            {
                if (item.WidgetOrder == widgetOrder - 1)
                {
                    item.WidgetOrder += 1;
                    _entityRepository.Edit(item);
                }
            }
            entity.WidgetOrder -= 1;
            //var upEntityList = LoadPrevious(entity.ModuleName, entity.ThemeId, entity.LayoutName, entity.Zone, widgetOrder);
            //foreach (var item in upEntityList)
            //{
            //    if (item.WidgetOrder == entity.WidgetOrder)
            //    {
            //        item.WidgetOrder += 1;
            //        _entityRepository.Edit(item);
            //    }
            //    //if(widgetOrder == entity.WidgetOrder)
            //    //{
            //    //    widgetOrder--;
            //    //}
            //    //item.WidgetOrder = widgetOrder--;
            //    //_entityRepository.Edit(item);
            //}

            _entityRepository.Edit(entity);
            _entityRepository.SaveChange();
            return "Success: update successful";
        }

        public NccWebSiteWidget Get(string moduleName, string theme, string layout, string zone, string widget)
        {
            var entity = _entityRepository.Query().OrderBy(x => x.WidgetOrder).FirstOrDefault(
                x => x.ModuleName == moduleName
                && x.ThemeId == theme
                && x.LayoutName == layout
                && x.Zone == zone
                && x.WidgetId == widget
            );
            return entity;
        }

        public List<NccWebSiteWidget> Load(string theme, string layout, string zone)
        {
            var entityList = _entityRepository.Query().Where(
                x => x.ThemeId == theme
                && x.LayoutName == layout
                && x.Zone == zone
            ).OrderBy(x => x.WidgetOrder).ToList();
            return entityList;
        }

        public List<NccWebSiteWidget> LoadNext(string module, string theme, string layout, string zone, int order)
        {
            var entityList = _entityRepository.Query().Where(
                x => x.ModuleName == module
                && x.ThemeId == theme
                && x.LayoutName == layout
                && x.Zone == zone
                && x.WidgetOrder > order
            ).OrderBy(x => x.WidgetOrder).ToList();
            return entityList;
        }

        public List<NccWebSiteWidget> LoadPrevious(string moduleName, string theme, string layout, string zone, int order)
        {
            var entityList = _entityRepository.Query().Where(
                x => x.ModuleName == moduleName
                && x.ThemeId == theme
                && x.LayoutName == layout
                && x.Zone == zone
                && x.WidgetOrder < order
            ).OrderByDescending(x => x.WidgetOrder).ToList();
            return entityList;
        } 
        #endregion
    }
}

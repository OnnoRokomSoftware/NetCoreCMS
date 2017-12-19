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
using System.Linq;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Framework.Core.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using NetCoreCMS.Framework.Utility;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccMenuService : IBaseService<NccMenu>
    {
        private readonly NccMenuRepository _entityRepository;
        private readonly NccMenuItemRepository _menuItemRepository;

        public NccMenuService(NccMenuRepository entityRepository, NccMenuItemRepository menuItemRepository)
        {
            _entityRepository = entityRepository;
            _menuItemRepository = menuItemRepository;
        }

        public NccMenu Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId);
        }

        public List<NccMenu> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch);
        }

        public NccMenu Save(NccMenu entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NccMenu Update(NccMenu entity)
        {
            var oldEntity = _entityRepository.Query().Include("MenuItems").FirstOrDefault(x => x.Id == entity.Id);

            if (oldEntity != null)
            {
                 
                RecursiveLoad(oldEntity);

                for(var i = 0; i < oldEntity.MenuItems.Count; i++)
                {
                    RecursiveNccMenuItemDelete(oldEntity.MenuItems[i]);
                }

                _menuItemRepository.SaveChange();  

                oldEntity = _entityRepository.Query().Include("MenuItems").FirstOrDefault(x => x.Id == entity.Id);
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
        
        public void Remove(long entityId)
        {
            var entity = _entityRepository.Get(entityId );
            if (entity != null)
            {
                entity.Status = EntityStatus.Deleted;
                _entityRepository.Edit(entity);
                _entityRepository.SaveChange();
            }
        }

        public void DeletePermanently(long entityId)
        {
            var entity = _entityRepository.Get(entityId, false, new List<string>() { "MenuItems" } );

            if (entity != null)
            {
                RecursiveLoad(entity);                
                for (var i = 0; i < entity.MenuItems.Count; i++)
                {
                    RecursiveNccMenuItemDelete(entity.MenuItems[i]);
                }
                //_menuItemRepository.SaveChange();
                _entityRepository.Remove(entity);
                _entityRepository.SaveChange();
            }
        }

        private void CopyNewData(NccMenu oldEntity, NccMenu entity)
        {                
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = GlobalContext.GetCurrentUserId();
            oldEntity.MenuLanguage = entity.MenuLanguage;
            oldEntity.Name = entity.Name;            
            oldEntity.Status = entity.Status;            
            oldEntity.MenuIconCls = entity.MenuIconCls;
            oldEntity.Position = entity.Position;
            oldEntity.Status = entity.Status;
            oldEntity.MenuItems = entity.MenuItems;
            oldEntity.Metadata = entity.Metadata;
        }
         
        private void RecursiveNccMenuItemDelete(NccMenuItem nccMenuItem)
        {
            for (int i = 0; i <  nccMenuItem.Childrens.Count; i++)
            {
                RecursiveNccMenuItemDelete(nccMenuItem.Childrens[i]);
            }
            _menuItemRepository.DeletePermanently(nccMenuItem);
            _menuItemRepository.SaveChange();
            
        }

        public List<NccMenu> LoadAllSiteMenus()
        {
            var list =  _entityRepository.Query()
                .Include("MenuItems").ToList();
            foreach (var item in list)
            {
                RecursiveChildrenLoad(item.MenuItems);
            }
            return list;
        }

        private void RecursiveLoad(NccMenu parent)
        {
            var ParentFromDatabase = _entityRepository.GetEntityEntry(parent).Collection("MenuItems");
            RecursiveChildrenLoad(parent.MenuItems);
        }

        private void RecursiveChildrenLoad(List<NccMenuItem> menuItems)
        {
            foreach (var child in menuItems)
            {
                _menuItemRepository.GetEntityEntry(child).Collection("Childrens").Load();
                _menuItemRepository.GetEntityEntry(child).Collection("SubActions").Load();
                if (child.Childrens.Count > 0)
                    RecursiveChildrenLoad(child.Childrens);
            }
        }

    }
}

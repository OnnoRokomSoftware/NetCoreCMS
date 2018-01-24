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
using NetCoreCMS.Framework.Core.Repository;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Mvc.Service;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccMenuService : BaseService<NccMenu>
    {
        private readonly NccMenuRepository _entityRepository;
        private readonly NccMenuItemRepository _menuItemRepository;

        public NccMenuService(NccMenuRepository entityRepository, NccMenuItemRepository menuItemRepository) : base(entityRepository, new List<string>() { "MenuItems" })
        {
            _entityRepository = entityRepository;
            _menuItemRepository = menuItemRepository;
        }

        #region Method Override
        public override NccMenu Update(NccMenu entity)
        {
            var oldEntity = _entityRepository.Get(entity.Id);

            if (oldEntity != null)
            {

                RecursiveLoad(oldEntity);

                for (var i = 0; i < oldEntity.MenuItems.Count; i++)
                {
                    RecursiveNccMenuItemDelete(oldEntity.MenuItems[i]);
                }

                _menuItemRepository.SaveChange();

                oldEntity = _entityRepository.Get(entity.Id);
                using (var txn = _entityRepository.BeginTransaction())
                {
                    CopyNewData(entity, oldEntity);
                    _entityRepository.Edit(oldEntity);
                    _entityRepository.SaveChange();
                    txn.Commit();
                }
            }

            return entity;
        }

        public override void AfterCopyData(NccMenu entity, NccMenu oldEntity)
        {
            oldEntity.MenuItems.AddRange(entity.MenuItems);            
        }

        public override void DeletePermanently(long entityId)
        {
            var entity = _entityRepository.Get(entityId);

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
        #endregion

        #region New Methods
        public List<NccMenu> LoadAllSiteMenus()
        {
            var list = _entityRepository.Query()
                .Include("MenuItems").ToList();
            foreach (var item in list)
            {
                RecursiveChildrenLoad(item.MenuItems);
            }
            return list;
        }
        #endregion

        #region Helper

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
        private void RecursiveNccMenuItemDelete(NccMenuItem nccMenuItem)
        {
            for (int i = 0; i < nccMenuItem.Childrens.Count; i++)
            {
                RecursiveNccMenuItemDelete(nccMenuItem.Childrens[i]);
            }
            _menuItemRepository.DeletePermanently(nccMenuItem);
            _menuItemRepository.SaveChange();

        } 
        #endregion

    }
}

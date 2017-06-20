using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Framework.Core.Repository;
using Microsoft.EntityFrameworkCore;
using System;

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

        public NccMenu Get(long entityId)
        {
            return _entityRepository.Query().Include(x => x.MenuItems).FirstOrDefault(x => x.Id == entityId);
        }

        public NccMenu Save(NccMenu entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NccMenu Update(NccMenu entity)
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

        public List<NccMenu> LoadAll()
        {
            return _entityRepository.Query().ToList();
        }

        public List<NccMenu> LoadAllByStatus(int status)
        {
            return _entityRepository.Query().Where(x => x.Status == status).ToList();
        }

        public List<NccMenu> LoadAllByName(string name)
        {
            return _entityRepository.Query().Where(x => x.Name == name).ToList();
        }

        public List<NccMenu> LoadAllByNameContains(string name)
        {
            return _entityRepository.Query().Where(x => x.Name.Contains(name)).ToList();
        }

        public void DeletePermanently(long entityId)
        {
            var entity = _entityRepository.Query().Include(x=>x.MenuItems).FirstOrDefault(x => x.Id == entityId);
            if (entity != null)
            {
                _entityRepository.Remove(entity);
                _entityRepository.SaveChange();
            }
        }

        private void CopyNewData(NccMenu oldEntity, NccMenu entity)
        {                
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = BaseModel.GetCurrentUserId();
            oldEntity.Name = entity.Name;            
            oldEntity.Status = entity.Status;
            oldEntity.MenuFor = entity.MenuFor;
            oldEntity.MenuIconCls = entity.MenuIconCls;
            oldEntity.Position = entity.Position;
            oldEntity.Status = entity.Status;            
        }

        public List<NccMenu> LoadAllSiteMenus()
        {
            var list =  _entityRepository.Query()
                .Include("MenuItems")
                .Where(x => x.MenuFor == NccMenu.NccMenuFor.Site).ToList();
            foreach (var item in list)
            {
                RecursiveChieldLoad(item.MenuItems);
            }
            return list;
        }

        private void RecursiveLoad(NccMenu parent)
        {
            var ParentFromDatabase = _entityRepository.GetEntityEntry(parent).Collection("MenuItems");
            RecursiveChieldLoad(parent.MenuItems);
        }

        private void RecursiveChieldLoad(List<NccMenuItem> menuItems)
        {
            foreach (var child in menuItems)
            {
                _menuItemRepository.GetEntityEntry(child).Collection("Childrens").Load();
                _menuItemRepository.GetEntityEntry(child).Collection("SubActions").Load();
                if (child.Childrens.Count > 0)
                    RecursiveChieldLoad(child.Childrens);
            }
        }

    }
}

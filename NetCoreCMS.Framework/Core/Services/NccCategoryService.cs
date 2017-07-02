using System;
using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Framework.Core.Repository;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccCategoryService : IBaseService<NccCategory>
    {
        private readonly NccCategoryRepository _entityRepository;

        public NccCategoryService(NccCategoryRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }
         
        public NccCategory Get(long entityId)
        {
            return _entityRepository.Get(entityId);
        }

        public NccCategory Save(NccCategory entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NccCategory Update(NccCategory entity)
        {
            var oldEntity = _entityRepository.Get(entity.Id);
            if(oldEntity != null)
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

        public List<NccCategory> LoadAll()
        {
            return _entityRepository.LoadAll();
        }

        public List<NccCategory> LoadAllActive()
        {
            return _entityRepository.LoadAllActive();
        }

        public List<NccCategory> LoadAllByStatus(int status)
        {
            return _entityRepository.LoadAllByStatus(status);
        }

        public List<NccCategory> LoadAllByName(string name)
        {
            return _entityRepository.LoadAllByName(name);
        }

        public List<NccCategory> LoadAllByNameContains(string name)
        {
            return _entityRepository.LoadAllByNameContains(name);
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

        private void CopyNewData(NccCategory oldEntity, NccCategory entity)
        {            
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Name = entity.Name;           
            oldEntity.Status = entity.Status;
            oldEntity.Categories = entity.Categories;
            oldEntity.CategoryImage = entity.CategoryImage;
            oldEntity.MetaDescription = entity.MetaDescription;
            oldEntity.MetaKeyword = entity.MetaKeyword;
            oldEntity.Parent = entity.Parent;
            oldEntity.Slug = entity.Slug;
            oldEntity.Title = entity.Title;            
        }
        
    }
}

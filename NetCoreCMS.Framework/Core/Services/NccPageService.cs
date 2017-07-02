using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Framework.Core.Repository;
using System;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccPageService : IBaseService<NccPage>
    {
        private readonly NccPageRepository _entityRepository;

        public NccPageService(NccPageRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }
         
        public NccPage Get(long entityId)
        {
            return _entityRepository.Get(entityId);
        }

        public NccPage GetBySlugs(string slug)
        {
           return _entityRepository.Query().FirstOrDefault(x => x.Slug == slug.ToLower());
        }

        public List<NccPage> LoadRecentPages(int count)
        {
            var pages = _entityRepository.LoadRecentPages(count);
            return pages;
        }

        public NccPage Save(NccPage entity)
        {
            using (var txn = _entityRepository.BeginTransaction())
            {
                try
                {
                    _entityRepository.Add(entity);
                    _entityRepository.SaveChange();
                    txn.Commit();
                }
                catch (Exception ex)
                {
                    txn.Rollback();
                    throw ex;
                }

                return entity;
            }
        }
         
        public NccPage Update(NccPage entity)
        {
            var oldEntity = _entityRepository.Get(entity.Id);
            if(oldEntity != null)
            {
                oldEntity.ModificationDate = DateTime.Now;
                oldEntity.ModifyBy = BaseModel.GetCurrentUserId();
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

        public List<NccPage> LoadAll()
        {
            return _entityRepository.LoadAll();
        }

        public List<NccPage> LoadAllByStatus(int status)
        {
            return _entityRepository.LoadAllByStatus(status);
        }

        public List<NccPage> LoadAllByPageStatus(NccPage.NccPageStatus status)
        {
            return _entityRepository.Query().Where(x => x.PageStatus == status).ToList();
        }

        public List<NccPage> LoadAllByName(string name)
        {
            return _entityRepository.LoadAllByName(name);
        }

        public List<NccPage> LoadAllByNameContains(string name)
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

        public NccPage CopyNewData(NccPage copyFrom, NccPage copyTo)
        {                
            copyTo.ModificationDate = copyFrom.ModificationDate;
            copyTo.ModifyBy = BaseModel.GetCurrentUserId();
            copyTo.Name = copyFrom.Name;            
            copyTo.Status = copyFrom.Status;
            copyTo.AddToNavigationMenu = copyFrom.AddToNavigationMenu;
            copyTo.Content = copyFrom.Content;
            copyTo.MetaDescription = copyFrom.MetaDescription;
            copyTo.MetaKeyword = copyFrom.MetaKeyword;
            copyTo.ModificationDate = copyFrom.ModificationDate;
            copyTo.ModifyBy = copyFrom.ModifyBy;
            copyTo.PageStatus = copyFrom.PageStatus;
            copyTo.PageType = copyFrom.PageType;
            copyTo.Parent = copyFrom.Parent;
            copyTo.PublishDate = copyFrom.PublishDate;
            copyTo.Slug = copyFrom.Slug;
            copyTo.Title = copyFrom.Title;
            copyTo.Layout = copyFrom.Layout;
            copyTo.VersionNumber = copyFrom.VersionNumber;
            return copyTo;
        }
        
    }
}

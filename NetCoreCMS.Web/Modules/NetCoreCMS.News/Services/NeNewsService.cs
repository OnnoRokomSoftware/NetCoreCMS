using System;
using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Modules.News.Repository;
using NetCoreCMS.Modules.News.Models;
using Microsoft.EntityFrameworkCore;

namespace NetCoreCMS.Modules.News.Services
{
    public class NeNewsService : IBaseService<NeNews>
    {
        private readonly NeNewsRepository _entityRepository;

        public NeNewsService(NeNewsRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public NeNews Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId, isAsNoTracking, new List<string>() { "CategoryList" });
        }

        public NeNews Save(NeNews entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NeNews Update(NeNews entity)
        {
            RemoveCategories(entity);
            var oldEntity = _entityRepository.Get(entity.Id, false, new List<string>() { "CategoryList" });
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

        private void RemoveCategories(NeNews entity)
        {
            var oldEntityCount = _entityRepository.Get(entity.Id, false, new List<string>() { "CategoryList" }).CategoryList.Count();

            for (int i = 0; i < oldEntityCount; i++)
            {
                var tempEntity = _entityRepository.Get(entity.Id, false, new List<string>() { "CategoryList" });
                tempEntity.CategoryList.RemoveAt(0);
                _entityRepository.SaveChange();
            }
        }

        public void Remove(long entityId)
        {
            var entity = _entityRepository.Query().FirstOrDefault(x => x.Id == entityId);
            if (entity != null)
            {
                entity.Status = EntityStatus.Deleted;
                _entityRepository.Edit(entity);
                _entityRepository.SaveChange();
            }
        }

        public List<NeNews> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch, new List<string>() { "CategoryList" });
        }

        public List<NeNews> LoadAllByCategory(string categoryName, int page = 0, int count = 10)
        {
            return _entityRepository.Query().AsNoTracking()
                .Where(x => x.Status >= EntityStatus.Active
                    && x.CategoryList.Any(c => c.NeCategory.Name == categoryName)
                    && (
                        x.HasDateRange == false
                        || (x.PublishDate >= DateTime.Now && x.ExpireDate <= DateTime.Now)
                    )
                )
                .OrderBy(x => x.Order)
                .Skip(count * page)
                .Take(count)
                .ToList();
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

        private void CopyNewData(NeNews oldEntity, NeNews entity)
        {
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Name = entity.Name;
            oldEntity.Status = entity.Status;

            oldEntity.Content = entity.Content;
            oldEntity.Excerpt = entity.Excerpt;
            oldEntity.HasDateRange = entity.HasDateRange;
            oldEntity.PublishDate = entity.PublishDate;
            oldEntity.ExpireDate = entity.ExpireDate;
            oldEntity.Order = entity.Order;

            oldEntity.CategoryList = entity.CategoryList;
        }
    }
}
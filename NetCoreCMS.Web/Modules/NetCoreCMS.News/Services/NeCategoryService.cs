/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Modules.News.Repository;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Modules.News.Models.Entity;

namespace NetCoreCMS.Modules.News.Services
{
    public class NeCategoryService : IBaseService<NeCategory>
    {
        private readonly NeCategoryRepository _entityRepository;

        public NeCategoryService(NeCategoryRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public NeCategory Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId, isAsNoTracking, new List<string>() { "Details" });
        }

        public NeCategory Save(NeCategory entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NeCategory Update(NeCategory entity)
        {
            var oldEntity = _entityRepository.Query().Include("Details").FirstOrDefault(x => x.Id == entity.Id);
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

        public List<NeCategory> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch, new List<string>() { "Details" });
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

        private void CopyNewData(NeCategory oldEntity, NeCategory entity)
        {
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Metadata = entity.Metadata;
            oldEntity.Name = entity.Name;
            oldEntity.Status = entity.Status;

            var currentDateTime = DateTime.Now;
            foreach (var item in entity.Details)
            {
                var isNew = false;
                var temp = oldEntity.Details.Where(x => x.Language == item.Language).FirstOrDefault();
                if (temp == null)
                {
                    isNew = true;
                    temp = new NeCategoryDetails();
                    temp.Language = item.Language;
                }                
                temp.Metadata = item.Metadata;
                temp.Name = item.Name;
                if (isNew)
                {
                    oldEntity.Details.Add(temp);
                }
            }
        }



        public long Count(bool isActive, string keyword)
        {
            return _entityRepository.Count(isActive, keyword);
        }

        public List<NeCategory> Load(int from, int total, bool isActive, string keyword, string orderBy, string orderDir)
        {
            return _entityRepository.Load(from, total, isActive, keyword, orderBy, orderDir);
        }
    }
}
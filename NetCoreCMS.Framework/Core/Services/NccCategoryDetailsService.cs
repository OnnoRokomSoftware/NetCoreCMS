/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Framework.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccCategoryDetailsService : IBaseService<NccCategoryDetails>
    {
        private readonly NccCategoryDetailsRepository _entityRepository;

        public NccCategoryDetailsService(NccCategoryDetailsRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public NccCategoryDetails Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId, isAsNoTracking);
        }

        public NccCategoryDetails Get(string slug, string language)
        {
            return _entityRepository.Get(slug, language);
        }

        public List<NccCategoryDetails> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch);
        }

        public NccCategoryDetails Save(NccCategoryDetails entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NccCategoryDetails Update(NccCategoryDetails entity)
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

        private void CopyNewData(NccCategoryDetails oldEntity, NccCategoryDetails entity)
        {
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Name = entity.Name;
            oldEntity.Status = entity.Status;
            oldEntity.Category = entity.Category;
            oldEntity.Language = entity.Language;
            oldEntity.MetaDescription = entity.MetaDescription;
            oldEntity.MetaKeyword = entity.MetaKeyword;
            oldEntity.Slug = entity.Slug;
            oldEntity.Title = entity.Title;
            oldEntity.VersionNumber = entity.VersionNumber;
            oldEntity.Metadata = entity.Metadata;
        }

        public List<NccCategoryDetails> LoadByParrentId(long parrentId, bool isActive = true)
        {
            return _entityRepository.LoadByParrentId(parrentId, isActive);
        }

        public List<NccCategoryDetails> LoadRecentCategoryDetails(int count, string language = "")
        {
            return _entityRepository.LoadRecentCategoryDetails(count, language);
        }

        public List<NccCategoryDetails> Search(string name, string language = "", int count = 20)
        {
            return _entityRepository.Search(name, language, count);
        }
    }
}
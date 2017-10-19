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
using System;
using Microsoft.EntityFrameworkCore;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccPageDetailsService : IBaseService<NccPageDetails>
    {
        private readonly NccPageDetailsRepository _entityRepository;

        public NccPageDetailsService(NccPageDetailsRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public NccPageDetails Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId);
        }

        public NccPageDetails Get(string slug, string language)
        {
            return _entityRepository.Get(slug, language);
        }

        public List<NccPageDetails> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch);
        }

        public NccPageDetails Save(NccPageDetails entity)
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

        public NccPageDetails Update(NccPageDetails entity)
        {
            var oldEntity = _entityRepository.Get(entity.Id);
            if (oldEntity != null)
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

        public List<NccPageDetails> LoadRecentPageDetails(int limit, string language = "")
        {
            return _entityRepository.LoadRecentPageDetails(limit, language);
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

        public NccPageDetails CopyNewData(NccPageDetails copyFrom, NccPageDetails copyTo)
        {
            copyTo.ModificationDate = copyFrom.ModificationDate;
            copyTo.ModifyBy = BaseModel.GetCurrentUserId();
            copyTo.Name = copyFrom.Name;
            copyTo.Status = copyFrom.Status;
            copyTo.ModificationDate = copyFrom.ModificationDate;
            copyTo.ModifyBy = copyFrom.ModifyBy;
            copyTo.VersionNumber = copyFrom.VersionNumber;
            copyTo.Content = copyFrom.Content;
            copyTo.Language = copyFrom.Language;
            copyTo.MetaDescription = copyFrom.MetaDescription;
            copyTo.MetaKeyword = copyFrom.MetaKeyword;
            copyTo.Page = copyFrom.Page;
            copyTo.Slug = copyFrom.Slug;
            copyTo.Title = copyFrom.Title;
            copyTo.Metadata = copyFrom.Metadata;

            return copyTo;
        }

        public List<NccPageDetails> LoadAllByStatus(NccPage.NccPageStatus status)
        {
            return _entityRepository.LoadAllByStatus(status);
        }

        public List<NccPageDetails> Search(string name, string language = "")
        {
            return _entityRepository.Search(name, language);
        }
    }
}

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
    public class NccPostDetailsService : IBaseService<NccPostDetails>
    {
        private readonly NccPostDetailsRepository _entityRepository;

        public NccPostDetailsService(NccPostDetailsRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public NccPostDetails Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId, isAsNoTracking);
        }

        public List<NccPostDetails> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch);
        }

        public NccPostDetails Save(NccPostDetails entity)
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

        public NccPostDetails Update(NccPostDetails entity)
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

        public void Remove(long entityId)
        {
            var entity = _entityRepository.Get(entityId);
            if (entity != null)
            {
                entity.Post.PostStatus = NccPost.NccPostStatus.UnPublished;
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

        public NccPostDetails CopyNewData(NccPostDetails copyFrom, NccPostDetails copyTo)
        {
            copyTo.ModificationDate = copyFrom.ModificationDate;
            copyTo.ModifyBy = BaseModel.GetCurrentUserId();
            copyTo.Name = copyFrom.Name;
            copyTo.Status = copyFrom.Status;
            copyTo.ModificationDate = copyFrom.ModificationDate;
            copyTo.VersionNumber = copyFrom.VersionNumber;
            copyTo.CreateBy = copyFrom.CreateBy;
            copyTo.CreationDate = copyFrom.CreationDate;
            copyTo.Content = copyFrom.Content;
            copyTo.Language = copyFrom.Language;
            copyTo.MetaDescription = copyFrom.MetaDescription;
            copyTo.MetaKeyword = copyFrom.MetaKeyword;
            copyTo.Post = copyFrom.Post;
            copyTo.Slug = copyFrom.Slug;
            copyTo.Title = copyFrom.Title;
            copyTo.Metadata = copyFrom.Metadata;

            return copyTo;
        }

        public List<NccPostDetails> LoadRecentPages(int count)
        {
            var pages = _entityRepository.LoadRecentPosts(count);
            return pages;
        }

        public List<NccPostDetails> LoadAllByPostStatusAndDate(NccPost.NccPostStatus status, DateTime dateTime)
        {
            return _entityRepository
                .Query()
                .Include("Post")
                .Include("Post.PostDetails")
                .Where(x => x.Post.PostStatus == status && x.Post.PublishDate <= dateTime).ToList();
        }

        public List<NccPostDetails> LoadRecentPostDetails(int count, string language = "")
        {
            return _entityRepository.LoadRecentPostDetails(count, language);
        }

        public NccPostDetails Get(string slug, string language)
        {
            return _entityRepository.Get(slug, language);
        }

        public int TotalPublishedPostCount()
        {
            return _entityRepository
                .Query()
                .Include("Post")
                .Where(x => x.Post.PostStatus == NccPost.NccPostStatus.Published && x.Post.PublishDate <= DateTime.Now)
                .Count();
        }

        public List<NccPostDetails> Search(string name, string language = "")
        {
            return _entityRepository.Search(name, language);
        }
    }
}
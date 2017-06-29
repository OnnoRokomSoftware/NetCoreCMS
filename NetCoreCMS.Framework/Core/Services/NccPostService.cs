using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Framework.Core.Repository;
using System;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccPostService : IBaseService<NccPost>
    {
        private readonly NccPostRepository _entityRepository;

        public NccPostService(NccPostRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }
         
        public NccPost Get(long entityId)
        {
            return _entityRepository.Query().FirstOrDefault(x => x.Id == entityId);
        }

        public NccPost GetBySlugs(string slug)
        {
           return _entityRepository.Query().FirstOrDefault(x => x.Slug == slug.ToLower());
        }

        public List<NccPost> LoadRecentPages(int count)
        {
            var pages = _entityRepository.LoadRecentPosts(count);
            return pages;
        }

        public NccPost Save(NccPost entity)
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
         
        public NccPost Update(NccPost entity)
        {
            var oldEntity = _entityRepository.Query().FirstOrDefault(x => x.Id == entity.Id);
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
            var entity = _entityRepository.Query().FirstOrDefault(x => x.Id == entityId );
            if (entity != null)
            {
                entity.Status = EntityStatus.Deleted;
                _entityRepository.Edit(entity);
                _entityRepository.SaveChange();
            }
        }

        public List<NccPost> LoadAll()
        {
            return _entityRepository.Query().ToList();
        }

        public List<NccPost> LoadAllByStatus(int status)
        {
            return _entityRepository.Query().Where(x => x.Status == status).ToList();
        }

        public List<NccPost> LoadAllByPageStatus(NccPost.NccPostStatus status)
        {
            return _entityRepository.Query().Where(x => x.PostStatus == status).ToList();
        }

        public List<NccPost> LoadAllByName(string name)
        {
            return _entityRepository.Query().Where(x => x.Name == name).ToList();
        }

        public List<NccPost> LoadAllByNameContains(string name)
        {
            return _entityRepository.Query().Where(x => x.Name.Contains(name)).ToList();
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

        public NccPost CopyNewData(NccPost copyFrom, NccPost copyTo)
        {                
            copyTo.ModificationDate = copyFrom.ModificationDate;
            copyTo.ModifyBy = BaseModel.GetCurrentUserId();
            copyTo.Name = copyFrom.Name;            
            copyTo.Status = copyFrom.Status;
            
            copyTo.Content = copyFrom.Content;
            copyTo.MetaDescription = copyFrom.MetaDescription;
            copyTo.MetaKeyword = copyFrom.MetaKeyword;
            copyTo.ModificationDate = copyFrom.ModificationDate;
            //copyTo.ModifyBy = copyFrom.ModifyBy;
            copyTo.PostStatus = copyFrom.PostStatus;
            copyTo.PostType = copyFrom.PostType;
            copyTo.Parent = copyFrom.Parent;
            copyTo.PublishDate = copyFrom.PublishDate;
            copyTo.Slug = copyFrom.Slug;
            copyTo.Title = copyFrom.Title;
            copyTo.Layout = copyFrom.Layout;
            copyTo.VersionNumber = copyFrom.VersionNumber;
            copyTo.AllowComment = copyFrom.AllowComment;
            copyTo.Author = copyFrom.Author;
            copyTo.Categories = copyFrom.Categories;
            copyTo.CreateBy = copyFrom.CreateBy;
            copyTo.CreationDate = copyFrom.CreationDate;
            copyTo.IsFeatured = copyFrom.IsFeatured;
            copyTo.IsStiky = copyFrom.IsStiky;
            copyTo.Comments = copyFrom.Comments;
            copyTo.RelatedPosts = copyFrom.RelatedPosts;
            copyTo.Tags = copyFrom.Tags;
            copyTo.ThumImage = copyFrom.ThumImage;
            
            return copyTo;
        }
        
    }
}

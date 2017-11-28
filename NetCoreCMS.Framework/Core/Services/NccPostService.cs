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
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Framework.Core.Models.ViewModels;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccPostService : IBaseService<NccPost>
    {
        private readonly NccPostRepository _entityRepository;
        private readonly NccPostDetailsRepository _nccPostDetailsRepository;

        public NccPostService(NccPostRepository entityRepository, NccPostDetailsRepository nccPostDetailsRepository)
        {
            _entityRepository = entityRepository;
            _nccPostDetailsRepository = nccPostDetailsRepository;
        }

        public NccPost Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId, isAsNoTracking, new List<string> { "Parent", "PostDetails", "Categories", "Tags", "Tags.Tag" });
        }

        public NccPost GetBySlug(string slug)
        {
            return _nccPostDetailsRepository.Get(slug)?.Post;
        }

        public List<NccPost> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch, new List<string> { "Parent", "PostDetails", "Categories", "Tags", "Tags.Tag" });
        }

        public List<NccPost> LoadSpecialPosts(bool isSticky, bool isFeatured)
        {
            return _entityRepository.LoadPosts(isSticky, isFeatured);
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
            RemoveChieldsForUpdate(entity);
            var oldEntity = _entityRepository.Get(entity.Id, false, new List<string> { "Parent", "PostDetails", "Categories", "Tags" });
            if (oldEntity != null)
            {
                oldEntity.ModificationDate = DateTime.Now;
                oldEntity.ModifyBy = GlobalContext.GetCurrentUserId();
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

        private void RemoveChieldsForUpdate(NccPost entity)
        {
            //remove categories
            var temp = _entityRepository.Get(entity.Id, false, new List<string>() { "Categories", "Tags" });
            var count = temp.Categories.Count();
            for (int i = 0; i < count; i++)
            {
                var tempEntity = _entityRepository.Get(entity.Id, false, new List<string>() { "Categories" });
                tempEntity.Categories.RemoveAt(0);
                _entityRepository.SaveChange();
            }

            count = temp.Tags.Count();
            for (int i = 0; i < count; i++)
            {
                var tempEntity = _entityRepository.Get(entity.Id, false, new List<string>() { "Tags" });
                tempEntity.Tags.RemoveAt(0);
                _entityRepository.SaveChange();
            }
        }

        public void Remove(long entityId)
        {
            var entity = _entityRepository.Get(entityId);
            if (entity != null)
            {
                entity.PostStatus = NccPost.NccPostStatus.UnPublished;
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

        public NccPost CopyNewData(NccPost copyFrom, NccPost copyTo)
        {
            copyTo.ModificationDate = copyFrom.ModificationDate;
            copyTo.ModifyBy = GlobalContext.GetCurrentUserId();
            copyTo.Name = copyFrom.Name;
            copyTo.Status = copyFrom.Status;
            copyTo.ModificationDate = copyFrom.ModificationDate;
            copyTo.PostStatus = copyFrom.PostStatus;
            copyTo.PostType = copyFrom.PostType;
            copyTo.Parent = copyFrom.Parent;
            copyTo.PublishDate = copyFrom.PublishDate;
            copyTo.Layout = copyFrom.Layout;
            copyTo.VersionNumber = copyFrom.VersionNumber;
            copyTo.AllowComment = copyFrom.AllowComment;
            copyTo.Author = copyFrom.Author;
            copyTo.CreateBy = copyFrom.CreateBy;
            copyTo.CreationDate = copyFrom.CreationDate;
            copyTo.IsFeatured = copyFrom.IsFeatured;
            copyTo.IsStiky = copyFrom.IsStiky;
            //copyTo.Comments = copyFrom.Comments;
            copyTo.RelatedPosts = copyFrom.RelatedPosts;
            copyTo.Tags = copyFrom.Tags;
            copyTo.ThumImage = copyFrom.ThumImage;
            copyTo.Metadata = copyFrom.Metadata;

            copyTo.Categories = copyFrom.Categories;

            var currentDateTime = DateTime.Now;
            foreach (var item in copyFrom.PostDetails)
            {
                var isNew = false;
                NccPostDetails temp = copyTo.PostDetails.Where(x => x.Language == item.Language).FirstOrDefault();
                if (temp == null)
                {
                    isNew = true;
                    temp = new NccPostDetails();
                    temp.CreationDate = currentDateTime;
                    temp.CreateBy = GlobalContext.GetCurrentUserId();
                    temp.Language = item.Language;
                }
                temp.ModificationDate = currentDateTime;
                temp.ModifyBy = GlobalContext.GetCurrentUserId();

                temp.Title = item.Title;
                temp.Slug = item.Slug;
                temp.Content = item.Content;
                temp.MetaDescription = item.MetaDescription;
                temp.MetaKeyword = item.MetaKeyword;
                temp.Metadata = item.Metadata;
                temp.Name = string.IsNullOrEmpty(item.Name) ? item.Slug : item.Name;
                if (isNew)
                {
                    copyTo.PostDetails.Add(temp);
                }
            }

            return copyTo;
        }

        public List<NccPost> LoadRecentPages(int count)
        {
            var pages = _entityRepository.LoadRecentPosts(count);
            return pages;
        }

        public List<ArchiveItemViewModel> LoadAtchive(bool decendingOrder = true)
        {
            return _entityRepository.LoadArchive(decendingOrder);
        }

        /// <summary>
        /// Use this function to count post
        /// </summary>
        /// <param name="isActive">Load active records</param>
        /// <param name="isPublished">Load Published Records</param>
        /// <param name="withSticky">True to count with sticky and False to count without sticky post</param>
        /// <param name="withFeatured">True to count with featured and False to count without featured post</param>
        /// <param name="dateFrom">Pass value if you want to search between dates</param>
        /// <param name="dateTo">Pass value if you want to search between dates</param>
        /// <param name="categoryId">To count by category</param>
        /// <param name="tagId">To count by tag</param>
        /// <param name="createBy">To load by user</param>
        /// <param name="keyword">To load by keyword(search in title)</param>
        /// <returns></returns>
        public long Count(bool isActive, bool isPublished, bool withSticky, bool withFeatured, DateTime? dateFrom = null, DateTime? dateTo = null, long categoryId = 0, long tagId = 0, long createBy = 0, string keyword = "")
        {
            return _entityRepository.Count(isActive, isPublished, withSticky, withFeatured, dateFrom, dateTo, categoryId, tagId, createBy, keyword);
        }

        /// <summary>
        /// Use this function to lead post
        /// </summary>
        /// <param name="from">Starting index Default 0</param>
        /// <param name="total">Total record you want</param>
        /// <param name="isActive">Load active records</param>
        /// <param name="isPublished">Load Published Records</param>
        /// <param name="withSticky">True to load with sticky and False to load without sticky post</param>
        /// <param name="withFeatured">True to load with featured and False to load without featured post</param>
        /// <param name="dateFrom">Pass value if you want to search between dates</param>
        /// <param name="dateTo">Pass value if you want to search between dates</param>
        /// <param name="categoryId">To load by category</param>
        /// <param name="tagId">To load by tag</param>
        /// <param name="createBy">To load by user</param>
        /// <param name="keyword">To load by keyword(search in title)</param>
        /// <param name="orderBy">Order by column name</param>
        /// <param name="orderDir">Order direction (asc / desc)</param>
        /// <returns></returns>
        public List<NccPost> Load(int from, int total, bool isActive, bool isPublished, bool withSticky = true, bool withFeatured = true, DateTime? dateFrom = null, DateTime? dateTo = null, long categoryId = 0, long tagId = 0, long createBy = 0, string keyword = "", string orderBy = "", string orderDir = "")
        {
            return _entityRepository.Load(from, total, isActive, isPublished, withSticky, withFeatured, dateFrom, dateTo, categoryId, tagId, createBy, keyword, orderBy, orderDir);
        }
    }
}
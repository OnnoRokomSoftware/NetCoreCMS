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

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccCommentsService : IBaseService<NccComment>
    {
        private readonly NccCommentsRepository _entityRepository;

        public NccCommentsService(NccCommentsRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public NccComment Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId, isAsNoTracking, new List<string> { "Post", "Author" });
        }

        public List<NccComment> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch, new List<string> { "Post", "Author" });
        }

        public List<NccComment> LoadApproved(long postId, int page, int count = 10)
        {
            if (postId > 0)
                return _entityRepository.LoadApproved(postId, page, count);
            else
                return _entityRepository.LoadApproved(page, count);
        }

        public NccComment Save(NccComment entity)
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

        public NccComment Update(NccComment entity)
        {
            var oldEntity = _entityRepository.Get(entity.Id, false, new List<string> { "Post", "Author" });
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

        public NccComment CopyNewData(NccComment copyFrom, NccComment copyTo)
        {
            copyTo.ModificationDate = copyFrom.ModificationDate;
            copyTo.ModifyBy = GlobalContext.GetCurrentUserId();
            copyTo.VersionNumber = copyFrom.VersionNumber;
            copyTo.Status = copyFrom.Status;
            copyTo.CreateBy = copyFrom.CreateBy;
            copyTo.CreationDate = copyFrom.CreationDate;
            copyTo.ModificationDate = copyFrom.ModificationDate;
            copyTo.Name = copyFrom.Name;
            copyTo.Metadata = copyFrom.Metadata;

            copyTo.Title = copyFrom.Title;
            copyTo.Content = copyFrom.Content;
            copyTo.Email = copyFrom.Email;
            copyTo.WebSite = copyFrom.WebSite;
            copyTo.CommentStatus = copyFrom.CommentStatus;
            copyTo.Post = copyFrom.Post;
            copyTo.Author = copyFrom.Author;
            copyTo.AuthorName = copyFrom.AuthorName;

            return copyTo;
        }

        public List<NccComment> Load(long postId, int count = 5)
        {
            return _entityRepository.Load(postId, count);
        }

        public List<NccComment> LoadRecentComments(int count)
        {
            return _entityRepository.LoadRecentComments(count);
        }

        /// <summary>
        /// Use this function to count post
        /// </summary>
        /// <param name="isActive">Load active records</param>
        /// <param name="createBy">To load by user</param>
        /// <param name="keyword">To load by keyword(search in title)</param>
        /// <returns></returns>
        public long Count(bool isActive, long createBy = 0, string keyword = "")
        {
            return _entityRepository.Count(isActive, createBy, keyword);
        }

        /// <summary>
        /// Use this function to lead post
        /// </summary>
        /// <param name="from">Starting index Default 0</param>
        /// <param name="total">Total record you want</param>
        /// <param name="isActive">Load active records</param>
        /// <param name="createBy">To load by user</param>
        /// <param name="keyword">To load by keyword(search in title)</param>
        /// <param name="orderBy">Order by column name</param>
        /// <param name="orderDir">Order direction (asc / desc)</param>
        /// <returns></returns>
        public List<NccComment> Load(int from, int total, bool isActive, long createBy = 0, string keyword = "", string orderBy = "", string orderDir = "")
        {
            return _entityRepository.Load(from, total, isActive, createBy, keyword, orderBy, orderDir);
        }
    }
}
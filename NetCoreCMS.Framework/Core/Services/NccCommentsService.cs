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
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Repository;
using NetCoreCMS.Framework.Core.Mvc.Service;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccCommentsService : BaseService<NccComment>
    {
        private readonly NccCommentsRepository _entityRepository;

        public NccCommentsService(NccCommentsRepository entityRepository) : base(entityRepository, new List<string> { "Post", "Author" })
        {
            _entityRepository = entityRepository;
        }

        #region New Method
        public List<NccComment> LoadApproved(long postId, int page, int count = 10)
        {
            if (postId > 0)
                return _entityRepository.LoadApproved(postId, page, count);
            else
                return _entityRepository.LoadApproved(page, count);
        }

        public List<NccComment> LoadRecentComments(int count)
        {
            return _entityRepository.LoadRecentComments(count);
        }

        public List<NccComment> Load(long postId, int count = 5)
        {
            return _entityRepository.Load(postId, count);
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
        #endregion
    }
}
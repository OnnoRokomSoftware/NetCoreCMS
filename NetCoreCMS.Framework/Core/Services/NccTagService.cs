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
using NetCoreCMS.Framework.Core.Repository;
using NetCoreCMS.Framework.Core.Models.ViewModels;
using NetCoreCMS.Framework.Core.Mvc.Service;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccTagService : BaseService<NccTag>
    {
        private readonly NccTagRepository _entityRepository;

        public NccTagService(NccTagRepository entityRepository) : base(entityRepository, new List<string>() { "Posts" })
        {
            _entityRepository = entityRepository;
        }

        #region New Method
        public NccTag Get(string name)
        {
            return _entityRepository.Get(name);
        }

        public NccTag GetWithPost(string name)
        {
            long entityId = 0;
            var tag = _entityRepository.LoadAll(true, -1, name).FirstOrDefault();
            if (tag != null)
                entityId = tag.Id;

            return _entityRepository.Get(entityId, false, new List<string>() { "Posts", "Posts.Post", "Posts.Post.Author", "Posts.Post.PostDetails", "Posts.Post.Comments" });
        }

        public List<NccTag> LoadRecentTag(int count)
        {
            return _entityRepository.LoadRecentTag(count);
        }

        public List<NccTag> Search(string name, int count = 20)
        {
            return _entityRepository.Search(name, count);
        }

        public List<TagCloudItemViewModel> LoadTagCloud()
        {
            return _entityRepository.LoadTagCloud();
        }

        /// <summary>
        /// Use this function to count tags
        /// </summary>
        /// <param name="isActive">Load active records</param>
        /// <param name="keyword">To load by keyword(search in title)</param>
        /// <returns></returns>
        public long Count(bool isActive, string keyword = "")
        {
            return _entityRepository.Count(isActive, keyword);
        }

        /// <summary>
        /// Use this function to lead tags
        /// </summary>
        /// <param name="from">Starting index Default 0</param>
        /// <param name="total">Total record you want</param>
        /// <param name="isActive">Load active records</param>
        /// <param name="keyword">To load by keyword(search in title)</param>
        /// <param name="orderBy">Order by column name</param>
        /// <param name="orderDir">Order direction (asc / desc)</param>
        /// <returns></returns>
        public List<NccTag> Load(int from, int total, bool isActive, string keyword = "", string orderBy = "", string orderDir = "")
        {
            return _entityRepository.Load(from, total, isActive, keyword, orderBy, orderDir);
        } 
        #endregion
    }
}
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
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Service;
using NetCoreCMS.Framework.Core.Repository;
using NetCoreCMS.Framework.Utility;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccCategoryService : BaseService<NccCategory>
    {
        private readonly NccCategoryRepository _entityRepository;

        public NccCategoryService(NccCategoryRepository entityRepository):base(entityRepository,new List<string>() { "CategoryDetails", "Parent" })
        {
            _entityRepository = entityRepository;
        }

        #region Method Override
        public override void CopyNewData(NccCategory copyFrom, NccCategory copyTo)
        {
            copyTo.ModificationDate = copyFrom.ModificationDate;
            copyTo.ModifyBy = copyFrom.ModifyBy;
            copyTo.Name = copyFrom.Name;
            copyTo.Status = copyFrom.Status;
            copyTo.CategoryImage = copyFrom.CategoryImage;
            copyTo.Parent = copyFrom.Parent;
            copyTo.VersionNumber = copyFrom.VersionNumber;
            copyTo.Metadata = copyFrom.Metadata;

            var creationDate = DateTime.Now;
            copyTo.ModificationDate = creationDate;

            foreach (var item in copyFrom.CategoryDetails)
            {
                var tmpCategoryDetails = copyTo.CategoryDetails.Where(x => x.Language == item.Language).FirstOrDefault();
                if (tmpCategoryDetails == null)
                {
                    tmpCategoryDetails = new NccCategoryDetails();
                    copyTo.CategoryDetails.Add(tmpCategoryDetails);
                }

                tmpCategoryDetails.Language = item.Language;
                tmpCategoryDetails.MetaDescription = item.MetaDescription;
                tmpCategoryDetails.MetaKeyword = item.MetaKeyword;
                tmpCategoryDetails.ModificationDate = creationDate;
                tmpCategoryDetails.ModifyBy = GlobalContext.GetCurrentUserId();
                tmpCategoryDetails.Name = item.Name;
                tmpCategoryDetails.Slug = item.Slug;
                tmpCategoryDetails.Status = item.Status;
                tmpCategoryDetails.Title = item.Title;
                tmpCategoryDetails.VersionNumber = item.VersionNumber;
                tmpCategoryDetails.Metadata = item.Metadata;
            }
        }
        #endregion

        #region New Method
        public NccCategory GetBySlug(string slug)
        {
            return _entityRepository.GetBySlug(slug);
        }

        public NccCategory GetWithPost(string slug)
        {
            return _entityRepository.GetWithPost(slug);
        }

        public List<NccCategory> LoadAllWithPost(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch, new List<string>() { "CategoryDetails", "Parent", "Posts" });
        }

        public List<NccCategory> LoadByParrentId(long parrentId, bool isActive = true)
        {
            return _entityRepository.LoadByParrentId(parrentId, isActive);
        }

        /// <summary>
        /// Use this function to count post
        /// </summary>
        /// <param name="isActive">Load active records</param>
        /// <param name="keyword">To load by keyword(search in title)</param>
        /// <returns></returns>
        public long Count(bool isActive, string keyword = "")
        {
            return _entityRepository.Count(isActive, keyword);
        }

        /// <summary>
        /// Use this function to lead post
        /// </summary>
        /// <param name="from">Starting index Default 0</param>
        /// <param name="total">Total record you want</param>
        /// <param name="isActive">Load active records</param>
        /// <param name="keyword">To load by keyword(search in title)</param>
        /// <param name="orderBy">Order by column name</param>
        /// <param name="orderDir">Order direction (asc / desc)</param>
        /// <returns></returns>
        public List<NccCategory> Load(int from, int total, bool isActive, string keyword = "", string orderBy = "", string orderDir = "")
        {
            return _entityRepository.Load(from, total, isActive, keyword, orderBy, orderDir);
        } 
        #endregion
    }
}
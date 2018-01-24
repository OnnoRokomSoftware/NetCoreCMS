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
using NetCoreCMS.EasyNews.Repositories;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.EasyNews.Models.Entities;
using NetCoreCMS.Framework.Core.Mvc.Service;

namespace NetCoreCMS.EasyNews.Services
{
    public class CategoryService : BaseService<Category>
    {
        private readonly CategoryRepository _entityRepository;

        public CategoryService(CategoryRepository entityRepository) : base(entityRepository, new List<string>() { "Details" })
        {
            _entityRepository = entityRepository;
        }

        #region Method Override
        public override void AfterCopyData(Category entity, Category oldEntity)
        {
            var currentDateTime = DateTime.Now;
            foreach (var item in entity.Details)
            {
                var isNew = false;
                var temp = oldEntity.Details.Where(x => x.Language == item.Language).FirstOrDefault();
                if (temp == null)
                {
                    isNew = true;
                    temp = new CategoryDetails();
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
        #endregion

        #region New Method
        public long Count(bool isActive, string keyword)
        {
            return _entityRepository.Count(isActive, keyword);
        }

        public List<Category> Load(int from, int total, bool isActive, string keyword, string orderBy, string orderDir)
        {
            return _entityRepository.Load(from, total, isActive, keyword, orderBy, orderDir);
        } 
        #endregion
    }
}
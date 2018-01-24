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
using NetCoreCMS.EasyNews.Repositories;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.EasyNews.Models.Entities;
using NetCoreCMS.Framework.Core.Mvc.Service;

namespace NetCoreCMS.EasyNews.Services
{
    public class NewsService : BaseService<News>
    {
        private readonly NewsRepository _entityRepository;

        public NewsService(NewsRepository entityRepository) : base(entityRepository, new List<string>() { "CategoryList", "Details" })
        {
            _entityRepository = entityRepository;
        }

        #region Method Override
        public override bool DoBeforeUpdate(News entity)
        {
            RemoveCategories(entity);
            return true;
        }

        public override void AfterCopyData(News entity, News oldEntity)
        {
            var currentDateTime = DateTime.Now;
            foreach (var item in entity.Details)
            {
                var isNew = false;
                var temp = oldEntity.Details.Where(x => x.Language == item.Language).FirstOrDefault();
                if (temp == null)
                {
                    isNew = true;
                    temp = new NewsDetails();
                    temp.Language = item.Language;
                }
                temp.Metadata = item.Metadata;
                temp.Name = item.Name;
                temp.Content = item.Content;
                temp.Excerpt = item.Excerpt;
                if (isNew)
                {
                    oldEntity.Details.Add(temp);
                }
            }
        } 
        #endregion

        #region New Method
        public List<News> LoadAllByCategory(string categoryName, int page = 0, int count = 10)
        {
            return _entityRepository.Query().Include("Details").Include("CategoryList").AsNoTracking()
                .Where(x => x.Status >= EntityStatus.Active
                    && x.CategoryList.Any(c => c.Category.Name == categoryName)
                    && (
                        x.HasDateRange == false
                        || (x.PublishDate >= DateTime.Now && x.ExpireDate <= DateTime.Now)
                    )
                )
                .OrderBy(x => x.Order)
                .Skip(count * page)
                .Take(count)
                .ToList();
        }

        public long Count(bool isActive, string keyword)
        {
            return _entityRepository.Count(isActive, keyword);
        }

        public List<News> Load(int from, int total, bool isActive, string keyword, string orderBy, string orderDir)
        {
            return _entityRepository.Load(from, total, isActive, keyword, orderBy, orderDir);
        } 
        #endregion

        #region Helper
        private void RemoveCategories(News entity)
        {
            var oldEntityCount = _entityRepository.Get(entity.Id, false, new List<string>() { "CategoryList" }).CategoryList.Count();

            for (int i = 0; i < oldEntityCount; i++)
            {
                var tempEntity = _entityRepository.Get(entity.Id, false, new List<string>() { "CategoryList" });
                tempEntity.CategoryList.RemoveAt(0);
                _entityRepository.SaveChange();
            }
        } 
        #endregion
    }
}
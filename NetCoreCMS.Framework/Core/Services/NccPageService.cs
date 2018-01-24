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
using System;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Framework.Core.Models.ViewModels;
using NetCoreCMS.Framework.Core.Mvc.Service;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccPageService : BaseService<NccPage>
    {
        private readonly NccPageRepository _entityRepository;
        private readonly NccPageDetailsRepository _nccPageDetailsRepository;

        public NccPageService(NccPageRepository entityRepository, NccPageDetailsRepository nccPageDetailsRepository) : base(entityRepository, new List<string> { "Parent", "PageDetails" })
        {
            _entityRepository = entityRepository;
            _nccPageDetailsRepository = nccPageDetailsRepository;
        }

        #region Method Override
        public override void CopyNewData(NccPage copyFrom, NccPage copyTo)
        {
            copyTo.ModificationDate = copyFrom.ModificationDate;
            copyTo.ModifyBy = GlobalContext.GetCurrentUserId();
            copyTo.Name = copyFrom.Name;
            copyTo.Status = copyFrom.Status;
            copyTo.ModificationDate = copyFrom.ModificationDate;
            copyTo.ModifyBy = copyFrom.ModifyBy;
            copyTo.PageStatus = copyFrom.PageStatus;
            copyTo.PageType = copyFrom.PageType;
            copyTo.Parent = copyFrom.Parent;
            copyTo.PublishDate = copyFrom.PublishDate;
            copyTo.Layout = copyFrom.Layout;
            copyTo.VersionNumber = copyFrom.VersionNumber;
            copyTo.Metadata = copyFrom.Metadata;

            var currentDateTime = DateTime.Now;
            foreach (var item in copyFrom.PageDetails)
            {
                var isNew = false;
                NccPageDetails temp = copyTo.PageDetails.Where(x => x.Language == item.Language).FirstOrDefault();
                if (temp == null)
                {
                    isNew = true;
                    temp = new NccPageDetails();
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
                temp.Name = string.IsNullOrEmpty(item.Name) ? item.Slug : item.Name;
                temp.Metadata = item.Metadata;

                if (isNew)
                {
                    copyTo.PageDetails.Add(temp);
                }
            }

            //copyTo.PageDetails = copyFrom.PageDetails;
            //return copyTo;
        }
        #endregion

        #region New Method
        public NccPage GetBySlug(string slug)
        {
            return _nccPageDetailsRepository.Get(slug)?.Page;
        }

        public List<NccPage> LoadRecentPages(int count)
        {
            var pages = _entityRepository.LoadRecentPages(count);
            return pages;
        }
        public List<NccPage> LoadAllByPageStatus(NccPage.NccPageStatus status)
        {
            return _entityRepository.Query().Where(x => x.PageStatus == status).ToList();
        }

        public long Count(bool isActive, string keyword)
        {
            return _entityRepository.Count(isActive, keyword);
        }

        public List<NccPage> Load(int from, int total, bool isActive, string keyword, string orderBy, string orderDir)
        {
            return _entityRepository.Load(from, total, isActive, keyword, orderBy, orderDir);
        }

        public long SearchCount(string keyword)
        {
            return _entityRepository.SearchCount(keyword);
        }

        public List<NccSearchViewModel> SearchLoad(int from, int total, string keyword)
        {
            return _entityRepository.SearchLoad(from, total, keyword);
        } 
        #endregion
    }
}

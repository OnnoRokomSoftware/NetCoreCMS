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
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Framework.Core.Repository;
using System.Linq;
using System;
using NetCoreCMS.Framework.Utility;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccWebSiteService : IBaseService<NccWebSite>
    {
        private readonly NccWebSiteRepository _entityRepository;
        private readonly NccWebSiteInfoRepository _webSiteInfoRepository;

        public NccWebSiteService()
        {
        }
        public NccWebSiteService(NccWebSiteRepository entityRepository, NccWebSiteInfoRepository nccWebSiteInfoRepository)
        {
            _entityRepository = entityRepository;
            _webSiteInfoRepository = nccWebSiteInfoRepository;
        }

        public NccWebSite Get(long entityId, bool isAsNoTracking = false, bool withDeleted = false)
        {
            return _entityRepository.Get(entityId);
        }

        public List<NccWebSite> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false, bool withDeleted = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch, new List<string>() { "WebSiteInfos" });
        }

        public NccWebSite Save(NccWebSite entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NccWebSite Update(NccWebSite entity)
        {
            var oldEntity = _entityRepository.Get(entity.Id, false, new List<string>() { "WebSiteInfos" });
            if (oldEntity != null)
            {
                using (var txn = _entityRepository.BeginTransaction())
                {
                    CopyNewData(oldEntity, entity);
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

        private void CopyNewData(NccWebSite oldEntity, NccWebSite entity)
        {
            var modificationDate = DateTime.Now;

            oldEntity.AllowRegistration = entity.AllowRegistration;
            oldEntity.DateFormat = entity.DateFormat;
            oldEntity.DomainName = entity.DomainName;
            oldEntity.EmailAddress = entity.EmailAddress;
            oldEntity.Language = entity.Language;
            oldEntity.ModificationDate = modificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Name = entity.Name;
            oldEntity.NewUserRole = entity.NewUserRole;
            oldEntity.Status = entity.Status;
            oldEntity.TimeFormat = entity.TimeFormat;
            oldEntity.TimeZone = entity.TimeZone;
            oldEntity.IsMultiLangual = entity.IsMultiLangual;
            oldEntity.VersionNumber += 1;
            oldEntity.Metadata = entity.Metadata;
            oldEntity.EnableCache = entity.EnableCache;

            foreach (var item in entity.WebSiteInfos)
            {
                var isNew = false;
                var oldWsInfo = oldEntity.WebSiteInfos.Where(x => x.Language == item.Language).FirstOrDefault();
                if (oldWsInfo == null)
                {
                    isNew = true;
                    oldWsInfo = new NccWebSiteInfo();
                    oldWsInfo.CreateBy = item.CreateBy;
                    oldWsInfo.CreationDate = item.CreationDate;
                    oldWsInfo.VersionNumber = item.VersionNumber;
                    oldWsInfo.VersionNumber = 0;
                    oldWsInfo.Language = item.Language;
                }

                oldWsInfo.Copyrights = item.Copyrights;
                oldWsInfo.FaviconUrl = item.FaviconUrl;
                //oldWsInfo.Language = item.Language;
                oldWsInfo.ModificationDate = modificationDate;
                oldWsInfo.ModifyBy = GlobalContext.GetCurrentUserId();
                oldWsInfo.Name = item.Name;
                oldWsInfo.PrivacyPolicyUrl = item.PrivacyPolicyUrl;
                oldWsInfo.SiteLogoUrl = item.SiteLogoUrl;
                oldWsInfo.SiteTitle = item.SiteTitle;
                oldWsInfo.Status = item.Status;
                oldWsInfo.Tagline = item.Tagline;
                oldWsInfo.TermsAndConditionsUrl = item.TermsAndConditionsUrl;
                oldWsInfo.VersionNumber += 1;
                oldWsInfo.Metadata = item.Metadata;

                if (isNew)
                {
                    oldEntity.WebSiteInfos.Add(oldWsInfo);
                }
            }
        }
    }
}
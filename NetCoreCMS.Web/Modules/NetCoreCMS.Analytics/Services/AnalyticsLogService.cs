using System;
using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Modules.Analytics.Repository;
using NetCoreCMS.Modules.Analytics.Models;
using Microsoft.EntityFrameworkCore;

namespace NetCoreCMS.Modules.Analytics.Services
{
    public class AnalyticsLogService : IBaseService<AnalyticsLogModel>
    {
        private readonly AnalyticsLogRepository _entityRepository;

        public AnalyticsLogService(AnalyticsLogRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public AnalyticsLogModel Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId);
        }

        public AnalyticsLogModel Save(AnalyticsLogModel entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public AnalyticsLogModel Update(AnalyticsLogModel entity)
        {
            var oldEntity = _entityRepository.Query().FirstOrDefault(x => x.Id == entity.Id);
            if (oldEntity != null)
            {
                using (var txn = _entityRepository.BeginTransaction())
                {
                    CopyNewData(oldEntity, entity);
                    //_entityRepository.Edit(oldEntity);
                    _entityRepository.SaveChange();
                    txn.Commit();
                }
            }

            return entity;
        }

        public void Remove(long entityId)
        {
            var entity = _entityRepository.Query().FirstOrDefault(x => x.Id == entityId);
            if (entity != null)
            {
                entity.Status = EntityStatus.Deleted;
                _entityRepository.Edit(entity);
                _entityRepository.SaveChange();
            }
        }

        public List<AnalyticsLogModel> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch);
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

        private void CopyNewData(AnalyticsLogModel oldEntity, AnalyticsLogModel entity)
        {
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Name = entity.Name;
            oldEntity.Status = entity.Status;


            oldEntity.VisitUrl = entity.VisitUrl;
            oldEntity.ReferrerUrl = entity.ReferrerUrl;
            oldEntity.Browser = entity.Browser;
            oldEntity.BrowserVersion = entity.BrowserVersion;
            oldEntity.Platform = entity.Platform;
            oldEntity.IsMobile = entity.IsMobile;
            oldEntity.MobileDeviceModel = entity.MobileDeviceModel;
            oldEntity.IpAddress = entity.IpAddress;
            oldEntity.BrowserAgent = entity.BrowserAgent;

            oldEntity.AnalyticsModel = entity.AnalyticsModel;
        }
    }
}
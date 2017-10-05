/*
 * Author: OnnoRokom Software Ltd
 * Website: http://onnorokomsoftware.com
 * Copyright (c) onnorokomsoftware.com
 * License: Commercial
*/
using System;
using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.UmsBdResult.Repository;
using NetCoreCMS.UmsBdResult.Models;

namespace NetCoreCMS.UmsBdResult.Services
{
    public class UmsBdResultService : IBaseService<UmsBdResultSettings>
    {
        private readonly UmsBdResultRepository _entityRepository;

        public UmsBdResultService()
        {
        }
        public UmsBdResultService(UmsBdResultRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public UmsBdResultSettings Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId);
        }

        public UmsBdResultSettings GetSettings()
        {
            return _entityRepository.Query().FirstOrDefault();
        }

        public UmsBdResultSettings Save(UmsBdResultSettings entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public UmsBdResultSettings Update(UmsBdResultSettings entity)
        {
            var oldEntity = _entityRepository.Query().FirstOrDefault(x => x.Id == entity.Id);
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
            var entity = _entityRepository.Query().FirstOrDefault(x => x.Id == entityId);
            if (entity != null)
            {
                entity.Status = EntityStatus.Deleted;
                _entityRepository.Edit(entity);
                _entityRepository.SaveChange();
            }
        }

        public List<UmsBdResultSettings> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
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

        private void CopyNewData(UmsBdResultSettings oldEntity, UmsBdResultSettings entity)
        {
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Name = entity.Name;
            oldEntity.Status = entity.Status;
            oldEntity.ApiKey = entity.ApiKey;
            oldEntity.BaseApi = entity.BaseApi;
            oldEntity.OrgBusinessId = entity.OrgBusinessId;
        }

    }
}

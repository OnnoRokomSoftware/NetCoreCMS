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
using NetCoreCMS.Branch.Repository;
using NetCoreCMS.Branch.Models;

namespace NetCoreCMS.Branch.Services
{
    public class NccBranchService : IBaseService<NccBranch>
    {
        private readonly NccBranchRepository _entityRepository;

        public NccBranchService()
        {
        }
        public NccBranchService(NccBranchRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public NccBranch Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId);
        }

        public NccBranch GetSettings()
        {
            return _entityRepository.Query().FirstOrDefault();
        }

        public NccBranch Save(NccBranch entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NccBranch Update(NccBranch entity)
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

        public List<NccBranch> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
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

        private void CopyNewData(NccBranch oldEntity, NccBranch entity)
        {
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Name = entity.Name;
            oldEntity.Status = entity.Status;

            oldEntity.Address = entity.Address;
            oldEntity.Description = entity.Description;
            oldEntity.Phone = entity.Phone;
            oldEntity.LatLon = entity.LatLon;
            oldEntity.GoogleLocation = entity.GoogleLocation;
            oldEntity.ZoomLevel = entity.ZoomLevel;
            oldEntity.IsAdmissionOnly = entity.IsAdmissionOnly;
            oldEntity.Order = entity.Order;
        }

        public List<NccBranch> LoadAllActive()
        {
            return _entityRepository.Query().Where(x => x.Status != EntityStatus.Deleted).ToList();
        }
    }
}

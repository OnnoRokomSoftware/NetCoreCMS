using System;
using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.LinkShare.Repository;
using NetCoreCMS.LinkShare.Models;
using Microsoft.EntityFrameworkCore;

namespace NetCoreCMS.LinkShare.Services
{
    public class NccLinkShareCategoryService : IBaseService<NccCategory>
    {
        private readonly NccCategoryRepository _entityRepository;

        public NccLinkShareCategoryService(NccCategoryRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public NccCategory Get(long entityId)
        {
            return _entityRepository.Query().FirstOrDefault(x => x.Id == entityId);
        }

        public NccCategory Save(NccCategory entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NccCategory Update(NccCategory entity)
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

        public List<NccCategory> LoadAll()
        {
            return _entityRepository.Query().ToList();
        }

        public List<NccCategory> LoadAllActive()
        {
            return _entityRepository.LoadAllActive();
        }

        public List<NccCategory> LoadAllByStatus(int status)
        {
            return _entityRepository.Query().Where(x => x.Status == status).ToList();
        }

        public List<NccCategory> LoadAllByName(string name)
        {
            return _entityRepository.Query().Where(x => x.Name == name).ToList();
        }

        public List<NccCategory> LoadAllByNameContains(string name)
        {
            return _entityRepository.Query().Where(x => x.Name.Contains(name)).ToList();
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

        private void CopyNewData(NccCategory oldEntity, NccCategory entity)
        {
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Name = entity.Name;
            oldEntity.Status = entity.Status;
        }
    }
}
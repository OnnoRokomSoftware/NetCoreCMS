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
    public class LsCategoryService : IBaseService<LsCategory>
    {
        private readonly LsCategoryRepository _entityRepository;

        public LsCategoryService(LsCategoryRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public LsCategory Get(long entityId)
        {
            return _entityRepository.Query().FirstOrDefault(x => x.Id == entityId);
        }

        public LsCategory Save(LsCategory entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public LsCategory Update(LsCategory entity)
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

        public List<LsCategory> LoadAll()
        {
            return _entityRepository.Query().Include("Links").ToList();
        }

        public List<LsCategory> LoadAllActive()
        {
            return _entityRepository.LoadAllActive();
        }

        public List<LsCategory> LoadAllByStatus(int status)
        {
            return _entityRepository.Query().Include("Links").Where(x => x.Status == status).ToList();
        }

        public List<LsCategory> LoadAllByName(string name)
        {
            return _entityRepository.Query().Include("Links").Where(x => x.Name == name).ToList();
        }

        public List<LsCategory> LoadAllByNameContains(string name)
        {
            return _entityRepository.Query().Include("Links").Where(x => x.Name.Contains(name)).ToList();
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

        private void CopyNewData(LsCategory oldEntity, LsCategory entity)
        {
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Name = entity.Name;
            oldEntity.Status = entity.Status;
        }
    }
}
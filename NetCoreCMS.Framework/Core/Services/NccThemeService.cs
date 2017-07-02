using System;
using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Framework.Core.Repository;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccThemeService : IBaseService<NccTheme>
    {
        private readonly NccThemeRepository _entityRepository;
         
        public NccThemeService(NccThemeRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }
         
        public NccTheme Get(long entityId)
        {
            return _entityRepository.Get(entityId);
        }

        public NccTheme Save(NccTheme entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NccTheme Update(NccTheme entity)
        {
            var oldEntity = _entityRepository.Get(entity.Id);
            if(oldEntity != null)
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
            var entity = _entityRepository.Get(entityId );
            if (entity != null)
            {
                entity.Status = EntityStatus.Deleted;
                _entityRepository.Edit(entity);
                _entityRepository.SaveChange();
            }
        }

        public List<NccTheme> LoadAll()
        {
            return _entityRepository.LoadAll();
        }

        public List<NccTheme> LoadAllByStatus(int status)
        {
            return _entityRepository.LoadAllByStatus(status);
        }

        public List<NccTheme> LoadAllByName(string name)
        {
            return _entityRepository.LoadAllByName(name);
        }

        public List<NccTheme> LoadAllByNameContains(string name)
        {
            return _entityRepository.LoadAllByNameContains(name);
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

        private void CopyNewData(NccTheme oldEntity, NccTheme entity)
        {
             
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Name = entity.Name; 
            oldEntity.Status = entity.Status;
            oldEntity.ThemeId = entity.ThemeId;
            oldEntity.CreateBy = entity.CreateBy;
            oldEntity.CreationDate = entity.CreationDate;
            oldEntity.NetCoreCMSVersion = entity.NetCoreCMSVersion;
            oldEntity.Status = entity.Status;
            oldEntity.Version = entity.Version;
            oldEntity.VersionNumber = entity.VersionNumber;
            oldEntity.PreviewImage = entity.PreviewImage;
            oldEntity.ThemeName = entity.ThemeName;
            oldEntity.Type = entity.Type;
            oldEntity.VersionNumber = entity.VersionNumber;
            
        }
        
    }
}

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
            return _entityRepository.Query().FirstOrDefault(x => x.Id == entityId);
        }

        public NccTheme Save(NccTheme entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NccTheme Update(NccTheme entity)
        {
            var oldEntity = _entityRepository.Query().FirstOrDefault(x => x.Id == entity.Id);
            if(oldEntity != null)
            {
                using (var txn = _entityRepository.BeginTransaction())
                {
                    CopyNewData(oldEntity, entity);
                    _entityRepository.Eidt(oldEntity);
                    _entityRepository.SaveChange();
                    txn.Commit();
                }
            }
            
            return entity;
        }
        
        public void Remove(long entityId)
        {
            var entity = _entityRepository.Query().FirstOrDefault(x => x.Id == entityId );
            if (entity != null)
            {
                entity.Status = EntityStatus.Deleted;
                _entityRepository.Eidt(entity);
                _entityRepository.SaveChange();
            }
        }

        public List<NccTheme> LoadAll()
        {
            return _entityRepository.Query().ToList();
        }

        public List<NccTheme> LoadAllByStatus(int status)
        {
            return _entityRepository.Query().Where(x => x.Status == status).ToList();
        }

        public List<NccTheme> LoadAllByName(string name)
        {
            return _entityRepository.Query().Where(x => x.Name == name).ToList();
        }

        public List<NccTheme> LoadAllByNameContains(string name)
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

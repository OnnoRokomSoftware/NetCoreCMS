using System;
using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.ImageSlider.Repository;
using NetCoreCMS.ImageSlider.Models;

namespace NetCoreCMS.Notice.Services
{
    public class NccImageSliderService : IBaseService<NccImageSlider>
    {
        private readonly NccImageSliderRepository _entityRepository;
 
        public NccImageSliderService(NccImageSliderRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }
         
        public NccImageSlider Get(long entityId)
        {
            return _entityRepository.Query().FirstOrDefault(x => x.Id == entityId);
        }

        public NccImageSlider Save(NccImageSlider entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NccImageSlider Update(NccImageSlider entity)
        {
            var oldEntity = _entityRepository.Query().FirstOrDefault(x => x.Id == entity.Id);
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
            var entity = _entityRepository.Query().FirstOrDefault(x => x.Id == entityId );
            if (entity != null)
            {
                entity.Status = EntityStatus.Deleted;
                _entityRepository.Edit(entity);
                _entityRepository.SaveChange();
            }
        }

        public List<NccImageSlider> LoadAll()
        {
            return _entityRepository.Query().ToList();
        }

        public List<NccImageSlider> LoadAllActive()
        {
            return _entityRepository.LoadAllActive();
        }

        public List<NccImageSlider> LoadAllByStatus(int status)
        {
            return _entityRepository.Query().Where(x => x.Status == status).ToList();
        }

        public List<NccImageSlider> LoadAllByName(string name)
        {
            return _entityRepository.Query().Where(x => x.Name == name).ToList();
        }

        public List<NccImageSlider> LoadAllByNameContains(string name)
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

        private void CopyNewData(NccImageSlider oldEntity, NccImageSlider entity)
        {   
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Name = entity.Name;
            oldEntity.Status = entity.Status;
            oldEntity.ContainerStyle = entity.ContainerStyle;
            oldEntity.Interval = entity.Interval;
            oldEntity.ShowNav= entity.ShowNav;
            oldEntity.ShowSideNav= entity.ShowSideNav;
            oldEntity.ImageWidth= entity.ImageWidth;
            oldEntity.ImageHeight= entity.ImageHeight;
            oldEntity.ImageItems= entity.ImageItems; 
        } 
    }
}

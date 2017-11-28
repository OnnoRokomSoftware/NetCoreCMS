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
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.ImageSlider.Repository;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.ImageSlider.Models.Entity;

namespace NetCoreCMS.ImageSlider.Services
{
    public class NccImageSliderService : IBaseService<NccImageSlider>
    {
        private readonly NccImageSliderRepository _entityRepository;
        private readonly NccImageSliderItemRepository _itemRepository;

        public NccImageSliderService(NccImageSliderRepository entityRepository, NccImageSliderItemRepository itemRepository)
        {
            _entityRepository = entityRepository;
            _itemRepository = itemRepository;
        }

        public NccImageSlider Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId, false, new List<string>() { "ImageItems" });
            //return _entityRepository.Query().Include("ImageItems").FirstOrDefault(x => x.Id == entityId);
        }

        public NccImageSlider Save(NccImageSlider entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NccImageSlider Update(NccImageSlider entity)
        {
            var oldEntity = _entityRepository.Query().Include("ImageItems").FirstOrDefault(x => x.Id == entity.Id);
            if (oldEntity != null)
            {
                using (var txn = _entityRepository.BeginTransaction())
                {
                    foreach (var item in oldEntity.ImageItems)
                    {
                        _itemRepository.DeletePermanently(item);
                    }

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
            var entity = _entityRepository.Query().Include("ImageItems").FirstOrDefault(x => x.Id == entityId);
            if (entity != null)
            {
                entity.Status = EntityStatus.Deleted;
                _entityRepository.Edit(entity);
                _entityRepository.SaveChange();
            }
        }

        public List<NccImageSlider> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch, new List<string>() { "ImageItems" });
        }

        public void DeletePermanently(long entityId)
        {
            var entity = _entityRepository.Query().Include("ImageItems").FirstOrDefault(x => x.Id == entityId);
            if (entity != null)
            {
                foreach (var item in entity.ImageItems)
                {
                    _itemRepository.DeletePermanently(item);
                }
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
            oldEntity.ShowNav = entity.ShowNav;
            oldEntity.ShowSideNav = entity.ShowSideNav;
            oldEntity.ImageWidth = entity.ImageWidth;
            oldEntity.ImageHeight = entity.ImageHeight;
            oldEntity.ImageItems = entity.ImageItems;
        }
    }
}
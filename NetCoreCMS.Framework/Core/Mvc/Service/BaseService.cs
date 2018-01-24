using System;
using System.Collections.Generic;
using NetCoreCMS.Framework.Core.Exceptions;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Framework.Utility;

namespace NetCoreCMS.Framework.Core.Mvc.Service
{
    public class BaseService<EntityT> : IBaseService<EntityT>
    {
        #region Initialization
        private readonly IBaseRepository<EntityT, long> _repository;
        protected readonly List<string> _defaultIncludeRelations = new List<string>();

        public BaseService(IBaseRepository<EntityT, long> repository, List<string> defaultIncludeRelations = null)
        {
            _repository = repository;
            if (defaultIncludeRelations == null)
            {
                defaultIncludeRelations = new List<string>();
            }

            _repository.DefaultIncludedRelationalProperties = _defaultIncludeRelations = defaultIncludeRelations;
        }
        #endregion

        #region Single instance load
        public virtual EntityT Get(long entityId, bool isAsNoTracking = false, bool withDeleted = false)
        {
            return _repository.Get(entityId, isAsNoTracking, _defaultIncludeRelations, withDeleted);
        }
        #endregion

        #region List Load
        public virtual List<EntityT> LoadAll(bool isActive = true, int status = 0, string name = "", bool isLikeSearch = false, bool withDeleted = false)
        {
            return _repository.LoadAll(isActive, status, name, isLikeSearch, _defaultIncludeRelations, withDeleted);
        }
        #endregion

        #region Save Operation
        public virtual bool DoBeforeSave(EntityT entity)
        {
            return true;
        }

        public virtual EntityT Save(EntityT entity)
        {
            if (DoBeforeSave(entity))
            {
                using (var txn = _repository.BeginTransaction())
                {
                    try
                    {
                        var model = (IBaseModel<long>)entity;
                        model.ModifyBy = model.CreateBy = GlobalContext.GetCurrentUserId();
                        model.VersionNumber = 1;
                        _repository.Add(entity);
                        _repository.SaveChange();
                        txn.Commit();
                    }
                    catch (Exception ex)
                    {
                        txn.Rollback();
                        throw ex;
                    }
                }
                DoAfterSave(entity);
            }

            return entity;
        }

        public virtual void DoAfterSave(EntityT entity)
        {

        }
        #endregion

        #region Update Operation
        public virtual bool DoBeforeUpdate(EntityT entity)
        {
            return true;
        }

        public virtual EntityT Update(EntityT entity)
        {
            var model = (IBaseModel<long>)entity;

            if (DoBeforeUpdate(entity))
            {
                var oldEntity = _repository.Get(model.Id, false, _defaultIncludeRelations);
                if (oldEntity != null)
                {
                    var oldModel = (IBaseModel<long>)oldEntity;
                    oldModel.ModifyBy = GlobalContext.GetCurrentUserId();

                    using (var txn = _repository.BeginTransaction())
                    {
                        try
                        {
                            CopyNewData(entity, oldEntity);
                            _repository.Edit(oldEntity);
                            _repository.SaveChange();
                            txn.Commit();
                        }
                        catch (Exception ex)
                        {
                            txn.Rollback();
                            throw ex;
                        }
                    }
                    DoAfterUpdate(entity);
                }
                else
                {
                    throw new InvalidEntityDataException("Null Entity.");
                }
            }

            return entity;
        }

        public virtual void DoAfterUpdate(EntityT entity)
        {

        }
        #endregion

        #region Delete Operation
        public virtual bool DoBeforeDelete(long entityId)
        {
            return true;
        }

        public virtual void Remove(long entityId)
        {
            if (DoBeforeDelete(entityId))
            {
                using (var txn = _repository.BeginTransaction())
                {
                    try
                    {
                        var entity = _repository.Get(entityId);
                        var model = (IBaseModel<long>)entity;
                        if (model != null)
                        {
                            model.Status = EntityStatus.Deleted;
                            model.VersionNumber += 1;
                            _repository.Edit(entity);
                            _repository.SaveChange();
                        }
                        txn.Commit();
                    }
                    catch (Exception ex)
                    {
                        txn.Rollback();
                        throw ex;
                    }
                }
                DoAfterDelete(entityId);
            }
        }

        public virtual void DeletePermanently(long entityId)
        {
            if (DoBeforeDelete(entityId))
            {
                using (var txn = _repository.BeginTransaction())
                {
                    try
                    {
                        var model = _repository.Get(entityId);
                        if (model != null)
                        {
                            _repository.Remove(model);
                            _repository.SaveChange();
                        }
                        txn.Commit();
                    }
                    catch (Exception ex)
                    {
                        txn.Rollback();
                        throw ex;
                    }
                }
                DoAfterDelete(entityId);
            }
        }

        public virtual void DoAfterDelete(long entityId)
        {

        }
        #endregion

        #region Helper
        public virtual void CopyNewData(EntityT entity, EntityT oldEntity)
        {
            var entityType = entity.GetType();
            var oldEntityType = oldEntity.GetType();

            var entityProperties = entityType.GetProperties();

            for (int i = 0; i < entityProperties.Length; i++)
            {
                var entityProperty = entityProperties[i];
                if (entityProperty.Name != "Id" && entityProperty.Name != "CreateBy" && entityProperty.Name != "CreationDate" && entityProperty.Name != "ModificationDate" && entityProperty.Name != "ModifyBy" && entityProperty.PropertyType.Name.StartsWith("List") == false)
                {
                    var oldEntityProperty = oldEntityType.GetProperty(entityProperty.Name);
                    oldEntityProperty.SetValue(oldEntity, entityProperty.GetValue(entity));
                }
            }

            AfterCopyData(entity, oldEntity);
            //return to;
        }

        public virtual void AfterCopyData(EntityT entity, EntityT oldEntity)
        {

        }
        #endregion
    }
}

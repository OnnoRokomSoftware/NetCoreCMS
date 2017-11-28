/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.IoC;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Setup;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;


namespace NetCoreCMS.Framework.Core.Mvc.Repository
{
    public class BaseRepository<EntityT, IdT> : IBaseRepository<EntityT, IdT> where EntityT : class, IBaseModel<IdT>
    {
        public BaseRepository(NccDbContext context)
        {
            Context = context;
            DbSet = Context.Set<EntityT>();
        }

        protected DbContext Context { get; }

        protected DbSet<EntityT> DbSet { get; }

        public EntityT Add(EntityT entity)
        {
            entity.CreationDate = entity.ModificationDate = DateTime.Now;
            DbSet.Add(entity);
            return entity;
        }

        public EntityT Edit(EntityT entity)
        {
            //entity.Status = EntityStatus.Modified;
            entity.ModificationDate = DateTime.Now;
            Context.Entry(entity).State = EntityState.Modified;

            return entity;
        }

        public EntityT Get(IdT id, bool isAsNoTracking = false, List<string> includeRelationalProperties = null)
        {
            IQueryable<EntityT> tempDbSet = DbSet;

            if (includeRelationalProperties != null)
            {
                foreach (var item in includeRelationalProperties)
                {
                    tempDbSet = tempDbSet.Include(item);
                }
            }

            if (isAsNoTracking)
                tempDbSet = tempDbSet.AsNoTracking().Where(x => x.Id.Equals(id));
            else
                tempDbSet = tempDbSet.Where(x => x.Id.Equals(id));


            return tempDbSet.FirstOrDefault();
        }

        public EntityT Get(string name, bool isAsNoTracking = false, List<string> includeRelationalProperties = null)
        {
            IQueryable<EntityT> tempDbSet = DbSet;

            if (includeRelationalProperties != null)
            {
                foreach (var item in includeRelationalProperties)
                {
                    tempDbSet = tempDbSet.Include(item);
                }
            }

            if (isAsNoTracking)
                tempDbSet = tempDbSet.AsNoTracking().Where(x => x.Name.Equals(name));
            else
                tempDbSet = tempDbSet.Where(x => x.Name.Equals(name));

            return tempDbSet.FirstOrDefault();
        }

        public List<EntityT> Load(string name, List<string> includeRelationalProperties = null)
        {
            IQueryable<EntityT> tempDbSet = DbSet;

            if (includeRelationalProperties != null)
            {
                foreach (var item in includeRelationalProperties)
                {
                    tempDbSet = tempDbSet.Include(item);
                }
            }
            
            tempDbSet = tempDbSet.Where(x => x.Name.Contains(name));
            return tempDbSet.ToList();
        }

        public List<EntityT> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false, List<string> includeRelationalProperties = null)
        {
            IQueryable<EntityT> tempDbSet = DbSet.Where(x => x.Status != EntityStatus.Deleted);

            if (includeRelationalProperties != null)
            {
                foreach (var item in includeRelationalProperties)
                {
                    tempDbSet = tempDbSet.Include(item);
                }
            }

            if (isActive)
            {
                tempDbSet = tempDbSet.Where(x => x.Status == EntityStatus.Active);
            }

            if (status >= 0)
            {
                tempDbSet = tempDbSet.Where(x => x.Status == status);
            }
            if (!string.IsNullOrEmpty(name))
            {
                if (isLikeSearch)
                    tempDbSet = tempDbSet.Where(x => x.Name.ToLower().Contains(name.ToLower()));
                else
                    tempDbSet = tempDbSet.Where(x => x.Name.ToLower() == name.ToLower());
            }
                                                                                                                                                                                                    
            // :) 
            return tempDbSet.ToList();
        }
        
        public IDbContextTransaction BeginTransaction()
        {
            return Context.Database.BeginTransaction();            
        }

        public void SaveChange()
        {
            Context.SaveChanges();
        }

        public IQueryable<EntityT> Query()
        {
            return DbSet;
        }

        public EntityEntry GetEntityEntry(EntityT T)
        {
            return (EntityEntry)Context.Entry(T);
        }

        public void Remove(EntityT entity)
        {
            DbSet.Remove(entity);
        }

        public void DeletePermanently(EntityT entity)
        {
            DbSet.Remove(entity);
        }

        public int ExecuteSqlCommand(NccDbQueryText query)
        {
            var queryText = "";
            if (SetupHelper.SelectedDatabase == "SqLite")
            {
                queryText = query.SQLite_QueryText;
            }
            else if (SetupHelper.SelectedDatabase == "MSSQL")
            {
                queryText = query.MSSql_QueryText;
            }
            else if (SetupHelper.SelectedDatabase == "MySql")
            {
                queryText = query.MySql_QueryText;
            }
            else
            {
                return -1;
            }

            var effRow = Context.Database.ExecuteSqlCommand(queryText);
            return effRow;
        }

        public List<T> ExecuteSqlQuery<T>(NccDbQueryText query)
        {
            var entityType = typeof(T);
            var list = new List<T>();

            var queryText = "";
            if (SetupHelper.SelectedDatabase == "SqLite")
            {
                queryText = query.SQLite_QueryText;
            }
            else if (SetupHelper.SelectedDatabase == "MSSQL")
            {
                queryText = query.MSSql_QueryText;
            }
            else if (SetupHelper.SelectedDatabase == "MySql")
            {
                queryText = query.MySql_QueryText;
            }
            else
            {
                throw new System.Exception("No supported database found.");
            }

            var conn = Context.Database.GetDbConnection();
            try
            {
                var entityProperties = entityType.GetProperties();
                conn.Open();
                using (var command = conn.CreateCommand())
                {                    
                    command.CommandText = queryText;
                    DbDataReader reader = command.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        var objArr = new object[reader.FieldCount];
                        var value = reader.GetValues(objArr);
                        var obj = Activator.CreateInstance<T>();
                        for (int i = 0; i < entityProperties.Length; i++)
                        {
                            var prop = entityProperties[i];
                            var ordinal = reader.GetOrdinal(prop.Name);
                            if(ordinal >= 0)
                            {
                                if (prop.PropertyType == typeof(Int16))
                                {
                                    var val = reader.GetInt16(ordinal);
                                    var propInfo = entityType.GetProperty(prop.Name);
                                    propInfo.SetValue(obj, val);
                                }
                                else if(prop.PropertyType == typeof(int))
                                {
                                    var val = reader.GetInt32(ordinal);
                                    var propInfo = entityType.GetProperty(prop.Name);
                                    propInfo.SetValue(obj, val);
                                }
                                else if (prop.PropertyType == typeof(long))
                                {
                                    var val = reader.GetInt64(ordinal);
                                    var propInfo = entityType.GetProperty(prop.Name);
                                    propInfo.SetValue(obj, val);
                                }
                                else if (prop.PropertyType == typeof(string))
                                {
                                    var val = reader.GetString(ordinal);
                                    var propInfo = entityType.GetProperty(prop.Name);
                                    propInfo.SetValue(obj, val);
                                }
                                else if (prop.PropertyType == typeof(float))
                                {
                                    var val = reader.GetFloat(ordinal);
                                    var propInfo = entityType.GetProperty(prop.Name);
                                    propInfo.SetValue(obj, val);
                                }
                                else if (prop.PropertyType == typeof(double))
                                {
                                    var val = reader.GetDouble(ordinal);
                                    var propInfo = entityType.GetProperty(prop.Name);
                                    propInfo.SetValue(obj, val);
                                }
                                else if (prop.PropertyType == typeof(decimal))
                                {
                                    var val = reader.GetDecimal(ordinal);
                                    var propInfo = entityType.GetProperty(prop.Name);
                                    propInfo.SetValue(obj, val);
                                }
                                else if (prop.PropertyType == typeof(bool))
                                {
                                    var val = reader.GetBoolean(ordinal);
                                    var propInfo = entityType.GetProperty(prop.Name);
                                    propInfo.SetValue(obj, val);
                                }
                                else if (prop.PropertyType == typeof(DateTime))
                                {
                                    var val = reader.GetDateTime(ordinal);
                                    var propInfo = entityType.GetProperty(prop.Name);
                                    propInfo.SetValue(obj, val);
                                }
                                else if (prop.PropertyType == typeof(double))
                                {
                                    var val = reader.GetDouble(ordinal);
                                    var propInfo = entityType.GetProperty(prop.Name);
                                    propInfo.SetValue(obj, val);
                                }
                            }
                        }
                        list.Add(obj);
                    }
                    
                    reader.Dispose();
                }
            }
            finally
            {
                conn.Close();
            }
            return list;
        }

        public ArrayList ExecuteSqlQuery(NccDbQueryText query)
        {   
            var list = new ArrayList();

            var queryText = "";
            if (SetupHelper.SelectedDatabase == "SqLite")
            {
                queryText = query.SQLite_QueryText;
            }
            else if (SetupHelper.SelectedDatabase == "MSSQL")
            {
                queryText = query.MSSql_QueryText;
            }
            else if (SetupHelper.SelectedDatabase == "MySql")
            {
                queryText = query.MySql_QueryText;
            }
            else
            {
                throw new Exception("No supported database found.");
            }

            var conn = Context.Database.GetDbConnection();
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = queryText;
                    DbDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var objArr = new object[reader.FieldCount];
                        var value = reader.GetValues(objArr);                        
                        list.Add(objArr);
                    }
                    reader.Dispose();
                }
            }
            finally
            {
                conn.Close();
            }
            return list;
        }
    }
}

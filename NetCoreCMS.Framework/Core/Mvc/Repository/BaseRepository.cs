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
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Setup;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;


namespace NetCoreCMS.Framework.Core.Mvc.Repository
{
    public class BaseRepository<EntityT, IdT> : IBaseRepository<EntityT, IdT> where EntityT : class, IBaseModel<IdT>
    {
        public List<string> DefaultIncludedRelationalProperties { get; set; }

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
            entity.VersionNumber += 1;
            Context.Entry(entity).State = EntityState.Modified;

            return entity;
        }

        public EntityT Get(IdT id, bool isAsNoTracking = false, List<string> extraIncludeRelationalProperties = null, bool withDeleted = false)
        {
            IQueryable<EntityT> query = Query(null, true, extraIncludeRelationalProperties, isAsNoTracking, withDeleted);
            return query.Where(x => x.Id.Equals(id)).FirstOrDefault();
        }

        public EntityT Get(string name, bool isAsNoTracking = false, List<string> includeRelationalProperties = null, bool withDeleted = false)
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

            if (withDeleted == false)
                tempDbSet = tempDbSet.Where(x => x.Status != EntityStatus.Deleted);

            return tempDbSet.FirstOrDefault();  
        }

        public List<EntityT> Load(string name, List<string> includeRelationalProperties = null, bool withDeleted = false)
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

            if (withDeleted == false)
                tempDbSet = tempDbSet.Where(x => x.Status != EntityStatus.Deleted);

            return tempDbSet.ToList(); 
        }

        public List<EntityT> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false, List<string> includeRelationalProperties = null, bool withDeleted = false)
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

            if (withDeleted == false)
                tempDbSet = tempDbSet.Where(x => x.Status != EntityStatus.Deleted);

            /* :) 
             * 1. If you are installing newly then check setup.json is exists. Delete setup.json and start setup again.
             * 2. If you are here after installing then may be database tables are missing or databasae server is not running or models are changed have to update database. 
             */

            return tempDbSet.ToList();  
        }
        
        public IDbContextTransaction BeginTransaction()
        {
            return Context.Database.BeginTransaction();            
        }

        public void SaveChange()
        {
            var effectedChanges = Context.SaveChanges();
        }

        public IQueryable<EntityT> Query( bool? isActive = null, bool includeDefaultRelationProperties = true, List<string> extraIncludeRelationalProperties = null, bool isAsNoTracking = false, bool withDeleted = false)
        {
            IQueryable<EntityT> query = DbSet;

            if (isActive != null)
            {
                if (isActive.Value)
                {
                    query = query.Where(x => x.Status == EntityStatus.Active);
                }
                else
                {
                    query = query.Where(x => x.Status == EntityStatus.Inactive);
                }
            }

            if (includeDefaultRelationProperties && DefaultIncludedRelationalProperties != null)
            {
                foreach (var item in DefaultIncludedRelationalProperties)
                {
                    query = query.Include(item);
                }
            }

            if (extraIncludeRelationalProperties != null)
            {
                foreach (var item in extraIncludeRelationalProperties)
                {
                    if (DefaultIncludedRelationalProperties == null || DefaultIncludedRelationalProperties.Contains(item) == false)
                    {
                        query = query.Include(item);
                    }
                }
            }

            if (isAsNoTracking)
            {
                query = query.AsNoTracking();
            }

            if (withDeleted == false)
            {
                query = query.Where(x => x.Status != EntityStatus.Deleted);
            }

            return query;
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
             
            return Context.Database.ExecuteSqlCommand(queryText); 
        }

        public List<T> ExecuteSqlQuery<T>(NccDbQueryText query, int timeout = 60, CommandType commandType = CommandType.Text)
        {
            var isNewConnection = false;
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
                throw new Exception("No supported database found.");
            }

            var conn = Context.Database.GetDbConnection();
            
            try
            {
                var entityProperties = entityType.GetProperties();                
                if (conn.State == ConnectionState.Closed)
                {
                    conn.OpenAsync().Wait();
                    isNewConnection = true;
                }
                
                using (var command = conn.CreateCommand())
                {
                    command.CommandTimeout = timeout;
                    command.CommandType = commandType;
                    command.CommandText = queryText;
                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                var objArr = new object[reader.FieldCount];
                                var value = reader.GetValues(objArr);
                                var obj = Activator.CreateInstance<T>();
                                for (int i = 0; i < entityProperties.Length; i++)
                                {
                                    var prop = entityProperties[i];
                                    var ordinal = reader.GetOrdinal(prop.Name);
                                    if (ordinal >= 0)
                                    {
                                        if (prop.PropertyType == typeof(Int16))
                                        {
                                            var val = reader.GetInt16(ordinal);
                                            var propInfo = entityType.GetProperty(prop.Name);
                                            propInfo.SetValue(obj, val);
                                        }
                                        else if (prop.PropertyType == typeof(int))
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
                            catch (Exception ex)
                            {
                                reader.Close();
                                throw ex;
                            }
                        }
                        reader.Close();
                    }
                }

                if (isNewConnection && conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
            catch(Exception ex)
            {
                if (isNewConnection && conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                throw ex;
            }
            
            return list;
        }

        public ArrayList ExecuteSqlQuery(NccDbQueryText query, int timeout = 60, CommandType commandType = CommandType.Text)
        {   
            var list = new ArrayList();
            var isNewConnection = false;

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
                if (conn.State == ConnectionState.Closed)
                {
                    conn.OpenAsync().Wait();
                    isNewConnection = true;
                }

                using (var command = conn.CreateCommand())
                    {
                        command.CommandTimeout = timeout;
                        command.CommandType = commandType;
                        command.CommandText = queryText;
                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            try
                            {
                                while (reader.Read())
                                {
                                    var objArr = new object[reader.FieldCount];
                                    var value = reader.GetValues(objArr);
                                    list.Add(objArr);
                                }
                            }
                            catch (Exception ex)
                            {
                                reader.Close();
                                throw ex;
                            }
                            reader.Close();
                        }
                    }

                if (isNewConnection && conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
            catch(Exception ex)
            {
                if (isNewConnection && conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }

                throw ex;
            }

            return list;
        }
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using NetCoreCMS.Framework.Core.Models;

namespace NetCoreCMS.Framework.Core.Mvc.Cache
{
    public static class NccGlobalCache
    {
        public static NccUser GetNccUser(this IMemoryCache cache, long userId)
        {
            Hashtable cacheEntry;
            cache.TryGetValue(NccCacheKeys.UsersCache, out cacheEntry);

            if(cacheEntry != null)
            {
                if (cacheEntry.ContainsKey(userId))
                {
                    return (NccUser) cacheEntry[userId];
                }
            }
            else
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(20));
                cache.Set(NccCacheKeys.UsersCache, new Hashtable(), cacheEntryOptions);
            }

            return null;
        }

        public static void SetNccUser(this IMemoryCache cache, NccUser nccUser)
        {
            if (nccUser != null)
            {
                Hashtable cacheEntry;
                cache.TryGetValue(NccCacheKeys.UsersCache, out cacheEntry);

                if (cacheEntry != null)
                {
                    cacheEntry[nccUser.Id] = nccUser;
                }
                else
                {
                    cacheEntry = new Hashtable();
                    cacheEntry[nccUser.Id] = nccUser;
                    var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(20));
                    cache.Set(NccCacheKeys.UsersCache, cacheEntry, cacheEntryOptions);
                }  
            }           
        }

        public static void ClearNccUserCache(this IMemoryCache cache)
        {
            cache.Remove(NccCacheKeys.UsersCache);
        }

        public static void RemoveNccUserFromCache(this IMemoryCache cache, long entityId)
        {
            Hashtable cacheEntry;
            cache.TryGetValue(NccCacheKeys.UsersCache, out cacheEntry);

            if (cacheEntry != null)
            {
                if (cacheEntry.ContainsKey(entityId))
                {
                    cacheEntry.Remove(entityId);
                }
            }            
        }

        public static NccWebSiteWidget GetNccWebSiteWidget(this IMemoryCache cache, long entityId)
        {
            Hashtable cacheEntry;
            cache.TryGetValue(NccCacheKeys.WebSiteWidgetCache, out cacheEntry);

            if (cacheEntry != null)
            {
                if (cacheEntry.ContainsKey(entityId))
                {
                    return (NccWebSiteWidget)cacheEntry[entityId];
                }
            }
            else
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(20));
                cache.Set(NccCacheKeys.WebSiteWidgetCache, new Hashtable(), cacheEntryOptions);
            }

            return null;
        }

        public static void SetNccWebSiteWidget(this IMemoryCache cache, NccWebSiteWidget entity)
        {
            if (entity != null)
            {
                Hashtable cacheEntry;
                cache.TryGetValue(NccCacheKeys.WebSiteWidgetCache, out cacheEntry);

                if (cacheEntry != null)
                {
                    cacheEntry[entity.Id] = entity;
                }
                else
                {
                    cacheEntry = new Hashtable();
                    cacheEntry[entity.Id] = entity;
                    var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(20));
                    cache.Set(NccCacheKeys.WebSiteWidgetCache, cacheEntry, cacheEntryOptions);

                }
            }
        }

        public static void ClearNccWebSiteWidget(this IMemoryCache cache)
        {
            cache.Remove(NccCacheKeys.WebSiteWidgetCache);
        }

        public static void RemoveNccWebSiteWidgetFromCache(this IMemoryCache cache, long entityId)
        {
            Hashtable cacheEntry;
            cache.TryGetValue(NccCacheKeys.WebSiteWidgetCache, out cacheEntry);

            if (cacheEntry != null)
            {
                if (cacheEntry.ContainsKey(entityId))
                {
                    cacheEntry.Remove(entityId);
                }
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using NetCoreCMS.Framework.Core.Models;

namespace NetCoreCMS.Framework.Core.Mvc.Cache
{
    internal static class NccGlobalCache
    {
        internal static NccUser GetNccUser(this IMemoryCache cache, long userId)
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

        internal static void SetNccUser(this IMemoryCache cache, NccUser nccUser)
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

        internal static void ClearNccUserCache(this IMemoryCache cache)
        {
            cache.Remove(NccCacheKeys.UsersCache);
        }
    }
}

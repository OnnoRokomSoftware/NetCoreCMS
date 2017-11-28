using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace NetCoreCMS.Framework.Core.Mvc.Cache
{
    public class NccResponseCacheClient : ICacheClient
    {
        private readonly IMemoryCache _cache;

        public NccResponseCacheClient(IMemoryCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public bool TryGet<T>(string key, out T entry)
        {
            return _cache.TryGetValue(NccCacheKeys.CacheKeyPrefix + key, out entry);
        }

        public void Set(string key, object entry, TimeSpan expiry, params string[] tags)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry
            };

            var allTokenSource = _cache.GetOrCreate(NccCacheKeys.CacheTagPrefix + NccCacheKeys.AllTag,
                allTagEntry => new CancellationTokenSource());

            options.AddExpirationToken(new CancellationChangeToken(allTokenSource.Token));

            foreach (var tag in tags)
            {
                var tokenSource = _cache.GetOrCreate(NccCacheKeys.CacheTagPrefix + tag, tagEntry =>
                {
                    tagEntry.AddExpirationToken(new CancellationChangeToken(allTokenSource.Token));

                    return new CancellationTokenSource();
                });

                options.AddExpirationToken(new CancellationChangeToken(tokenSource.Token));
            }

            _cache.Set(NccCacheKeys.CacheKeyPrefix + key, entry, options);
        }

        public void Remove(string key)
        {
            _cache.Remove(NccCacheKeys.CacheKeyPrefix + key);
        }

        public void RemoveByTag(string tag)
        {
            if (_cache.TryGetValue(NccCacheKeys.CacheTagPrefix + tag, out CancellationTokenSource tokenSource))
            {
                tokenSource.Cancel();

                _cache.Remove(NccCacheKeys.CacheTagPrefix + tag);
            }
        }

        public void RemoveAll()
        {
            RemoveByTag(NccCacheKeys.AllTag);
        }
    }

    public interface ICacheClient
    {
        bool TryGet<T>(string key, out T entry);
        void Set(string key, object entry, TimeSpan expiry, params string[] tags);
        void Remove(string key);
        void RemoveByTag(string tag);
        void RemoveAll();
    }
}
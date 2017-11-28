using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.Mvc.Cache
{
    internal class NccCacheKeys
    {
        internal const string UsersCache        = "NCC_USERS_CACHE";

        internal const string CacheKeyPrefix    = "NCC_CACHE_";
        internal const string ClientDuration    = "NCC_CACHE_CLIENT_DURATION";
        internal const string ServerDuration    = "NCC_CACHE_SERVER_DURATION";
        internal const string Tags              = "NCC_CACHE_TAGS";
        internal const string CacheTagPrefix    = "NCC_CACHE_TAG_";
        internal const string AllTag            = "NCC_ALL_TAG";
    }
}

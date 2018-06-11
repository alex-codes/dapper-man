using System;
using System.Runtime.Caching;

namespace DapperMan.Core
{
    public class PropertyCache
    {
        public ObjectCache Cache { get; set; }
        public CacheItemPolicy Policy { get; set; }
        public static CacheItemPolicy DefaultCachePolicy => new CacheItemPolicy { SlidingExpiration = DefaultExpiry };
        public static TimeSpan DefaultExpiry => TimeSpan.FromMinutes(15);

        public PropertyCache(ObjectCache cache)
            : this(cache, DefaultCachePolicy)
        {

        }

        public PropertyCache(ObjectCache cache, CacheItemPolicy policy)
        {
            Cache = cache ?? throw new ArgumentNullException(nameof(cache));
            Policy = policy ?? throw new ArgumentNullException(nameof(policy));
        }
    }
}

using System;
using System.Runtime.Caching;

namespace DapperMan.Core
{
    /// <summary>
    /// An object used for caching information about the typed object.
    /// </summary>
    public class PropertyCache
    {
        /// <summary>
        /// The underlying cache instance.
        /// </summary>
        public ObjectCache Cache { get; set; }

        /// <summary>
        /// A cache policy to be applied to all cached items.
        /// </summary>
        public CacheItemPolicy Policy { get; set; }

        /// <summary>
        /// The default cache policy if none is provided.
        /// </summary>
        public static CacheItemPolicy DefaultCachePolicy => new CacheItemPolicy { SlidingExpiration = DefaultExpiry };

        /// <summary>
        /// The default cache expiration if none is provided.
        /// </summary>
        public static TimeSpan DefaultExpiry => TimeSpan.FromMinutes(15);

        /// <summary>
        /// Creates a new PropertyCache.
        /// </summary>
        /// <param name="cache">A cache instance.</param>
        public PropertyCache(ObjectCache cache)
            : this(cache, DefaultCachePolicy)
        {

        }

        /// <summary>
        /// Creates a new PropertyCache.
        /// </summary>
        /// <param name="cache">A cache instance.</param>
        /// <param name="policy">A cache plicy to be applied to all cached items.</param>
        public PropertyCache(ObjectCache cache, CacheItemPolicy policy)
        {
            Cache = cache ?? throw new ArgumentNullException(nameof(cache));
            Policy = policy ?? throw new ArgumentNullException(nameof(policy));
        }
    }
}

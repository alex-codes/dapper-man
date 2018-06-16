using System;
using System.Runtime.Caching;
using DapperMan.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class PropertyCacheTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertyCache_Ctor_Throws_When_Cache_Null()
        {
            var cache = new PropertyCache(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertyCache_Ctor_Throws_When_Policy_Null()
        {
            var cache = new PropertyCache(new MemoryCache("test"), null);
        }

        [TestMethod]
        public void PropertyCache_Ctor_Uses_Default_Policy_When_Null()
        {
            var cache = new PropertyCache(new MemoryCache("test"));
            Assert.AreEqual(cache.Policy.SlidingExpiration, PropertyCache.DefaultCachePolicy.SlidingExpiration);
        }
    }
}

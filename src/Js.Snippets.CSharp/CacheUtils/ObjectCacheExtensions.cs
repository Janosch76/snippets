namespace Js.Snippets.CSharp.CacheUtils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Caching;
    using System.Text;
    using System.Threading.Tasks;
    using Js.Snippets.CSharp.AsyncUtils;

    /// <summary>
    /// Extension methods
    /// </summary>
    public static class ObjectCacheExtensions
    {
        /// <summary>
        /// inserts a lazily initialized cache entry into the cache, specifying a key and a value for the cache entry,
        /// and information about how the entry will be evicted.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="cache">The cache.</param>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="valueFactory">The value factory method.</param>
        /// <param name="policy">An object that contains eviction details for the cache entry.</param>
        /// <returns>The cached value.</returns>
        public static T AddOrGetExisting<T>(this ObjectCache cache, string key, Func<T> valueFactory, CacheItemPolicy policy)
        {
            var newValue = new Lazy<T>(valueFactory);
            var oldValue = cache.AddOrGetExisting(key, newValue, policy) as Lazy<T>;

            try
            {
                return (oldValue ?? newValue).Value;
            }
            catch
            {
                cache.Remove(key);
                throw;
            }
        }

        /// <summary>
        /// inserts a lazily, asynchronously initialized cache entry into the cache, specifying a key and a value for the cache entry,
        /// and information about how the entry will be evicted.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="cache">The cache.</param>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <param name="valueFactory">The value factory method.</param>
        /// <param name="policy">An object that contains eviction details for the cache entry.</param>
        /// <returns>The cached value.</returns>
        public static async Task<T> AddOrGetExistingAsync<T>(this ObjectCache cache, string key, Func<Task<T>> valueFactory, CacheItemPolicy policy)
        {
            var newValue = new AsyncLazy<T>(valueFactory);
            var oldValue = cache.AddOrGetExisting(key, newValue, policy) as Lazy<T>;

            try
            {
                return oldValue != null ? oldValue.Value : await newValue.Value;
            }
            catch
            {
                cache.Remove(key);
                throw;
            }
        }
    }
}

﻿namespace Js.Snippets.CSharp.CollectionUtils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extension methods
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Splits the source collection into batches of the specified batch size.
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <returns>A collection of batches of the given size taken from the source.</returns>
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
        {
            return source
                .Select((v, i) => new { Index = i, Value = v })
                .GroupBy(item => item.Index / batchSize)
                .Select(g => g.Select(item => item.Value));
        }

        /// <summary>
        /// Splits the source collection into buckets according to specified split points.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="splitPoints">The split points.</param>
        /// <returns>The source collection split into buckets according to the specified split points.</returns>
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, params T[] splitPoints)
             where T : IComparable<T>
        {
            return source.Split(v => v, splitPoints.ToList());
        }

        /// <summary>
        /// Splits the source collection into buckets according to specified split points.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="selector">The key selector.</param>
        /// <param name="splitPoints">The split points.</param>
        /// <returns>The source collection split into buckets according to the specified split points.</returns>
        public static IEnumerable<IEnumerable<T>> Split<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector, IList<TKey> splitPoints)
            where TKey : IComparable<TKey>
        {
            return source
                .Select(v => new { Bucket = splitPoints.Where(p => p.CompareTo(selector(v)) < 0).Count(), Value = v })
                .GroupBy(item => item.Bucket)
                .Select(g => g.Select(item => item.Value));
        }
    }
}

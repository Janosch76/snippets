namespace Js.Snippets.CSharp.CollectionUtils
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
        /// Determines the unique items from the given collection, according to the specified key selector.
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns>The unique items from the given collection.</returns>
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            var knownKeys = new HashSet<TKey>();
            foreach (var item in source)
            {
                if (knownKeys.Add(keySelector(item)))
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Splits the source collection into batches of the specified batch size.
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <returns>A collection of batches of the given size taken from the source.</returns>
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

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
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source
                .Select(v => new { Bucket = splitPoints.Where(p => p.CompareTo(selector(v)) < 0).Count(), Value = v })
                .GroupBy(item => item.Bucket)
                .Select(g => g.Select(item => item.Value));
        }

        /// <summary>
        /// Splits a collection into segments of equal values
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The segmented source.</returns>
        public static IEnumerable<IEnumerable<int>> Segments(this IEnumerable<int> source)
        {
            return source.Segments(v => v);
        }

        /// <summary>
        /// Splits a collection into segments of equal values
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="selector">The key selector.</param>
        /// <returns>The segmented source.</returns>
        public static IEnumerable<IEnumerable<T>> Segments<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector)
            where TKey : IEquatable<TKey>
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            while (source.Any())
            {
                var current = source.First();
                var segment = source.TakeWhile(v => selector(v).Equals(selector(current)));
                source = source.Skip(segment.Count());
                yield return segment;
            }
        }

        /// <summary>
        /// Computes the standard deviation of a series of values
        /// </summary>
        /// <param name="source">The values</param>
        /// <returns>The standard deviation</returns>
        public static double StdDev(this IEnumerable<double> source)
        {
            return source.StdDev(v => v);
        }

        /// <summary>
        /// Computes the standard deviation of a series of values
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="source">The values</param>
        /// <param name="selector">A projection function</param>
        /// <returns>The standard deviation</returns>
        /// <seealso cref="https://stackoverflow.com/a/2878000"/>
        public static double StdDev<T>(this IEnumerable<T> source, Func<T, double> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (!source.Any())
            {
                return 0.0;
            }

            double mean = 0.0;
            double sum = 0.0;
            int n = 0;
            foreach (var s in source)
            {
                n++;
                var value = selector(s);
                double delta = value - mean;
                mean += delta / n;
                sum += delta * (value - mean);
            }

            return Math.Sqrt(sum / n);
        }
    }
}

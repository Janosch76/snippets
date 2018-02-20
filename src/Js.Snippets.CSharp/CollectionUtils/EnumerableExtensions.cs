namespace Js.Snippets.CSharp.CollectionUtils
{
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
        public static IEnumerable<List<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
        {
            return source
                .Select((v, i) => new { Index = i, Value = v })
                .GroupBy(item => item.Index / batchSize)
                .Select(g => g.Select(item => item.Value).ToList());
        }
    }
}

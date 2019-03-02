namespace Js.Snippets.CSharp.CollectionUtils
{
    using System.Collections.Generic;

    /// <summary>
    /// Extension methods for arrays
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Slices a row from the given array.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="row">The row.</param>
        /// <returns>The specified row from the array.</returns>
        public static IEnumerable<T> Row<T>(this T[,] array, int row)
        {
            for (var i = array.GetLowerBound(1); i <= array.GetUpperBound(1); i++)
            {
                yield return array[row, i];
            }
        }

        /// <summary>
        /// Slices a column from the given array.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="column">The column.</param>
        /// <returns>The specified column from the array.</returns>
        public static IEnumerable<T> Column<T>(this T[,] array, int column)
        {
            for (var i = array.GetLowerBound(0); i <= array.GetUpperBound(0); i++)
            {
                yield return array[i, column];
            }
        }
    }
}

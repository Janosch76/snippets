namespace Js.Snippets.CSharp.IComparable
{
    using System;

    /// <summary>
    /// Extension methods on types implementing <see cref="IComparable{T}"/>
    /// </summary>
    public static class ComparableExtensions
    {
        /// <summary>
        /// Determines whether the value is between the specified lower and upper bounds (inclusive).
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="lower">The lower bound.</param>
        /// <param name="upper">The upper bound.</param>
        /// <returns>
        ///   <c>true</c> if the value is between the specified lower and upper bounds; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBetween<T>(this T value, T lower, T upper) where T : IComparable<T>
        {
            return value.CompareTo(lower) >= 0 && value.CompareTo(upper) <= 0;
        }
    }
}
namespace Js.Snippets.CSharp.EnumUtils
{
    using System;
    using System.Linq;

    /// <summary>
    /// Extension methods
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Determines whether a sequence contains a specified element 
        /// by using the default equality comparer.
        /// </summary>
        /// <typeparam name="T">An enumeration type.</typeparam>
        /// <param name="value">The value to find.</param>
        /// <param name="values">A sequence in which to locate a value.</param>
        /// <returns>True if the sequence contains an element that has the specified value; otherwise, false.</returns>
        public static bool In<T>(this T value, params T[] values)
            where T : struct
        {
            return values.Contains(value);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one 
        /// or more enumerated constants to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="T">An enumeration type.</typeparam>
        /// <param name="value">The string containing the name or value to convert.</param>
        /// <param name="ignoreCase">true to ignore case; false to regard case.</param>
        /// <returns>An object of type <typeparamref name="T"/> whose value is represented by given string.</returns>
        public static T Parse<T>(string value, bool ignoreCase = false)
            where T : struct
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }
    }
}

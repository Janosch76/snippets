namespace Js.Snippets.CSharp.StringUtils
{
    using System;
    using System.Linq;

    /// <summary>
    /// Extension methods
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Prefix of a string with a specified length.
        /// </summary>
        /// <param name="value">The string.</param>
        /// <param name="length">The length of the prefix.</param>
        /// <returns>The prefix with the specified length.</returns>
        public static string Left(this string value, int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Length cannot be negative.");
            }

            if (length >= value.Length)
            {
                return value;
            }

            return value.Substring(0, length);
        }

        /// <summary>
        /// Suffix of a string with a specified length.
        /// </summary>
        /// <param name="value">The string.</param>
        /// <param name="length">The length of the suffix.</param>
        /// <returns>The suffix with the specified length.</returns>
        public static string Right(this string value, int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Length cannot be negative.");
            }

            if (length >= value.Length)
            {
                return value;
            }

            return value.Substring(value.Length - length, length);
        }

        /// <summary>
        /// Determines whether a sequence contains a specified element 
        /// by using the default equality comparer.
        /// </summary>
        /// <param name="value">The value to find.</param>
        /// <param name="values">A sequence in which to locate a value.</param>
        /// <returns>True if the sequence contains an element that has the specified value; otherwise, false.</returns>
        public static bool In(this string value, params string[] values)
        {
            return values.Contains(value);
        }

        /// <summary>
        /// Converts this string representation of the name or numeric value of one 
        /// or more enumerated constants to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="T">An enumeration type.</typeparam>
        /// <param name="value">The string containing the name or value to convert.</param>
        /// <param name="ignoreCase">true to ignore case; false to regard case.</param>
        /// <returns>An object of type <typeparamref name="T"/> whose value is represented by this instance.</returns>
        public static T ToEnum<T>(this string value, bool ignoreCase = false)
            where T : struct
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        /// <summary>
        /// Replaces the tokens in this instance, using the public properties of 
        /// one or more objects as token value providers. For each token reference,
        /// an optional format string can be used to control the formatting of the 
        /// token value.
        /// </summary>
        /// <param name="template">The template in which to replace tokens.</param>
        /// <param name="tokenValueProviders">The token value providers.</param>
        /// <returns>
        /// The template string where all tokens have been replaced
        /// by their respective values.
        /// </returns>
        public static string ReplaceTokens(this string template, params object[] tokenValueProviders)
        {
            return TokenReplacer.ReplaceTokens(template, tokenValueProviders);
        }
    }
}

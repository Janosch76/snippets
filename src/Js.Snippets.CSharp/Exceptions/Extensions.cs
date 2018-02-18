namespace Js.Snippets.CSharp.Exceptions
{
    using System;

    /// <summary>
    /// Extension methods
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Throws an ArgumentNullException if the given data item is null.
        /// No parameter name is specified.
        /// </summary>
        /// <param name="obj">The item to check for nullity.</param>
        public static void ThrowIfNull<T>(this T obj) where T : class
        {
            if (obj == null)
            {
                throw new ArgumentNullException();
            }
        }

        /// <summary>
        /// Throws an ArgumentNullException if the given data item is null.
        /// </summary>
        /// <param name="obj">The item to check for nullity.</param>
        /// <param name="name">The name to use when throwing an exception, if necessary</param>
        public static void ThrowIfNull<T>(this T obj, string name) where T : class
        {
            if (obj == null)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}

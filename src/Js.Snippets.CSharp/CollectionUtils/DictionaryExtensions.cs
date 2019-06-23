namespace Js.Snippets.CSharp.CollectionUtils
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Extension methods
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Creates a query string for a URL out of the key value-pairs in the given collection.
        /// </summary>
        /// <param name="source">The key value pairs</param>
        /// <returns>A URL query string</returns>
        public static string ToQueryString(this IDictionary<string, string> source)
        {
            return string.Join(
                "&", 
                source.Keys.Select(key => $"{HttpUtility.UrlEncode(key)}={HttpUtility.UrlEncode(source[key])}"));
        }
    }
}

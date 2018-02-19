namespace Js.Snippets.CSharp.DateUtils
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// <see cref="TimeSpan"/> based extension methods 
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Formats the date in ISO format.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>The ISO format string representation of the date.</returns>
        public static string ToDateIso(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Formats the timestamp in ISO format.
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <returns>The ISO format string representation of the timestamp.</returns>
        public static string ToDateTimeIso(this DateTime timestamp)
        {
            return timestamp.ToString("o");
        }
    }
}

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

        /// <summary>
        /// Determines whether the date/time is in the specified range (inclusive).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="start">The start time of the range.</param>
        /// <param name="end">The end time of the range.</param>
        /// <returns>
        ///   <c>true</c> if the value is between the specified lower and upper bounds; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBetween(this DateTime value, DateTime start, DateTime end)
        {
            return value.Ticks >= start.Ticks && value.Ticks <= end.Ticks;
        }
    }
}

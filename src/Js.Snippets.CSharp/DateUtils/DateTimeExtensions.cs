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

        /// <summary>
        /// Determines if the date is a working day (disregarding holidays...).
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>
        ///   <c>false</c> if the specified date is weekend; otherwise, <c>true</c>.
        /// </returns>
        public static bool IsWorkingDay(this DateTime date)
        {
            return !date.IsWeekend();
        }

        /// <summary>
        /// Determines if the date is a weekend.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>
        ///   <c>true</c> if the specified date is weekend; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWeekend(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Gets the next workday after today (disregarding holidays...).
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>The next workday.</returns>
        public static DateTime NextWorkday(this DateTime date)
        {
            while (!date.IsWorkingDay())
            {
                date = date.AddDays(1);
            }

            return date;
        }

        /// <summary>
        /// Determine the next date of a given day of the week. For example, from this date, when is the next Tuesday?
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="dayOfWeek">The day of the week to find.</param>
        /// <returns>The date of the next given day of week.</returns>
        public static DateTime Next(this DateTime date, DayOfWeek dayOfWeek)
        {
            while (date.DayOfWeek != dayOfWeek)
            {
                date = date.AddDays(1);
            }

            return date;
        }
    }
}

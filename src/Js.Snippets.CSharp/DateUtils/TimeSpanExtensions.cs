namespace Js.Snippets.CSharp.DateUtils
{
    using System;

    /// <summary>
    /// <see cref="TimeSpan"/> based extension methods.
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Returns a <see cref="TimeSpan"/> representing the specified number of milliseconds.
        /// </summary>
        /// <param name="milliseconds">The number of milliseconds.</param>
        /// <returns>A <see cref="TimeSpan"/> representing the specified number of milliseconds.</returns>
        public static TimeSpan Milliseconds(this int milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> representing the specified number of seconds.
        /// </summary>
        /// <param name="seconds">The number of seconds.</param>
        /// <returns>A <see cref="TimeSpan"/> representing the specified number of seconds.</returns>
        public static TimeSpan Seconds(this int seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> representing the specified number of minutes.
        /// </summary>
        /// <param name="minutes">The number of minutes.</param>
        /// <returns>A <see cref="TimeSpan"/> representing the specified number of minutes.</returns>
        public static TimeSpan Minutes(this int minutes)
        {
            return TimeSpan.FromMinutes(minutes);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> representing the specified number of hours.
        /// </summary>
        /// <param name="hours">The number of hours.</param>
        /// <returns>A <see cref="TimeSpan"/> representing the specified number of hours.</returns>
        public static TimeSpan Hours(this int hours)
        {
            return TimeSpan.FromHours(hours);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> representing the specified number of days.
        /// </summary>
        /// <param name="days">The number of days.</param>
        /// <returns>A <see cref="TimeSpan"/> representing the specified number of days.</returns>
        public static TimeSpan Days(this int days)
        {
            return TimeSpan.FromDays(days);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> representing the specified number of weeks.
        /// </summary>
        /// <param name="weeks">The number of weeks.</param>
        /// <returns>A <see cref="TimeSpan"/> representing the specified number of weeks.</returns>
        public static TimeSpan Weeks(this int weeks)
        {
            return (weeks * 7).Days();
        }
    }
}

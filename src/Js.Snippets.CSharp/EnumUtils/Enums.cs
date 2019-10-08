namespace Js.Snippets.CSharp.EnumUtils
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// Convenience methods for enumeration types.
    /// </summary>
    /// <typeparam name="TClass">The enumeration type</typeparam>
    /// <see cref="https://stackoverflow.com/a/28527552"/>
    public abstract class ConstrainedEnumParser<TClass>
        where TClass : class
    {
        /// <summary>
        /// Converts the string representation of the name or numeric value of one 
        /// or more enumerated constants to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="TEnum">An enumeration type.</typeparam>
        /// <param name="value">The string containing the name or value to convert.</param>
        /// <param name="ignoreCase">true to ignore case; false to regard case.</param>
        /// <returns>An object of type <typeparamref name="TEnum"/> whose value is represented by given string.</returns>
        public static TEnum Parse<TEnum>(string value, bool ignoreCase = false)
            where TEnum : struct, TClass
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more
        /// enumerated constants to an equivalent enumerated object. A parameter specifies
        /// whether the operation is case-sensitive. The return value indicates whether the
        /// conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">An enumeration type.</typeparam>
        /// <param name="value">The string to parse</param>
        /// <param name="result">The enumeratino value corresponding to the given string value.</param>
        /// <param name="ignoreCase">true to ignore case; false to regard case.</param>
        /// <param name="defaultValue">a default value to use in case of failed parsing</param>
        /// <returns>A value indicates whether the conversion succeeded</returns>
        public static bool TryParse<TEnum>(string value, out TEnum result, bool ignoreCase = false, TEnum defaultValue = default(TEnum)) 
            where TEnum : struct, TClass 
        {
            var success = Enum.TryParse(value, ignoreCase, out result);
            if (!success)
            {
                result = defaultValue;
            }

            return success;
        }

        public static IList<TEnum> ToList<TEnum>()
            where TEnum : struct
        {
            var type = typeof(TEnum);
            if (!type.IsEnum)
            {
                return new List<TEnum>();
            }

            return Enum.GetValues(type).Cast<TEnum>().ToList();
        }

        public static IDictionary<TEnum, string> ToDictionary<TEnum>()
            where TEnum : struct
        {
            var type = typeof(TEnum);
            if (!type.IsEnum)
            {
                return new Dictionary<TEnum, string>();
            }

            var result = new Dictionary<TEnum, string>();
            var values = Enum.GetValues(type).Cast<TEnum>();
            foreach (var value in values)
            {
                var memInfo = type.GetMember(type.GetEnumName(value));
                var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                var description = ((DescriptionAttribute)descriptionAttributes.FirstOrDefault())?.Description ?? value.ToString();
                result.Add(value, description);
            }

            return result;
        }
    }

    /// <summary>
    /// Convenience methods for enumeration types.
    /// </summary>
    public class Enums : ConstrainedEnumParser<System.Enum>
    {
    }
}
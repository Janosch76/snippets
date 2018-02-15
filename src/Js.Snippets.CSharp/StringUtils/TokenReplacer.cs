namespace Js.Snippets.CSharp.StringUtils
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Token replacement
    /// </summary>
    public class TokenReplacer
    {
        /// <summary>
        /// The default left delimiter
        /// </summary>
        public const char DEFAULT_LEFT_DELIMITER = '{';

        /// <summary>
        /// The default right delimiter
        /// </summary>
        public const char DEFAULT_RIGHT_DELIMITER = '}';

        private readonly object[] tokenValueProviders;
        private readonly bool throwOnTokenValueNotFound;
        private readonly char leftDelimiter;
        private readonly char rightDelimiter;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenReplacer"/> class.
        /// </summary>
        /// <param name="tokenValueProviders">The token value providers.</param>
        public TokenReplacer(IEnumerable<object> tokenValueProviders)
            : this(tokenValueProviders, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenReplacer"/> class.
        /// </summary>
        /// <param name="tokenValueProviders">The token value providers.</param>
        /// <param name="throwOnTokenValueNotFound">if set to <c>true</c> throws an exception when a token value is not found.</param>
        public TokenReplacer(IEnumerable<object> tokenValueProviders, bool throwOnTokenValueNotFound)
            : this(tokenValueProviders, throwOnTokenValueNotFound, TokenReplacer.DEFAULT_LEFT_DELIMITER, TokenReplacer.DEFAULT_RIGHT_DELIMITER)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenReplacer"/> class.
        /// </summary>
        /// <param name="tokenValueProviders">The token value providers.</param>
        /// <param name="throwOnTokenValueNotFound">if set to <c>true</c> throws an exception when a token value is not found.</param>
        /// <param name="leftDelimiter">The left delimiter.</param>
        /// <param name="rightDelimiter">The right delimiter.</param>
        public TokenReplacer(IEnumerable<object> tokenValueProviders, bool throwOnTokenValueNotFound, char leftDelimiter, char rightDelimiter)
        {
            this.tokenValueProviders = tokenValueProviders.ToArray();
            this.throwOnTokenValueNotFound = throwOnTokenValueNotFound;
            this.leftDelimiter = leftDelimiter;
            this.rightDelimiter = rightDelimiter;
        }

        private string LDelim
        {
            get { return Regex.Escape(this.leftDelimiter.ToString()); }
        }

        private string RDelim
        {
            get { return Regex.Escape(this.rightDelimiter.ToString()); }
        }

        /// <summary>
        /// Replaces the tokens in a given template string by the values found in a token value provider object(s).
        /// </summary>
        /// <param name="template">The template string.</param>
        /// <param name="tokenValueProviders">The token value providers.</param>
        /// <returns>The template string with tokens replaced by the respective values.</returns>
        public static string ReplaceTokens(string template, params object[] tokenValueProviders)
        {
            var tokenReplacer = new TokenReplacer(tokenValueProviders);
            return tokenReplacer.ReplaceTokens(template);
        }

        /// <summary>
        /// Replaces the tokens in a given template string by the values found in a token value provider object(s).
        /// </summary>
        /// <param name="template">The template string.</param>
        /// <param name="throwOnTokenValueNotFound">A value indicating whether an exception is thrown when a token value is not provided.</param>
        /// <param name="tokenValueProviders">The token value providers.</param>
        /// <returns>The template string with tokens replaced by the respective values.</returns>
        public static string ReplaceTokens(string template, bool throwOnTokenValueNotFound, params object[] tokenValueProviders)
        {
            var tokenReplacer = new TokenReplacer(tokenValueProviders, throwOnTokenValueNotFound);
            return tokenReplacer.ReplaceTokens(template);
        }

        /// <summary>
        /// Replaces the tokens in a given template string by the values found in the token value provider object(s) of this instance.
        /// </summary>
        /// <param name="template">The template string.</param>
        /// <returns>The template string with tokens replaced by the respective values.</returns>
        public string ReplaceTokens(string template)
        {
            var tokenPattern = $@"{LDelim}(?<tokenref>\S[^{RDelim}]*){RDelim}";
            foreach (Match token in Regex.Matches(template, tokenPattern))
            {
                var tokenRef = TokenRef.Parse(token.Groups["tokenref"].Value);
                var tokenValue = FindTokenValue(tokenRef.TokenName);
                if (tokenValue == null)
                {
                    continue;
                }

                if (tokenRef.IsIndexedValue)
                {
                    tokenValue = GetIndexedValue(tokenValue, tokenRef.Index);
                }

                var formattedTokenValue = string.Format(tokenRef.Format, tokenValue);
                template = template.Replace(token.Value, formattedTokenValue);
            }

            return template;
        }

        private object GetIndexedValue(object tokenValue, string index)
        {
            if (tokenValue is IList)
            {
                var values = tokenValue as IList;
                int i = 0;
                if (!int.TryParse(index, out i))
                {
                    throw new FormatException($"Invalid index expression. Expected a numeric index expression, but actual expression is '{index}'.");
                }

                if (!(0 <= i && i < values.Count))
                {
                    throw new IndexOutOfRangeException($"Index out of range: '{index}'.");
                }

                return values[i];
            }

            if (tokenValue is IDictionary)
            {
                var values = tokenValue as IDictionary;
                if (!values.Contains(index))
                {
                    throw new IndexOutOfRangeException($"Key '{index}' does not exist in the collection.");
                }

                return values[index];
            }

            throw new ArgumentException("Token value is not a collection.");
        }

        private object FindTokenValue(string tokenName)
        {
            foreach (var tokenValueProvider in this.tokenValueProviders)
            {
                var t = tokenValueProvider.GetType();
                var prop = t.GetProperty(tokenName);
                if (prop != null)
                {
                    return prop.GetValue(tokenValueProvider);
                }
            }

            if (this.throwOnTokenValueNotFound)
            {
                throw new Exception($"Token value for '{tokenName}' not found.");
            }
            else
            {
                return null;
            }
        }

        private class TokenRef
        {
            private TokenRef(string tokenName, string index, string formatString)
            {
                TokenName = tokenName;
                Index = index;
                Format = string.IsNullOrEmpty(formatString) ? "{0}" : $"{{0:{formatString}}}";
            }

            public string TokenName { get; private set; }

            public string Index { get; private set; }

            public string Format { get; private set; }

            public bool IsIndexedValue
            {
                get { return !string.IsNullOrEmpty(Index); }
            }

            public static TokenRef Parse(string tokenRef)
            {
                var match = Regex.Match(tokenRef, @"(?<name>[^\s\[\:]+)(\[(?<index>[^\s\]]+)\])?(\s*\:\s*(?<format>\S.*))?");
                if (!match.Success)
                {
                    throw new FormatException("Invalid token format.");
                }

                return new TokenRef(
                    match.Groups["name"].Value,
                    match.Groups["index"].Value,
                    match.Groups["format"].Value);
            }
        }
    }
}
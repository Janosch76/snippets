namespace Js.Snippets.CSharp.StringUtils
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    public class TokenReplacer
    {
        public const char DEFAULT_LEFT_DELIMITER = '{';
        public const char DEFAULT_RIGHT_DELIMITER = '}';

        private readonly char leftDelimiter;
        private readonly char rightDelimiter;
        private readonly object[] tokenValueProviders;

        public TokenReplacer(IEnumerable<object> tokenValueProviders)
            : this(tokenValueProviders, TokenReplacer.DEFAULT_LEFT_DELIMITER, TokenReplacer.DEFAULT_RIGHT_DELIMITER)
        {
        }

        public TokenReplacer(IEnumerable<object> tokenValueProviders, char leftDelimiter, char rightDelimiter)
        {
            this.tokenValueProviders = tokenValueProviders.ToArray();
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

        public static string ReplaceTokens(string template, params object[] tokenValueProviders)
        {
            var tokenReplacer = new TokenReplacer(tokenValueProviders);
            return tokenReplacer.ReplaceTokens(template);
        }

        public string ReplaceTokens(string template)
        {
            var tokenPattern = $@"{LDelim}(?<tokenref>\S[^{RDelim}]*){RDelim}";
            foreach (Match token in Regex.Matches(template, tokenPattern))
            {
                var tokenRef = TokenRef.Parse(token.Groups["tokenref"].Value);
                var tokenValue = FindTokenValue(tokenRef.TokenName);
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

            throw new Exception($"Token value for '{tokenName}' not found.");
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
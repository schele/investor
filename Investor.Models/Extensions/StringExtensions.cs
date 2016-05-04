using System.Text.RegularExpressions;
using Lucene.Net.QueryParsers;

namespace Investor.Models.Extensions
{
    public static class StringExtensions
    {
        public static string ToSafeLucineValue(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            if (value[0] == '*' || value[0] == '?')
            {
                value = value.Substring(1);
            }

            if (!string.IsNullOrEmpty(value))
            {
                if (value[0] == '*' || value[0] == '?')
                {
                    value = Regex.Replace(value, @"[^A-Za-z0-9\s+_\-]", "");
                }
            }

            return QueryParser.Escape(value);
        }
    }
}

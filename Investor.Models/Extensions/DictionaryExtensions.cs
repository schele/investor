using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Investor.Models.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Converts a dictionary object to a query string representation such as:
        /// firstname=shannon&lastname=deminick
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ToQueryString(this IDictionary<string, object> d)
        {
            if (!d.Any()) return "";

            var builder = new StringBuilder();
            foreach (var i in d)
            {
                builder.Append(String.Format("{0}={1}&", HttpUtility.UrlEncode(i.Key), i.Value == null ? string.Empty : HttpUtility.UrlEncode(i.Value.ToString())));
            }
            return builder.ToString().TrimEnd('&');
        }
    }
}

﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Castle.Core.Internal;
using Lucene.Net.QueryParsers;

namespace Investor.Models.Extensions
{
    public static class StringExtensions
    {
        //const string Hellip = "&hellip;";

        public static string GetWords(this string input, int count)
        {
            var words = input.Split(' ').Take(count);

            var sb = new StringBuilder();

            foreach (var word in words)
            {
                sb.Append(word + " ");
            }

            return sb + "...";
        }


        //public static IEnumerable<string> GetWords(this string input, int limit)
        //{
        //    var matches = Regex.Matches(input, @"\b[\w']*\b");

        //    var words = from m in matches.Cast<Match>()
        //                where !string.IsNullOrEmpty(m.Value)
        //                select TrimSuffix(m.Value);

        //    return words.Take(limit);
        //}

        public static IEnumerable<string> GetWords(this string input)
        {
            var matches = Regex.Matches(input, @"\b[\w']*\b");

            var words = from m in matches.Cast<Match>()
                        where !string.IsNullOrEmpty(m.Value)
                        select TrimSuffix(m.Value);

            return words;
        }

        //public static string TruncateWords(this string input, int maxLength, string suffix = Hellip)
        //{
        //    if (string.IsNullOrEmpty(input))
        //    {
        //        return input;
        //    }

        //    // Clean the input of any html
        //    input = input.StripHtml();

        //    if (maxLength <= 0)
        //    {
        //        return input;
        //    }

        //    var suffixLength = suffix == Hellip ? 3 : suffix.Length;
        //    int strLength = maxLength - suffixLength;
        //    if (strLength <= 0)
        //    {
        //        return input;
        //    }

        //    if (input.Length <= maxLength)
        //    {
        //        return input;
        //    }

        //    var wordLength = 0;
        //    var words = GetWords(input, maxLength).TakeWhile(x =>
        //    {
        //        // Check if word + suffix is not greater then current word length
        //        if ((x.Length + suffixLength + wordLength) > maxLength)
        //        {
        //            return false;
        //        }

        //        // Incrise the word length.
        //        wordLength = wordLength + x.Length;

        //        return true;
        //    });

        //    return string.Join(" ", words) + suffix;
        //}

        /// <summary>
        /// Strips all html from a string.
        /// 
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>
        /// Returns the string without any html tags.
        /// </returns>
        public static string StripHtml(this string text)
        {
            return Regex.Replace(text, "<(.|\\n)*?>", string.Empty);
        }

        private static string TrimSuffix(string word)
        {
            int apostrapheLocation = word.IndexOf('\'');
            if (apostrapheLocation != -1)
            {
                word = word.Substring(0, apostrapheLocation);
            }

            return word;
        }

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

        public static string GetAbsoluteUrl(string url)
        {
            var absoluteUrl = "";

            if (!url.IsNullOrEmpty())
            {
                absoluteUrl = "https://" + HttpContext.Current.Request.Url.Host + url;
            }
            
            return absoluteUrl;
        }
    }
}

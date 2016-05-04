using System;
using Investor.SearchTools.Abstraction;

namespace Investor.SearchTools.Behaviors.FormatFields
{
    public class BodyTextFormater : IFormatField
    {
        public string FieldAlias { get { return "bodyText"; } }

        public string FormatAlias()
        {
            return FieldAlias;
        }

        public string FormatField(string value)
        {
            return value.Substring(0, Math.Min(value.Length, 200));
        }
    }
}

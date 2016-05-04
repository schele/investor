using System;
using System.Collections.Generic;
using System.Linq;
using Investor.SearchTools.Abstraction;

namespace Investor.SearchTools
{
    public abstract class SearchToolsBase
    {
        protected IList<IFormatField> FormatFields { get; private set; }

        protected ISearchBehavior SearchBehavior { get; set; }

        protected SearchToolsBase()
        {
            FormatFields = new List<IFormatField>();
        }

        protected SearchToolsBase AddFieldFormater(IFormatField formatField)
        {
            FormatFields.Add(formatField);

            return this;
        }

        public KeyValuePair<string, string> FormatField(string field, string value)
        {
            // if field contains * do not format
            if (field.Contains("*"))
            {
                return new KeyValuePair<string, string>(field, value);
            }

            var formatField = FormatFields.FirstOrDefault(x => x.FieldAlias.Equals(field));
            
            if (formatField == null)
            {
                return new KeyValuePair<string, string>(field, value);
            }

            return new KeyValuePair<string, string>(formatField.FormatAlias(), formatField.FormatField(value));
        }

        public virtual IList<string> Search(string term)
        {
            if (SearchBehavior == null)
            {
                throw new Exception("No ISearchBehavior set!");
            }

            return SearchBehavior.Search(term);
        }

        public virtual void AddToBuffert(string item)
        {
            SearchBehavior.Buffert.Add(item);
        }

        public virtual void ClearBuffert()
        {
            SearchBehavior.Buffert.Clear();
        }
    }
}

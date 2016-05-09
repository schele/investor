using System.Collections.Generic;
using Examine;
using Investor.UmbExamine.Abstraction;
using Umbraco.Web;

namespace Investor.UmbExamine.Model
{
    public abstract class ResultBase : Dictionary<string, object>, IModelResult, IModelResultFormatter
    {
        public UmbracoHelper Umbraco { get; private set; }

        protected ResultBase(UmbracoHelper helper)
        {
            Umbraco = helper;
        }

        public abstract void Format(SearchResult result);

        public void AddProperty(string name, object value)
        {
            if (ContainsKey(name))
            {
                this[name] = value;

                return;
            }

            Add(name, value);
        }

        public void RemoveProperty(params string[] names)
        {
            foreach (var name in names)
            {
                if (ContainsKey(name))
                {
                    Remove(name);
                }
            }
        }
    }
}
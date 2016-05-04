using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Investor.Models.Extensions
{
    public static class ObjectExtensions
    {
        public static bool TryConvertTo<T>(this object input, out T result)
        {
            var tryConvert = Umbraco.Core.ObjectExtensions.TryConvertTo<T>(input);

            if (tryConvert.Success)
            {
                result = tryConvert.Result;

                return true;
            }

            result = default(T);

            return false;
        }

        public static T TryConvertTo<T>(this object input, T defaultValue)
        {
            T result;

            if (TryConvertTo(input, out result))
            {
                return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// Turns object into dictionary
        /// </summary>
        /// <param name="o"></param>
        /// <param name="ignoreProperties">Properties to ignore</param>
        /// <returns></returns>
        public static IDictionary<string, TVal> ToDictionary<TVal>(this object o, params string[] ignoreProperties)
        {
            if (o != null)
            {
                var props = TypeDescriptor.GetProperties(o);
                var d = new Dictionary<string, TVal>();
                foreach (var prop in props.Cast<PropertyDescriptor>().Where(x => !ignoreProperties.Contains(x.Name)))
                {
                    var val = prop.GetValue(o);
                    if (val != null)
                    {
                        d.Add(prop.Name, (TVal)val);
                    }
                }
                return d;
            }
            return new Dictionary<string, TVal>();
        }
    }
}

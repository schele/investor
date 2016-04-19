using System.Reflection;
using UCodeFirst.ContentTypes;
using UCodeFirst.Converters;
using UCodeFirst.Extensions;
using Umbraco.Core;

namespace Investor.Models.Converters
{
    public class ModelConverter<T> : IConverter where T : ContentTypeBase
    {
        public object Read(PropertyInfo propertyInfo, object value)
        {
            if (string.IsNullOrEmpty(value.ToString()))
            {
                return null;
            }

            var content = ApplicationContext.Current.Services.ContentService.GetById(int.Parse(value.ToString()));
            var model = content.ToModel<T>();

            return model;
        }
    }
}
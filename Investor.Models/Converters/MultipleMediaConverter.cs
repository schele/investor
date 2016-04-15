using System.Collections.Generic;
using System.Reflection;
using UCodeFirst.Converters;
using Umbraco.Core.Models;

namespace Investor.Models.Converters
{
    public class MultipleMediaConverter : IConverter
    {
        public object Read(PropertyInfo propertyInfo, object value)
        {
            //if (!string.IsNullOrEmpty(value))
            //{
            //    var images = new List<IMedia>();
            //    var imageIds = value.Split(',').Select(sValue => sValue.Trim()).ToArray();

            //    foreach (var id in imageIds)
            //    {
            //        var image = MediaExtensions.GetMediaById(id);

            //        images.Add(image);
            //    }

            //    return images;
            //}

            return new List<IMedia>();
        }

    }
}
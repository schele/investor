using Umbraco.Core.Models;

namespace Investor.Extensions
{
    public static class PublishedContent
    {
        public static string GetSize(this IPublishedContent publishedContent)
        {
            var size = publishedContent.GetProperty("umbracoBytes").Value.ToString();

            return size;
        }
    }
}
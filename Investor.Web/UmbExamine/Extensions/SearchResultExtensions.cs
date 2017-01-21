using System;
using Examine;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Investor.UmbExamine.Extensions
{
    public static class SearchResultExtensions
    {
        public static IPublishedContent TryGetContentOrMedia(this SearchResult result, UmbracoHelper helper)
        {
            var id = result.Id;
            var tryContent = helper.TypedContent(id);

            if (tryContent != null)
            {
                return tryContent;
            }

            var tryMedia = helper.TypedMedia(id);

            if (tryMedia != null)
            {
                return tryMedia;
            }

            return null;
        }

        public static ContentType GetContentType(this SearchResult result, string nodeTypeAlias)
        {
            if (string.IsNullOrEmpty(nodeTypeAlias))
            {
                return ContentType.None;
            }

            nodeTypeAlias = nodeTypeAlias.ToLower();

            switch (nodeTypeAlias)
            {
                case "image":
                    return ContentType.Media;
                case "file":
                    return ContentType.File;
                default:
                    return ContentType.Content;
            }
        }

        public static bool IsContent(this SearchResult result, string nodeTypeAlias)
        {
            if (GetContentType(result, nodeTypeAlias) == ContentType.Content)
            {
                return true;
            }

            return false;
        }

        public static bool IsImage(this SearchResult result, string nodeTypeAlias)
        {
            if (GetContentType(result, nodeTypeAlias) == ContentType.Media)
            {
                return true;
            }

            return false;
        }

        public static bool IsFile(this SearchResult result, string nodeTypeAlias)
        {
            if (GetContentType(result, nodeTypeAlias) == ContentType.File)
            {
                return true;
            }

            return false;
        }

        public static bool IsFieldEqual(this SearchResult result, string key, string value, StringComparison comparison)
        {
            string nodeTypeAlias;

            if (result.Fields.TryGetValue(key, out nodeTypeAlias))
            {
                if (nodeTypeAlias.Equals(value, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
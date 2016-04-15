using Umbraco.Core;
using Umbraco.Core.Models;

namespace Investor.Models.Extensions
{
    public static class MediaExtensions
    {
        public static IMedia GetMediaById(string mediaId)
        {
            var mediaService = ApplicationContext.Current.Services.MediaService;
            int parsedId;

            if (int.TryParse(mediaId, out parsedId))
            {
                return mediaService.GetById(parsedId);
            }

            return null;
        }

        public static string FriendlyUrl(this IMedia media)
        {
            return media != null ? media.Url() : string.Empty;
        }

        public static string Url(this IMedia currentMedia)
        {
            return currentMedia.GetValue<string>("umbracoFile");
        }
    }
}
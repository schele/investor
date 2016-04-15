using Umbraco.Core;
using Umbraco.Core.Models;

namespace Investor.Models.Extensions
{
    public class MediaExtensions
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
    }
}
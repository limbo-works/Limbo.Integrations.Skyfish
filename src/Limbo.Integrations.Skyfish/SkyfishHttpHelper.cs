using System;
using System.Linq;
using Limbo.Integrations.Skyfish.Models.Media;
using Limbo.Integrations.Skyfish.Options.Videos;
using Limbo.Integrations.Skyfish.Responses.Search;

namespace Limbo.Integrations.Skyfish {
    
    public class SkyfishHttpHelper {

        #region Properties

        public SkyfishHttpService Service { get; }

        #endregion

        #region Constructors

        public SkyfishHttpHelper(SkyfishHttpService service) {
            Service = service ?? throw new ArgumentNullException(nameof(service));
        }

        #endregion

        #region Member methods

        public SkyfishMediaItem GetVideoByMediaId(int mediaId) {

            // Search for the video in the via the Search API
            SkyfishSearchResponse response = Service.Search.Search(new SkyfishSearchOptions {
                MediaId = mediaId
            });


            return response.Body.Media.FirstOrDefault();

        }

        public SkyfishMediaItem GetVideoByUniqueMediaId(int uniqueMediaId) {

            // Search for the video in the via the Search API
            SkyfishSearchResponse response = Service.Search.Search(new SkyfishSearchOptions {
                UniqueMediaId = uniqueMediaId
            });


            return response.Body.Media.FirstOrDefault();

        }

        public string GetEmbedUrlByUniqueMediaId(int uniqueMediaId) {
            return Service.GetEmbedUrl(uniqueMediaId);
        }

        #endregion

    }

}
using System;
using System.Linq;
using System.Net;
using System.Threading;
using Limbo.Integrations.Skyfish.Exceptions;
using Limbo.Integrations.Skyfish.Models.Media;
using Limbo.Integrations.Skyfish.Options.Search;
using Limbo.Integrations.Skyfish.Responses.Media;
using Limbo.Integrations.Skyfish.Responses.Search;

#pragma warning disable CS1591

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

        /// <summary>
        /// Gets the embed URL of the media with the specified <paramref name="uniqueMediaId"/>.
        ///
        /// If a stream doesn't already exist, the method will trigger Skyfish to start creating the stream. Skyfish
        /// will then create the stream in the background.
        ///
        /// The method will make <paramref name="retryCount"/> additional requests with a delay of
        /// <paramref name="delay"/> until a stream URL becomes available, or <paramref name="retryCount"/> has been
        /// exceeded.
        /// </summary>
        /// <param name="uniqueMediaId">The unique media ID of the media.</param>
        /// <param name="retryCount">The amount of retries if a stream URL is not immediately available.</param>
        /// <param name="delay">The delay between each retry.</param>
        /// <returns>The embed URL if successful; otherwise, <see langword="null"/>.</returns>
        public string? GetEmbedUrl(int uniqueMediaId, int retryCount, TimeSpan delay) {
            string? streamUrl = GetStreamUrl(uniqueMediaId, retryCount, delay);
            return string.IsNullOrWhiteSpace(streamUrl) ? null : $"https://player.skyfish.com/?v={streamUrl}&media={uniqueMediaId}";
        }

        /// <summary>
        /// Gets the stream URL of the media with the specified <paramref name="uniqueMediaId"/>.
        ///
        /// If a stream doesn't already exist, the method will trigger Skyfish to start creating the stream. Skyfish
        /// will then create the stream in the background.
        ///
        /// The method will make <paramref name="retryCount"/> additional requests with a delay of
        /// <paramref name="delay"/> until a stream URL becomes available, or <paramref name="retryCount"/> has been
        /// exceeded.
        /// </summary>
        /// <param name="uniqueMediaId">The unique media ID of the media.</param>
        /// <param name="retryCount">The amount of retries if a stream URL is not immediately available.</param>
        /// <param name="delay">The delay between each retry.</param>
        /// <returns>The stream URL if successful; otherwise, <see langword="null"/>.</returns>
        public string? GetStreamUrl(int uniqueMediaId, int retryCount, TimeSpan delay) {

            try {

                // Make the initial request to see whether a stream URL already exist
                SkyfishMediaStreamUrlResponse response = Service.Media.GetStreamUrl(uniqueMediaId);

                // If it exist, we can return it right away
                if (!string.IsNullOrWhiteSpace(response.Body.Stream)) return response.Body.Stream;

            } catch (SkyfishHttpException ex) when (ex.StatusCode == HttpStatusCode.NotFound) {

                // Make a new request to start creating the stream
                Service.Media.CreateStream(uniqueMediaId);

            }

            for (int i = 0; i < retryCount; i++) {

                // Sleep for a bit so we don't spam the API
                Thread.Sleep(delay);

                // Make the initial request to see whether a stream URL already exist
                SkyfishMediaStreamUrlResponse response = Service.Media.GetStreamUrl(uniqueMediaId);

                // If it exist, we can return it right away
                if (!string.IsNullOrWhiteSpace(response.Body.Stream)) return response.Body.Stream;

            }

            return null;

        }

        /// <summary>
        /// Returns the media media item with the specified <paramref name="mediaId"/>, or <see langword="null"/> if not found.
        /// </summary>
        /// <param name="mediaId">The ID of the media.</param>
        /// <returns>An instance of <see cref="SkyfishMediaItem"/> if successful; otherwise, <see langword="null"/>.</returns>
        public SkyfishMediaItem? GetMediaByMediaId(int mediaId) {

            // Search for the video in the via the Search API
            SkyfishSearchResponse response = Service.Search.Search(new SkyfishSearchOptions {
                MediaId = mediaId
            });


            return response.Body.Media.FirstOrDefault();

        }

        /// <summary>
        /// Returns the media media item with the specified <paramref name="uniqueMediaId"/>, or <see langword="null"/> if not found.
        /// </summary>
        /// <param name="uniqueMediaId">The unique ID of the media.</param>
        /// <returns>An instance of <see cref="SkyfishMediaItem"/> if successful; otherwise, <see langword="null"/>.</returns>
        public SkyfishMediaItem? GetMediaByUniqueMediaId(int uniqueMediaId) {

            // Search for the video in the via the Search API
            SkyfishSearchResponse response = Service.Search.Search(new SkyfishSearchOptions {
                UniqueMediaId = uniqueMediaId
            });


            return response.Body.Media.FirstOrDefault();

        }

        #endregion

    }

}
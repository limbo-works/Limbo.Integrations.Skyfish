using Limbo.Integrations.Skyfish.Models;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Http;
using Skybrud.Essentials.Http.Client;
using Skybrud.Essentials.Json;
using Skybrud.Essentials.Json.Extensions;
using Skybrud.Essentials.Security;
using Skybrud.Essentials.Time.UnixTime;
using System;
using System.Runtime.Caching;
using System.Threading;

namespace Limbo.Integrations.Skyfish.Http {
    public class SkyfishHttpClient : HttpClient {
        public string ApiKey { get; }
        public string SecretKey { get; }
        public string Username { get; }
        public string Password { get; }
        private readonly string _token;

        public SkyfishHttpClient() { }

        public SkyfishHttpClient(string apikey, string secretkey, string username, string password) {
            ApiKey = apikey;
            SecretKey = secretkey;
            Username = username;
            Password = password;

            _token = GetToken();
        }

        private string GetToken() {
            // Default token expiration is 14 days, so instead of requesting each time we cache it and get it from cache
            ObjectCache cache = MemoryCache.Default;
            string token = cache["skyfishToken"] as string;

            if (string.IsNullOrWhiteSpace(token)) {
                // Hmac hash needed for authing with Skyfish - https://api.skyfish.com/#sectionHead-21
                int unixTimestamp = (int) UnixTimeUtils.CurrentSeconds;
                string hmac = SecurityUtils.GetHmacSha1Hash(SecretKey, $"{ApiKey}:{unixTimestamp}");

                JObject body = new JObject {
                    ["username"] = Username,
                    ["password"] = Password,
                    ["key"] = ApiKey,
                    ["ts"] = unixTimestamp,
                    ["hmac"] = hmac.ToLower()
                };

                // Authenticate with Skyfish
                IHttpResponse result = HttpRequest.Post("https://api.colourbox.com/authenticate/userpasshmac", body).GetResponse();

                // Responds with unix expiration timestamp we can use to set the token expiration in cache - and the token itself
                SkyfishAuthResponse response = new SkyfishAuthResponse(JObject.Parse(result.Body));

                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(response.ValidUntilUnix);
                cache.Set("skyfishToken", response.Token, dateTimeOffset);

                return response.Token;
            }

            return token;
        }

        public SkyfishVideo GetVideo(int videoId) {
            SkyfishVideo video = GetSkyfishVideoData(videoId);
            var videoStream = GetSkyfishVideo(video.VideoId);

            // if stream doesn't exist we generate it
            if (videoStream.StatusCode != System.Net.HttpStatusCode.OK) {
                CreateSkyfishStream(video.VideoId);

                // wait 1 sec then check if it's ready
                Thread.Sleep(1000);
                video = GetVideo(video);
            } else {
                video.EmbedUrl = "https://player.skyfish.com/?v=" + JObject.Parse(videoStream.Body).GetString("Stream") + "&media=" + video.VideoId;
            }

            return video;
        }

        private SkyfishVideo GetVideo(SkyfishVideo video) {
            var videoStream = GetSkyfishVideo(video.VideoId);

            if (videoStream.StatusCode != System.Net.HttpStatusCode.OK) {
                // if not ready yet, we wait 1s then query again - normally quite fast
                Thread.Sleep(1000);
                video = GetVideo(video);
            } else {
                // for some reason they only return 404 if it's not currently generating, so any subsequent requests to see if its ready we need to parse the json body to see if it has the stream link
                if (string.IsNullOrWhiteSpace(JObject.Parse(videoStream.Body).GetString("Stream"))) {
                    // if not ready yet, we wait 1s then query again - normally quite fast
                    Thread.Sleep(1000);
                    video = GetVideo(video);
                } else {
                    video.EmbedUrl = "https://player.skyfish.com/?v=" + JObject.Parse(videoStream.Body).GetString("Stream") + "&media=" + video.VideoId;
                }
            }

            return video;
        }

        private SkyfishVideo GetSkyfishVideoData(int videoId) {
            var response = Get($"/search?media_id={videoId}&return_values=unique_media_id+height+width+title+description+thumbnail_url+thumbnail_url_ssl+filename+file_disksize+file_mimetype");
            return JsonUtils.ParseJsonObject(response.Body, SkyfishVideo.Parse);
        }

        private void CreateSkyfishStream(int videoId) {
            Post($"/media/{videoId}/stream");
        }

        private IHttpResponse GetSkyfishVideo(int videoId) {
            return Get($"/media/{videoId}/metadata/stream_url");
        }

        protected override void PrepareHttpRequest(IHttpRequest request) {

            // Append the scheme and domain of not already present
            if (request.Url.StartsWith("/")) request.Url = $"https://api.colourbox.com{request.Url}";

            // Set the "Authorization" header if the token is present
            if (!string.IsNullOrWhiteSpace(_token)) request.Authorization = $"CBX-SIMPLE-TOKEN Token={_token}";

        }
    }
}

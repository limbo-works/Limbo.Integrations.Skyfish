using System;
using System.Runtime.Caching;
using System.Threading;
using Limbo.Integrations.Skyfish.Endpoints;
using Limbo.Integrations.Skyfish.Models;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Common;
using Skybrud.Essentials.Http;
using Skybrud.Essentials.Http.Client;
using Skybrud.Essentials.Json.Extensions;
using Skybrud.Essentials.Security;
using Skybrud.Essentials.Time.UnixTime;

namespace Limbo.Integrations.Skyfish.Http {

    public class SkyfishHttpClient : HttpClient {

        public string? ApiKey { get; }

        public string? SecretKey { get; }

        public string? Username { get; }

        public string? Password { get; }

        public SkyfishMediaRawEndpoint Media { get; }

        public SkyfishSearchRawEndpoint Search { get; }

        private readonly string? _token;

        public SkyfishHttpClient() {
            Search = new SkyfishSearchRawEndpoint(this);
            Media = new SkyfishMediaRawEndpoint(this);
        }

        public SkyfishHttpClient(string apikey, string secretkey, string username, string password) {

            ApiKey = apikey;
            SecretKey = secretkey;
            Username = username;
            Password = password;

            Search = new SkyfishSearchRawEndpoint(this);
            Media = new SkyfishMediaRawEndpoint(this);

            _token = GetToken();
        }

        private string? GetToken() {

            if (string.IsNullOrWhiteSpace(ApiKey)) throw new PropertyNotSetException(nameof(ApiKey));
            if (string.IsNullOrWhiteSpace(SecretKey)) throw new PropertyNotSetException(nameof(SecretKey));
            if (string.IsNullOrWhiteSpace(Username)) throw new PropertyNotSetException(nameof(Username));
            if (string.IsNullOrWhiteSpace(Password)) throw new PropertyNotSetException(nameof(Password));

            // Default token expiration is 14 days, so instead of requesting each time we cache it and get it from cache
            ObjectCache cache = MemoryCache.Default;
            string? token = cache["skyfishToken"] as string;

            if (string.IsNullOrWhiteSpace(token)) {
                // Hmac hash needed for authing with Skyfish - https://api.skyfish.com/#sectionHead-21
                int unixTimestamp = (int) UnixTimeUtils.CurrentSeconds;
                string hmac = SecurityUtils.GetHmacSha1Hash(SecretKey!, $"{ApiKey}:{unixTimestamp}");

                JObject body = new() {
                    ["username"] = Username,
                    ["password"] = Password,
                    ["key"] = ApiKey,
                    ["ts"] = unixTimestamp,
                    ["hmac"] = hmac.ToLower()
                };

                // Authenticate with Skyfish
                IHttpResponse result = HttpRequest.Post("https://api.colourbox.com/authenticate/userpasshmac", body).GetResponse();

                // Responds with unix expiration timestamp we can use to set the token expiration in cache - and the token itself
                SkyfishAuthResponse response = new(JObject.Parse(result.Body));

                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(response.ValidUntilUnix);
                cache.Set("skyfishToken", response.Token, dateTimeOffset);

                return response.Token;
            }

            return token;

        }

        public string? GetEmbedUrl(int uniqueMediaId) {

            IHttpResponse videoStreamUrl = GetSkyfishVideoStream(uniqueMediaId);

            string? response = null;

            // if stream doesn't exist we generate it
            if (videoStreamUrl.StatusCode != System.Net.HttpStatusCode.OK) {
                CreateSkyfishStream(uniqueMediaId);

                Thread.Sleep(1000);

                // Call this method again
                GetEmbedUrl(uniqueMediaId);
            } else if (string.IsNullOrWhiteSpace(JObject.Parse(videoStreamUrl.Body).GetString("Stream"))) {
                // for some reason they only return 404 if it's not currently generating, so any subsequent requests to see if its ready we need to parse the json body to see if it has the stream link

                response = GetEmbedUrlWithRetries(uniqueMediaId, 0);
            } else {
                response = "https://player.skyfish.com/?v=" + JObject.Parse(videoStreamUrl.Body).GetString("Stream") + "&media=" + uniqueMediaId;
            }

            return response;
        }

        private string? GetEmbedUrlWithRetries(int uniqueMediaId, int count) {
            IHttpResponse videoStream = GetSkyfishVideoStream(uniqueMediaId);

            // Exit condition? Only retry for 2 mins.
            if (count >= 120) return null;

            string? response;

            if (string.IsNullOrWhiteSpace(JObject.Parse(videoStream.Body).GetString("Stream"))) {
                // for some reason they only return 404 if it's not currently generating, so any subsequent requests to see if its ready we need to parse the json body to see if it has the stream link

                Thread.Sleep(1000);
                count++;

                response = GetEmbedUrlWithRetries(uniqueMediaId, count);
            } else {
                response = "https://player.skyfish.com/?v=" + JObject.Parse(videoStream.Body).GetString("Stream") + "&media=" + uniqueMediaId;
            }

            return response;
        }

        private void CreateSkyfishStream(int uniqueMediaId) {
            Post($"/media/{uniqueMediaId}/stream");
        }

        private IHttpResponse GetSkyfishVideoStream(int videoId) {
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
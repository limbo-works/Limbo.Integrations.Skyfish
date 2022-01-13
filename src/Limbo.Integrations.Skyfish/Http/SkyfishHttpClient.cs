using System;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Limbo.Integrations.Skyfish.Extensions;
using Limbo.Integrations.Skyfish.Models;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Http;
using Skybrud.Essentials.Json.Extensions;

namespace Limbo.Integrations.Skyfish.Http
{
    public class SkyfishHttpClient
    {
        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        private readonly string _token;

        public SkyfishHttpClient() { }

        public SkyfishHttpClient(string apikey, string secretkey, string username, string password)
        {
            ApiKey = apikey;
            SecretKey = secretkey;
            Username = username;
            Password = password;

            _token = GetToken();
        }

        private string GetToken()
        {
            // Default token expiration is 14 days, so instead of requesting each time we cache it and get it from cache
            ObjectCache cache = MemoryCache.Default;
            string token = cache["skyfishToken"] as string;

            if(string.IsNullOrWhiteSpace(token))
            {
                // Hmac hash needed for authing with Skyfish - https://api.skyfish.com/#sectionHead-21
                int unixTimestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                string hmac = "";
                var keyByte = Encoding.UTF8.GetBytes(SecretKey);
                using (var hmacsha1 = new HMACSHA1(keyByte))
                {
                    hmacsha1.ComputeHash(Encoding.UTF8.GetBytes($"{ApiKey}:{unixTimestamp}"));

                    hmac += StringExtensions.ByteToString(hmacsha1.Hash);
                }

                JObject body = new JObject
                {
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
        
        public SkyfishVideo GetVideo(int videoId)
        {
            SkyfishVideo video = GetSkyfishVideoData(videoId);
            var videoStream = GetSkyfishVideo(video.VideoId);

            // if stream doesn't exist we generate it
            if(videoStream.StatusCode != System.Net.HttpStatusCode.OK)
            {
                CreateSkyfishStream(video.VideoId);

                // wait 1 sec then check if it's ready
                Thread.Sleep(1000);
                video = GetVideo(video);
            } else
            {
                video.EmbedUrl = "https://player.skyfish.com/?v=" + JObject.Parse(videoStream.Body).GetString("Stream") + "&media=" + video.VideoId;                
            }

            return video;
        }

        private SkyfishVideo GetVideo(SkyfishVideo video)
        {            
            var videoStream = GetSkyfishVideo(video.VideoId);

            if (videoStream.StatusCode != System.Net.HttpStatusCode.OK)
            {
                // if not ready yet, we wait 1s then query again - normally quite fast
                Thread.Sleep(1000);
                video = GetVideo(video);
            }
            else
            {
                // for some reason they only return 404 if it's not currently generating, so any subsequent requests to see if its ready we need to parse the json body to see if it has the stream link
                if (string.IsNullOrWhiteSpace(JObject.Parse(videoStream.Body).GetString("Stream")))
                {
                    // if not ready yet, we wait 1s then query again - normally quite fast
                    Thread.Sleep(1000);
                    video = GetVideo(video);
                }
                else
                {
                    video.EmbedUrl = "https://player.skyfish.com/?v=" + JObject.Parse(videoStream.Body).GetString("Stream") + "&media=" + video.VideoId;
                }                
            }

            return video;
        }

        private SkyfishVideo GetSkyfishVideoData(int videoId)
        {
            var req = HttpRequest.Get($"https://api.colourbox.com/search?media_id={videoId}&return_values=unique_media_id+height+width+title+description+thumbnail_url+thumbnail_url_ssl+filename+file_disksize+file_mimetype");
            req.Headers.Authorization = $"CBX-SIMPLE-TOKEN Token={_token}";
            var result = req.GetResponse();

            return SkyfishVideo.Parse(JObject.Parse(result.Body));
        }

        private void CreateSkyfishStream(int videoId)
        {
            var req = HttpRequest.Post($"https://api.colourbox.com/media/{videoId}/stream");
            req.Headers.Authorization = $"CBX-SIMPLE-TOKEN Token={_token}";
            req.GetResponse();
        }

        private IHttpResponse GetSkyfishVideo(int videoId)
        {
            var req = HttpRequest.Get($"https://api.colourbox.com/media/{videoId}/metadata/stream_url");
            req.Headers.Authorization = $"CBX-SIMPLE-TOKEN Token={_token}";
            return req.GetResponse();
        }
    }
}

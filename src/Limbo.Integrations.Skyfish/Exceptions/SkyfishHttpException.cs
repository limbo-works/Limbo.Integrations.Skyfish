using Skybrud.Essentials.Http;
using Skybrud.Essentials.Http.Exceptions;
using System;
using System.Net;

namespace Limbo.Integrations.Skyfish.Exceptions {
    public class SkyfishHttpException : Exception, IHttpException {
        public IHttpResponse Response { get; }

        public HttpStatusCode StatusCode => Response.StatusCode;

        public SkyfishHttpException(IHttpResponse response) : base($"Invalid response received from the Skyfish API (Status: {(int) response.StatusCode}") {
            Response = response;
        }
    }
}

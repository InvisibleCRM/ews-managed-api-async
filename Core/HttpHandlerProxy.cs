using System.Net.Http;
using System.Net;
using Microsoft.Net.Http.Headers;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Exchange.WebServices.NETStandard.Extensions;

namespace Microsoft.Exchange.WebServices.NETStandard.Core
{
    internal class HttpHandlerProxy : DelegatingHandler
    {
        private readonly string httpClientName;
        private readonly IHttpMessageHandlerFactory httpMessageHandlerFactory;

        /// <summary>
        /// Gets or sets the cookie container.
        /// </summary>
        /// <value>The cookie container.</value>
        public CookieContainer CookieContainer { get; set; }

        /// <summary>
        /// Gets or sets authentication information for the request.
        /// </summary>
        /// <returns>An <see cref="T:System.Net.ICredentials"/> that contains the authentication credentials associated with the request. The default is null.</returns>
        public ICredentials Credentials { get; set; }

        /// <summary>
        /// Gets or sets proxy information for the request.
        /// </summary>
        //public IWebProxy Proxy { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether to send an authenticate header with the request.
        /// </summary>
        /// <returns>true to send a WWW-authenticate HTTP header with requests after authentication has taken place; otherwise, false. The default is false.</returns>
        //public bool PreAuthenticate { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="T:System.Boolean"/> value that controls whether default credentials are sent with requests.
        /// </summary>
        /// <returns>true if the default credentials are used; otherwise false. The default value is false.</returns>
        //public bool UseDefaultCredentials { get; set; }

        public HttpHandlerProxy(IHttpMessageHandlerFactory httpMessageHandlerFactory, string httpClientName)
        {
            this.httpClientName = httpClientName;
            this.httpMessageHandlerFactory = httpMessageHandlerFactory;
        }

        private bool UseDedicatedHandler()
        {
            return Credentials is NetworkCredential;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var cookieHeader = CookieContainer?.GetCookieHeader(request.RequestUri);
            if (!string.IsNullOrEmpty(cookieHeader))
            {
                request.Headers.Add(HeaderNames.Cookie, cookieHeader);
            }

            if (null == InnerHandler)
            {
                // SB: special case for Negotiate authentication, we can't reuse named HttpMessageHandler,
                // since this type of auth requires credentials to be set on handler level, after that
                // handler can't be reused with another credentials
                if (UseDedicatedHandler())
                {
                    var handler = HttpClientExtensions.CreateHttpClientHandler(httpClientName);
                    handler.Credentials = Credentials;

                    InnerHandler = handler;
                }
                else
                {
                    InnerHandler = httpMessageHandlerFactory?.CreateHandler(httpClientName) ?? HttpClientExtensions.CreateHttpClientHandler(httpClientName);
                }
            }

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            if (response.Headers.TryGetValues(HeaderNames.SetCookie, out var values))
            {
                values.ForEach(c => CookieContainer?.SetCookies(request.RequestUri, c));
            }

            return response;
        }
    }
}

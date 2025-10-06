using BestApp.Abstraction.Common;
using BestApp.Abstraction.General.Infasructures.REST;
using Common.Abstrtactions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace BestApp.Impl.Cross.Infasructures.REST
{
    internal class RestClient : IRestClient
    {
        private readonly Lazy<IAuthTokenService> authTokenService;
        private readonly Lazy<ILoggingService> loggingService;
        private readonly Lazy<IConstants> constants;

        public RestClient(Lazy<IAuthTokenService> authTokenService, Lazy<ILoggingService> loggingService, Lazy<IConstants> constants)
        {
            this.authTokenService = authTokenService;
            this.loggingService = loggingService;
            this.constants = constants;
        }

        public const int DEFAULT_SMALL_TIMEOUT = 10;
        public const int DEFAULT_MEDIUM_TIMEOUT = 30;
        public const int DEFAULT_LARGE_TIMEOUT = 60;
        public const int DEFAULT_TOO_LARGE_TIMEOUT = 120;
        protected static readonly HttpClient getHttpClient = new HttpClient()
        {
            Timeout = TimeSpan.FromSeconds(DEFAULT_SMALL_TIMEOUT),
        };

        protected static readonly HttpClient mediumHttpClient = new HttpClient()
        {
            Timeout = TimeSpan.FromSeconds(DEFAULT_MEDIUM_TIMEOUT),
        };

        protected static readonly HttpClient largeHttpClient = new HttpClient()
        {
            Timeout = TimeSpan.FromSeconds(DEFAULT_LARGE_TIMEOUT),
        };

        protected static readonly HttpClient veryLargeHttpClient = new HttpClient()
        {
            Timeout = TimeSpan.FromSeconds(DEFAULT_TOO_LARGE_TIMEOUT),
        };        

        public async Task<string> DoHttpRequest(RestMethod method, RestRequest restRequest)
        {
            var request = new HttpRequestMessage();
            switch (method)
            {
                case RestMethod.GET:
                    request.Method = HttpMethod.Get;
                    break;
                case RestMethod.POST:
                    request.Method = HttpMethod.Post;
                    break;
                case RestMethod.PUT:
                    request.Method = HttpMethod.Put;
                    break;
                case RestMethod.DELETE:
                    request.Method = HttpMethod.Delete;
                    break;
                default:
                    throw new ArgumentException("invalid method");
            }
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var accessToken = await authTokenService.Value.GetToken();
            if (!string.IsNullOrEmpty(accessToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (restRequest.HeaderValues != null)
            {
                foreach (var item in restRequest.HeaderValues)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }

            //make full uri
            var baseUri = new Uri(constants.Value.ServerUrlHost);
            var fullUri = new Uri(baseUri, restRequest.ApiEndpoint);
            var jsonBody = string.Empty;
            //set body
            if (restRequest.Body != null)
            {               
                if (Debugger.IsAttached)
                {
                    //show it good in output or in logger
                    jsonBody = JsonConvert.SerializeObject(restRequest.Body, Formatting.Indented);
                }
                else
                {
                    jsonBody = JsonConvert.SerializeObject(restRequest.Body);
                }
                
                var mediaType = "application/json";
                HttpContent payload = new StringContent(jsonBody, Encoding.UTF8, mediaType);
                request.Content = payload;

                loggingService.Value.LogMethodStarted($"DoHttpRequest({method}, {fullUri}, {jsonBody})");
            }
            else
            {
                loggingService.Value.LogMethodStarted($"DoHttpRequest({method}, {fullUri})");
            }

            request.RequestUri = fullUri;
            //send the request
            HttpResponseMessage response;
            if (restRequest.TimeoutType == TimeoutType.High)
            {
                response = await largeHttpClient.SendAsync(request).ConfigureAwait(false);
            }
            else if (restRequest.TimeoutType == TimeoutType.VeryHigh)
            {
                response = await veryLargeHttpClient.SendAsync(request).ConfigureAwait(false);
            }
            else if (method != RestMethod.GET || restRequest.TimeoutType == TimeoutType.Medium)
            {
                response = await mediumHttpClient.SendAsync(request).ConfigureAwait(false);
            }
            else
            {
                response = await getHttpClient.SendAsync(request).ConfigureAwait(false);
            }

            using (response)
            {
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                //log response without a sensivity data
                var contentForLog = HideSensivityData(responseContent);
                if (!string.IsNullOrEmpty(jsonBody))
                    loggingService.Value.LogMethodFinished($"DoHttpRequest({method}, {fullUri}, {jsonBody}) with result: {contentForLog}");
                else
                    loggingService.Value.LogMethodFinished($"DoHttpRequest({method}, {fullUri}) with result: {contentForLog}");

                return responseContent;
            }
        }

        private string HideSensivityData(string data)
        {
            if (data.Contains("access_token"))
            {
                if (Debugger.IsAttached)
                {
                    return data;
                }
                else
                {
                    var jobject = JObject.Parse(data);
                    jobject.Remove("access_token");
                    jobject.Remove("userName");
                    jobject.Remove("phoneNumber");
                    jobject.Remove("token_type");
                    jobject.Remove(".issued");
                    jobject.Remove(".expires");
                    jobject.Remove("expires_in");
                    var str = jobject.ToString();
                    return str;
                }
            }

            return data;
        }
    }
}

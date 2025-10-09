using BestApp.Abstraction.Common.Events;
using BestApp.Abstraction.Main.Infasructures.Events;
using BestApp.Abstraction.Main.Infasructures.Exceptions;
using BestApp.Abstraction.Main.Infasructures.REST;
using Common.Abstrtactions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BestApp.Impl.Cross.Infasructures.REST
{
    internal class RestService
    {
        public RestService(Lazy<ILoggingService> loggingService, 
                           Lazy<IAuthTokenService> authTokenService, 
                           Lazy<IRestClient> restClient, 
                           Lazy<IMessagesCenter> eventAggregator,
                           RequestQueueList requestQueues)
        {
            this.loggingService = loggingService;            
            this.authTokenService = authTokenService;
            this.restClient = restClient;
            this.eventAggregator = eventAggregator;
            QueueList = requestQueues;
        }
                
        private RequestQueueList QueueList = null;
        private readonly Lazy<ILoggingService> loggingService;        
        private readonly Lazy<IAuthTokenService> authTokenService;
        private readonly Lazy<IRestClient> restClient;
        private readonly Lazy<IMessagesCenter> eventAggregator;
        private string Tag = "RestClientService: ";       

       
        protected Task<T> Get<T>(RestRequest restRequest)
        {
            return CatchAuthErrors(() =>
            {
                return MakeWebRequest<T>(RestMethod.GET, restRequest);
            });
            
        }

        protected Task<T> Post<T>(RestRequest restRequest)
        {
            return CatchAuthErrors(() =>
            {
                return MakeWebRequest<T>(RestMethod.POST, restRequest);
            });
        }

        protected Task Put(RestRequest restRequest)
        {
            return CatchAuthErrors(() =>
            {
                return MakeWebRequest<object>(RestMethod.PUT, restRequest);
            });                
        }

        protected Task<object> Delete(RestRequest restRequest)
        {
            return CatchAuthErrors(() =>
            {
                return MakeWebRequest<object>(RestMethod.DELETE, restRequest);
            });
            
        }

        protected virtual T Deserialize<T>(string json)
        {
            //check response for error
            if(json.Contains("error:"))
            {
                var obj = JsonConvert.DeserializeObject<JObject>(json);
                if(obj.ContainsKey("error"))
                {
                    var error = obj["error"].ToString();
                    throw new RestApiException(error);
                }
            }            

            var model = JsonConvert.DeserializeObject<T>(json);            
            return model;
        }

        private async Task<T> CatchAuthErrors<T>(Func<Task<T>> requestAction)
        {
            try
            {
                var result = await requestAction();
                return result;
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                //invalid access token
                eventAggregator.Value.GetEvent<AuthErrorEvent>().Publish();
                throw;
            }
            catch (AuthExpiredException)
            {
                //invalid access token
                eventAggregator.Value.GetEvent<AuthErrorEvent>().Publish();
                throw;
            }
        }

        private async Task<T> MakeWebRequest<T>(RestMethod restMethod, RestRequest restRequest)
        {   
            if (restRequest.WithBearer)
            {
                await authTokenService.Value.EnsureAuthValid();
            }                      
            var requestResult = await AddRequestToQueue(restMethod, restRequest);            
            var response = Deserialize<T>(requestResult);
            return response;
        }

        
        private Task<string> AddRequestToQueue(RestMethod method, RestRequest restRequest)
        {            
            var path = GetUrlWithoutParam(restRequest.ApiEndpoint);
            var queueItemId = $"{method}{path}/{restRequest.Priority}/{restRequest.CancelSameRequest}";

            Log($"Request {method}: {queueItemId}, priority: {restRequest.Priority} is added to the Queue");
            RequestQueueItem item = new RequestQueueItem()
            {
                Id = queueItemId,
                Priority = restRequest.Priority,
                TimeoutType = restRequest.TimeoutType,
                LoggingService = loggingService.Value,
                RequestAction = () => restClient.Value.DoHttpRequest(method, restRequest),
                ParentList = QueueList
            };
            //Bellow is very dangerous code thus was commented
            //if the same request is made from two different places it will cancel one of them
            //TODO: it is better to set the result for the duplicate request from first request(if first request fails then the second should continue to execute)
            //if (cancelSameRequest && QueueList.Any(s => s.Id == item.Id))
            //{
            //    Log($"Similar request id: {item.Id} exists in the Queue, thus old will be removed");
            //    var list = QueueList.Where(s => s.Id == item.Id).ToList();
            //    foreach (var q in list)
            //    {
            //        QueueList.Remove(q);
            //        q.CompletionSource.TrySetCanceled();
            //    }
            //}
            QueueList.Add(item);

            return item.CompletionSource.Task;
        }

        
        private void Log(string message)
        {
            loggingService.Value.Log($"{Tag}{message}");
        }

        private void LogWarning(string message)
        {
            loggingService.Value.LogWarning($"{Tag}{message}");
        }

        private string GetUrlWithoutParam(string url)
        {
            var hasQuery = url.Contains("?");
            url = hasQuery ? url.Split('?')[0] : url;
            var segments = url.Split('/');
            var newUrl = string.Empty;
            var count = hasQuery ? segments.Length : segments.Length - 1;

            if (!hasQuery && segments.Length == 3)
                count = segments.Length;

            for (var i = 0; i < count; i++)
            {
                if (string.IsNullOrEmpty(segments[i]))
                    continue;

                newUrl += "/" + segments[i];
            }

            return newUrl;
        }        
    }
}


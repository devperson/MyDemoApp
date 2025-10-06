using BestApp.Abstraction.General.Infasructures.REST;
using Common.Abstrtactions;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Impl.Cross.Infasructures.REST
{
    internal class RestService
    {
        public RestService(Lazy<ILoggingService> loggingService, Lazy<IAuthTokenService> authTokenService, Lazy<IRestClient> restClient)
        {
            this.loggingService = loggingService;            
            this.authTokenService = authTokenService;
            this.restClient = restClient;
            QueueList = new RequestQueueList(loggingService);
            QueueList.RequestStarted += QueueList_RequestStarted;
            QueueList.RequestPending += QueueList_RequestPending;
            QueueList.RequestCompleted += QueueList_RequestCompleted;
        }
                
        private RequestQueueList QueueList = null;
        private readonly Lazy<ILoggingService> loggingService;        
        private readonly Lazy<IAuthTokenService> authTokenService;
        private readonly Lazy<IRestClient> restClient;
        private string Tag = "RestClientService: ";                
        public const int RETRY_COUNT = 4;
        

       
        protected Task<T> Get<T>(RestRequest restRequest)
        {
            return MakeWebRequest<T>(RestMethod.GET, restRequest);
        }

        protected Task<T> Post<T>(RestRequest restRequest)
        {
            return MakeWebRequest<T>(RestMethod.POST, restRequest);
        }

        protected Task Put(RestRequest restRequest)
        {
            return MakeWebRequest<object>(RestMethod.PUT, restRequest);         
        }

        protected Task<object> Delete(RestRequest restRequest)
        {
            return MakeWebRequest<object>(RestMethod.DELETE, restRequest);
        }

        protected virtual T Deserialize<T>(string json)
        {
            var obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
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

        private void QueueList_RequestPending(object sender, RequestQueueItem e)
        {
            try
            {
                Log($"Waiting to running requests to complete. {GetQueueInfo()}");
            }
            catch (Exception ex)
            {
                loggingService.Value.LogError(ex, string.Empty);
            }
        }

        private void QueueList_RequestStarted(object sender, RequestQueueItem e)
        {
            try
            {
                Log($"The next request {e.Id} started. {GetQueueInfo()}");
            }
            catch (Exception ex)
            {
                loggingService.Value.LogError(ex, string.Empty);
            }
        }

        private void QueueList_RequestCompleted(object sender, RequestQueueItem e)
        {
            try
            {
                Log($"The request {e.Id} completed. {GetQueueInfo()}");
            }
            catch (Exception ex)
            {
                loggingService.Value.LogError(ex, string.Empty);
            }
        }

        private string GetQueueInfo()
        {
            var list = QueueList.ToList();
            var totalCount = list.Count;
            var runningCount = list.Count(s => s.IsRunning);
            var priorityCount = list.Count(s => s.Priority == Priority.HIGH);
            var infoStr = $"Queue total count: {totalCount}, running count: {runningCount}, high priority count: {priorityCount}";

            return infoStr;
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


using Base.Abstractions.Diagnostic;
using Base.Infrastructures.Abstractions.REST;
using System.Timers;

namespace BestApp.Impl.Cross.Infasructures.REST
{
    public class RequestQueueList : List<RequestQueueItem>
    {
        public RequestQueueList(Lazy<ILoggingService> loggingService)
        {
            this.loggingService = loggingService;
            timeOutTimer.Elapsed += TimeOutTimer_Elapsed;
        }

        public event EventHandler<RequestQueueItem> RequestStarted;
        public event EventHandler<RequestQueueItem> RequestPending;
        public event EventHandler<RequestQueueItem> RequestCompleted;
        private System.Timers.Timer timeOutTimer = new System.Timers.Timer(10000);

        private int MaxBackgroundRequest = 1;
        private int MaxHighPriority = 2;
        public new void Add(RequestQueueItem item)
        {
            base.Add(item);
            _ = TryRunNextRequest();

            Resume();
        }

        public void Release()
        {
            timeOutTimer.Stop();
            Clear();
        }

        public void Pause()
        {
            timeOutTimer.Stop();
        }

        public void Resume()
        {
            if (!timeOutTimer.Enabled)
            {
                loggingService.Value.Log("RequestQueueList: Starting timer that checks time out reuest in the Queue list");
                timeOutTimer.Start();
            }
        }

        SemaphoreSlim queueSemaphor = new SemaphoreSlim(1, 1);
        private readonly Lazy<ILoggingService> loggingService;

        public async Task<bool> TryRunNextRequest()
        {
            var canStart = false;
            try
            {
                await queueSemaphor.WaitAsync();
                var currentList = this.ToList();
                var item = currentList.OrderBy(s => s.Priority).FirstOrDefault(s => !s.IsRunning && !s.IsCompleted);

                if (item != null)
                {
                    //up to 3 simultaneous for High priority
                    if (item.Priority == Priority.HIGH && currentList.Count(s => s.Priority == Priority.HIGH && s.IsRunning) <= MaxHighPriority)
                    {
                        canStart = true;
                    }
                    //up to 2 simultaneous for other priority
                    if (item.Priority != Priority.HIGH && !currentList.Any(s => s.Priority == Priority.HIGH && s.IsRunning))
                    {
                        var currentRunningCount = currentList.Count(s => s.IsRunning);
                        if (currentRunningCount <= MaxBackgroundRequest)
                        {
                            canStart = true;
                        }
                    }

                    if (canStart)
                    {
                        OnRequestStarted(item);                        
                        item.RunRequest();
                    }
                    else
                    {
                        OnRequestPending(currentList.LastOrDefault());                        
                    }
                }
            }
            catch (Exception ex)
            {
                loggingService.Value.TrackError(ex);
                //let to 
                return true;
            }
            finally
            {
                queueSemaphor.Release();
            }
            return canStart;
        }

        public async void OnItemCompleted(RequestQueueItem requestQueueItem)
        {
            OnRequestCompleted(requestQueueItem);
            //CheckForSimilarRequest(requestQueueItem);

            var currentCount = this.Count;
            for (int i = 0; i < currentCount; i++)
            {
                var val = await TryRunNextRequest();
                if (!val)
                    break;
            }
        }


        private void TimeOutTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            loggingService.Value.Log("RequestQueueList: Time out timer tick elapsed to check request that time out.");
            CheckTimeOutRequest();
        }

        private void CheckTimeOutRequest()
        {
            var timeOutList = this.Where(s => s.IsTimeOut).ToList();

            if (timeOutList.Count == 0)
            {
                loggingService.Value.Log($"RequestQueueList: No time out requests, total items count: {Count}");
            }
            else
            {
                loggingService.Value.Log($"RequestQueueList: Found {timeOutList.Count} time out items, removing them");
                foreach (var requestItem in timeOutList)
                {
                    requestItem.ForceToComplete(new OperationCanceledException($"The request id:{requestItem.Id} is TIME OUT"), $"RequestQueueList: The request id:{requestItem.Id} is TIME OUT");
                    requestItem.RemoveFromParent();
                }

                if (Count == 0)
                {
                    loggingService.Value.Log($"RequestQueueList: No items to run (Count:0)");
                }
                else
                {
                    loggingService.Value.Log($"RequestQueueList: Calling the TryRunNextRequest() to run next item in the Queue, totalCount: {Count}");
                    _ = TryRunNextRequest();
                }
            }
        }

        private void OnRequestPending(RequestQueueItem item)
        {
            try
            {
                loggingService.Value.Log($"Waiting to running requests to complete. {GetQueueInfo()}");
                RequestPending?.Invoke(this, item);
            }
            catch (Exception ex)
            {
                loggingService.Value.LogError(ex, string.Empty);
            }
        }

        private void OnRequestStarted(RequestQueueItem e)
        {
            try
            {
                loggingService.Value.Log($"The next request {e.Id} started. {GetQueueInfo()}");
            }
            catch (Exception ex)
            {
                loggingService.Value.LogError(ex, string.Empty);
            }
        }

        private void OnRequestCompleted(RequestQueueItem e)
        {
            try
            {
                loggingService.Value.Log($"The request {e.Id} completed. {GetQueueInfo()}");
            }
            catch (Exception ex)
            {
                loggingService.Value.LogError(ex, string.Empty);
            }
        }

        private string GetQueueInfo()
        {
            var list = this.ToList();
            var totalCount = list.Count;
            var runningCount = list.Count(s => s.IsRunning);
            var priorityCount = list.Count(s => s.Priority == Priority.HIGH);
            var infoStr = $"Queue total count: {totalCount}, running count: {runningCount}, high priority count: {priorityCount}";

            return infoStr;
        }


        //private void CheckForSimilarRequest(RequestQueueItem requestQueueItem)
        //{
        //    var theSameQuery = this.Where(s => s.Id == requestQueueItem.Id).ToList();
        //    foreach (var similar in theSameQuery)
        //    {                
        //        if (!similar.CompletionSource.Task.IsCanceled)
        //        {
        //            loggingService.Value.Log($"RestClientService: RequestQueueList: Setting the same value for {similar.Id} from similar request");
        //            similar.CompletionSource.SetResult(requestQueueItem.Result);
        //        }
        //    }
        //}
    }

    public class RequestQueueItem
    {      
        public DateTime StartedAt { get; set; }
        public RequestQueueList ParentList { get; set; }
        public string Id { get; set; }
        public Func<Task<string>> RequestAction { get; set; }
        public Priority Priority { get; set; }
        public TimeoutType TimeoutType { get; set; }
        public TaskCompletionSource<string> CompletionSource { get; private set; } = new TaskCompletionSource<string>();
        public bool IsCompleted { get; set; }
        public bool IsRunning { get; set; }
        public string Result { get; set; }
        public ILoggingService LoggingService { get; set; }

        public int TimeOut
        {
            get
            {
                if (TimeoutType == TimeoutType.High)
                {
                    return (int)TimeoutType.High + 5;
                }
                else if (TimeoutType == TimeoutType.VeryHigh)
                {
                    return (int)TimeoutType.VeryHigh + 5;
                }
                else
                {
                    return (int)TimeoutType.Medium + 1;
                }
            }
        }

        public bool IsTimeOut
        {
            get
            {
                if (this.StartedAt == DateTime.MinValue)
                    return false;

                if (this.IsCompleted)
                    return false;

                var elapsed = DateTime.Now.Subtract(StartedAt);
                if (elapsed.TotalSeconds > TimeOut)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async void RunRequest()
        {
            try
            {
                IsRunning = true;
                StartedAt = DateTime.Now;
                Result = await RequestAction?.Invoke();

                if (!CompletionSource.Task.IsCanceled)
                {
                    CompletionSource.TrySetResult(Result);
                }
                else
                {
                    LoggingService.LogWarning($"RequestQueueItem: Skip setting result for Id:{Id} because CompletionSource.Task.Status == {CompletionSource.Task.Status}");
                }
                IsRunning = false;
                IsCompleted = true;
            }
            catch (Exception ex)
            {
                ForceToComplete(ex, $"RequestQueueItem: Id:{Id} Failed to invoke RequestAction(), exception details: {ex}");
            }

            RemoveFromParent();
        }

        public void ForceToComplete(Exception error, string logString)
        {
            try
            {
                if (IsCompleted)
                {
                    LoggingService.LogWarning($"No need to force complete the request {Id} because it is already completed");
                    return;
                }

                IsRunning = false;
                IsCompleted = true;
                LoggingService.LogWarning(logString);
                CompletionSource.TrySetException(error);
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, $"Failed to force complete the RequestQueueItem {Id}");
            }
        }

        public void RemoveFromParent()
        {
            try
            {
                if (ParentList.Contains(this))
                {
                    ParentList.Remove(this);
                    ParentList.OnItemCompleted(this);
                }
                ParentList = null;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex, $"Failed to remove the RequestQueueItem {Id} from list");
            }
        }
    }
}

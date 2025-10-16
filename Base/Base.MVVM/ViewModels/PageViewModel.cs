using Base.Abstractions.REST.Exceptions;
using Base.Aspect;
using Base.MVVM.Events;
using Base.MVVM.Navigation;
using System.Net;
using INavigationParameters = Base.MVVM.Navigation.INavigationParameters;

namespace Base.MVVM.ViewModels
{
    [LogMethods]
    public abstract class PageViewModel : NavigatingBaseViewModel, IPageLifecycleAware
    {        
        private bool isFirstTimeAppears = true;
        private AppResumedEvent appResumedEvent;
        private AppPausedEvent appPausedEvent;

        public PageViewModel(InjectedServices services) : base(services)
        {
            this.InstanceId = Guid.NewGuid().ToString();

            if (services != null)
            {
                appResumedEvent = injectedServices.EventAggregator.GetEvent<AppResumedEvent>();
                appPausedEvent = injectedServices.EventAggregator.GetEvent<AppPausedEvent>();
                appResumedEvent.Subscribe(ResumedFromBackground);
                appPausedEvent.Subscribe(PausedToBackground);
            }
        }

        public string Title { get; set; }
        public string InstanceId { get; set; }

        public bool IsPageVisable { get; set; }

        /// <summary>
        /// General Busy indicator that will be displayed as popup
        /// </summary>
        public bool BusyLoading { get; set; }


        [ExcludeFromLog]//manually log
        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            injectedServices.LoggingService.Log($"{this.GetType().Name}.Initialize() (from base)");
        }

        [ExcludeFromLog]//manually log
        public virtual void OnAppearing()
        {
            IsPageVisable = true;
            RaisePropertyChanged(nameof(CanGoBack));
            injectedServices.LoggingService.Log($"{this.GetType().Name}.OnAppearing() (from base)");

            if (isFirstTimeAppears)
            {
                isFirstTimeAppears = false;
                OnFirstTimeAppears();
            }            
        }

        [ExcludeFromLog]//manually log
        public virtual void OnFirstTimeAppears()
        {
            injectedServices.LoggingService.Log($"{this.GetType().Name}.OnFirstTimeAppears() (from base)");
        }


        [ExcludeFromLog]//manually log
        public virtual void OnAppeared()
        {
            injectedServices.LoggingService.Log($"{this.GetType().Name}.OnAppeared() (from base)");
        }

        [ExcludeFromLog]//manually log
        public virtual void OnDisappearing()
        {
            IsPageVisable = false;
            injectedServices.LoggingService.Log($"{this.GetType().Name}.OnDisappearing() (from base)");
        }

        [ExcludeFromLog]//manually log
        public virtual void ResumedFromBackground(object arg)
        {
            injectedServices.LoggingService.Log($"{this.GetType().Name}.ResumedFromBackground() (from base)");
        }

        [ExcludeFromLog]//manually log
        public virtual void PausedToBackground(object arg)
        {
            injectedServices.LoggingService.Log($"{this.GetType().Name}.PausedToBackground() (from base)");
        }

        [ExcludeFromLog]//manually log
        public override void Destroy()
        {
            base.Destroy();

            injectedServices.LoggingService.Log($"{this.GetType().Name}.Destroy() (from base)");

            appResumedEvent.Unsubscribe(ResumedFromBackground);
            appPausedEvent.Unsubscribe(PausedToBackground);
        }

        //************************************************************************

        /// <summary>
        /// This method is recommended to be called in a Command. 
        /// </summary>
        /// <param name="asyncAction">action to be executed</param>
        /// <param name="OnComplete">success lambada</param>
        /// <returns></returns>
        public async Task ShowLoading(Func<Task> asyncAction, Action<bool> OnComplete = null)
        {
            try
            {
                BusyLoading = true;
                await asyncAction();
                OnComplete?.Invoke(true);
            }
            finally
            {
                BusyLoading = false;
            }
        }

        public async Task ShowLoadingAndHandleError(Func<Task> asyncAction, Action<bool> OnComplete = null, bool skipCheckInternet = false, bool setIsBusy = true)
        {
            bool success = false;
            try
            {
                //if (!Service.DeviceInfo.HasInternetConnection)
                //{

                //    Services.PopupAlertService.ShowError(Strings.Error_NoInternet);
                //    OnComplete?.Invoke(false);
                //    return;
                //}

                BusyLoading = setIsBusy;
                success = await ExecuteAndHandleError(asyncAction);
            }
            finally
            {
                BusyLoading = false;
                OnComplete?.Invoke(success);
            }
        }

        protected async Task<bool> ExecuteAndHandleError(Func<Task> asyncAction)
        {
            try
            {
                await asyncAction();
                return true;
            }
            catch (Exception x)
            {
                HandleUIError(x);
                return false;
            }
        }


        /// <summary>
        /// Shows aler/error message
        /// Use this method when exception happened for user triggered action
        /// </summary>
        /// <param name="x"></param>
        public void HandleUIError(Exception x)
        {
            injectedServices.LoggingService.TrackError(x);
            //Check is SERVER API error
            if (x is HttpRequestException httpException && httpException.StatusCode != null && httpException.StatusCode != HttpStatusCode.ProxyAuthenticationRequired)
            {
                if (httpException.StatusCode == HttpStatusCode.Unauthorized)
                {
                    //we can ignore this type of exception because main page listening it and handles
                    injectedServices.LoggingService.LogWarning($"Skip showing error popup for user because this error is handled in main view, errorMessage: {nameof(HttpRequestException)}: {httpException.Message}");
                }
                else if (httpException.StatusCode == HttpStatusCode.ServiceUnavailable)
                {
                    injectedServices.SnackBarService.ShowError($"The server is temporarily unavailable. Please try again later.");
                }
                else
                {
                    var error = x.Message.Replace("Response status code does not indicate success:", string.Empty);
                    injectedServices.SnackBarService.ShowError($"It seems server is not available, please try again later. ({(int)httpException.StatusCode} - {httpException.StatusCode}).");
                }

                return;
            }
            else if (x is AuthExpiredException)
            {
                //we can ignore this type of exception because main page listening it and handles
                injectedServices.LoggingService.LogWarning($"Skip showing error popup for user because this error is handled in main view, errorMessage: {nameof(AuthExpiredException)}: {x.Message}");
                return;
            }
            else if (IsNoInternetException(x))//Is no Internet error
            {
                injectedServices.SnackBarService.ShowError($"It looks like there may be an issue with your connection. Please check your internet connection and try again.");
            }
            else //show general error message
            {
                if (x is RestApiException)
                {
                    injectedServices.SnackBarService.ShowError("Internal Server Error. Please try again later.");
                }
                else
                {
                    injectedServices.SnackBarService.ShowError("Oops something went wrong, please try again later.");
                }
            }
        }

        protected bool IsNoInternetException(Exception ex)
        {
            if (ex is OperationCanceledException)
            {
                //happens when request timeout 
                return true;
            }

            if (ex is WebException)
            {
                return true;
            }

            var noInternet = injectedServices.PlatformError.IsHttpRareError(ex);
            //if (!noInternet)
            //{
            //    noInternet = !Services.DeviceInfo.HasInternetConnection;
            //}

            if (noInternet)
            {
                //HandleNoInternet(asyncAction);
                return true;
            }
            else
                return false;

        }
    }
}

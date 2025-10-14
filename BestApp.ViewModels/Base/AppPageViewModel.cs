using Base.Abstractions.REST.Exceptions;
using Base.Aspect;
using Base.MVVM.Events;
using Base.MVVM.Helper;
using Base.MVVM.ViewModels;
using System.Net;

namespace BestApp.ViewModels.Base
{
    [LogMethods]
    public class AppPageViewModel : PageViewModel
    {        
        private bool isFirstTimeAppears = true;
        private AppResumedEvent appResumedEvent;
        private AppPausedEvent appPausedEvent;
        public PageInjectedServices Services => injectedServices as PageInjectedServices;

        public AppPageViewModel(PageInjectedServices services) : base(services)
        {
            RefreshCommand = new AsyncCommand(OnRefreshCommand);
        }

        /// General Busy indicator that will be displayed as popup
        /// </summary>
        public bool BusyLoading { get; set; }                
        public bool IsRefreshing { get; set; }
        public AsyncCommand RefreshCommand { get; set; }
        public string Title { get; set; }

        protected virtual Task OnRefreshCommand(object arg)
        {
            return Task.CompletedTask;
        }

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
            Services.LoggingService.TrackError(x);            
            //Check is SERVER API error
            if (x is HttpRequestException httpException && httpException.StatusCode != null && httpException.StatusCode != HttpStatusCode.ProxyAuthenticationRequired)
            {
                if(httpException.StatusCode == HttpStatusCode.Unauthorized)
                {
                    //we can ignore this type of exception because main page listening it and handles
                    Services.LoggingService.LogWarning($"Skip showing error popup for user because this error is handled in main view, errorMessage: {nameof(HttpRequestException)}: {httpException.Message}");                    
                }
                else if (httpException.StatusCode == HttpStatusCode.ServiceUnavailable)
                {
                    Services.PopupAlertService.ShowError($"The server is temporarily unavailable. Please try again later.");
                }
                else
                {
                    var error = x.Message.Replace("Response status code does not indicate success:", string.Empty);
                    Services.PopupAlertService.ShowError($"It seems server is not available, please try again later. ({(int)httpException.StatusCode} - {httpException.StatusCode}).");
                }
                
                return;
            }
            else if(x is AuthExpiredException)
            {
                //we can ignore this type of exception because main page listening it and handles
                Services.LoggingService.LogWarning($"Skip showing error popup for user because this error is handled in main view, errorMessage: {nameof(AuthExpiredException)}: {x.Message}");
                return;
            }
            else if (IsNoInternetException(x))//Is no Internet error
            {                
                Services.PopupAlertService.ShowError($"It looks like there may be an issue with your connection. Please check your internet connection and try again.");
            }
            else //show general error message
            {                
                if (x is RestApiException)
                {
                    Services.PopupAlertService.ShowError("Internal Server Error. Please try again later.");
                }
                else
                {
                    Services.PopupAlertService.ShowError("Oops something went wrong, please try again later.");
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

            var noInternet = Services.PlatformError.IsHttpRareError(ex);
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

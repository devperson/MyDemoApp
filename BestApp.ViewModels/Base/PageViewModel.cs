using BestApp.Abstraction.General.Platform;
using BestApp.ViewModels.Helper;
using BestApp.ViewModels.Helper.Commands;
using Common.Abstrtactions;
using Example;
using Logging.Aspects;
using Microsoft.VisualBasic;
using System.Net;

namespace BestApp.ViewModels.Base
{
    [LogMethods]
    public class PageViewModel : NavigatingBaseViewModel, IPageLifecycleAware
    {
        protected ClickUtil clickUtil = new ClickUtil();
        public PageViewModel(InjectedServices services) : base(services)
        {
            RefreshCommand = new AsyncCommand(OnRefreshCommand);
        }

        /// General Busy indicator that will be displayed as popup
        /// </summary>
        public bool BusyLoading { get; set; }
        public AsyncCommand RefreshCommand { get; set; }
        private bool isFirstTimeAppears = true;
        public bool IsRefreshing { get; set; }

        public virtual void OnAppearing()
        {
            if(isFirstTimeAppears)
            {
                isFirstTimeAppears = false;
                OnFirstTimeAppears();
            }
        }

        public virtual void OnFirstTimeAppears()
        {

        }

        public virtual void OnAppeared()
        {
            Services.LoggingService.Log($"{this.GetType().Name}.OnAppeared() (from base)");
        }

        public virtual void OnDisappearing()
        {
            
        }

        public virtual void PausedToBackground()
        {
            
        }

        public virtual void ResumedFromBackground()
        {
            
        }

        protected virtual Task OnRefreshCommand(object arg)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// This method is recommended to be called in a Command. Because it uses clickUtil which is 
        /// class level varible (ExecuteOnlyOnceAsync) which is not recommended to be used multiple places or 
        /// to call it in two different places simultaneously in one class
        /// </summary>
        /// <param name="asyncAction">action to be executed</param>
        /// <param name="OnComplete">success lambada</param>
        /// <returns></returns>
        public async Task ShowLoading(Func<Task> asyncAction, Action<bool> OnComplete = null)
        {
            await clickUtil.ExecuteOnlyOnceAsync(async () =>
            {
                //if (!Service.DeviceInfo.HasInternetConnection)
                //{

                //    Services.PopupAlertService.ShowError(Strings.Error_NoInternet);
                //    OnComplete?.Invoke(false);
                //    return;
                //}

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
            });
        }

        public async Task ShowLoadingAndHandleError(Func<Task> asyncAction, Action<bool> OnComplete = null, bool skipCheckInternet = false)
        {
            await clickUtil.ExecuteOnlyOnceAsync(async () =>
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

                    BusyLoading = true;
                    success = await ExecuteAndHandleError(asyncAction);
                }
                finally
                {
                    BusyLoading = false;
                    OnComplete?.Invoke(success);
                }
            });
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
            Services.PopupAlertService.ShowError("Oops something went wrong, please try again later.");
            //Check is SERVER API error
            if (x is HttpRequestException && x.Message.Contains("status code does not indicate"))
            {
                var error = x.Message.Replace("Response status code does not indicate success:", string.Empty);                
                Services.PopupAlertService.ShowError($"It seems server is not available, please try again later. ErrorCode: {error}");
            }
            else if (IsNoInternetException(x))//Is no Internet error
            {                
                Services.PopupAlertService.ShowError($"It looks like there may be an issue with your connection. Please check your internet connection and try again.");
            }
            else //show general error message
            {
                //ToastSeverity = SeverityType.Error;
                //if (x is RestDataServiceException)
                //{
                //    ToastMessage = "Internal Server Error. Please try again later.";
                //}
                //else
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

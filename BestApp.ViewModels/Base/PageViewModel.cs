using BestApp.Abstraction.General.Platform;
using Common.Abstrtactions;

namespace BestApp.ViewModels.Base
{
    public class PageViewModel : NavigatingBaseViewModel, IPageLifecycleAware
    {
        public PageViewModel(InjectedServices services) : base(services)
        {
        }

        private bool isFirstTimeAppears = true;
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


        public void HandleUIError(Exception x)
        {
            Services.LoggingService.TrackError(x);
            Services.PopupAlertService.ShowError("Oops something went wrong, please try again later.");
            //if (x is HttpRequestException && x.Message.Contains("status code does not indicate"))
            //{
            //    var error = x.Message.Replace("Response status code does not indicate success:", string.Empty);
            //    ToastSeverity = SeverityType.Error;
            //    ToastMessage = $"It seems server is not available, please try again later. ErrorCode: {error}";
            //}
            //else if (IsNoInternetException(x))
            //{
            //    ToastSeverity = SeverityType.Error;
            //    ToastMessage = $"It looks like there may be an issue with your connection. Please check your internet connection and try again.";
            //}
            //else
            //{
            //    ToastSeverity = SeverityType.Error;
            //    if (x is RestDataServiceException)
            //    {
            //        ToastMessage = "Internal Server Error. Please try again later.";
            //    }
            //    else
            //    {
            //        ToastMessage = "Oops something went wrong, please try again later.";
            //    }
            //}
        }
    }
}

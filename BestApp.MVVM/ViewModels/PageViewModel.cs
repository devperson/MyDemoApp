using Base.MVVM.Events;
using Base.MVVM.Navigation;
using Logging.Aspects;
using INavigationParameters = Base.MVVM.Navigation.INavigationParameters;

namespace Base.MVVM.ViewModels
{
    [LogMethods]
    public abstract class AppPageViewModel : NavigatingBaseViewModel, IPageLifecycleAware
    {        
        private bool isFirstTimeAppears = true;
        private AppResumedEvent appResumedEvent;
        private AppPausedEvent appPausedEvent;

        public AppPageViewModel(PageInjectedServices services) : base(services)
        {
            //RefreshCommand = new AsyncCommand(OnRefreshCommand);

            if (services != null)
            {
                appResumedEvent = injectedServices.EventAggregator.GetEvent<AppResumedEvent>();
                appPausedEvent = injectedServices.EventAggregator.GetEvent<AppPausedEvent>();
                appResumedEvent.Subscribe(ResumedFromBackground);
                appPausedEvent.Subscribe(PausedToBackground);
            }
        }
        
        public bool IsPageVisable { get; set; }        
        //public AsyncCommand RefreshCommand { get; set; }        


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
        public virtual void ResumedFromBackground()
        {
            injectedServices.LoggingService.Log($"{this.GetType().Name}.ResumedFromBackground() (from base)");
        }

        [ExcludeFromLog]//manually log
        public virtual void PausedToBackground()
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
       
        
    }
}

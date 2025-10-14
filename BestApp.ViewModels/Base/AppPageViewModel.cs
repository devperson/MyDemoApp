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

           
        public bool IsRefreshing { get; set; }
        public AsyncCommand RefreshCommand { get; set; }
        public string Title { get; set; }

        protected virtual Task OnRefreshCommand(object arg)
        {
            return Task.CompletedTask;
        }

       
    }
}

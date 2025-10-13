using Base.MVVM.Helper;
using Base.MVVM.Navigation;
using System.Text;
using INavigationParameters = Base.MVVM.Navigation.INavigationParameters;

namespace Base.MVVM.ViewModels
{
    public abstract class NavigatingBaseViewModel : BaseViewModel, INavigationAware
    {
        protected readonly InjectedServices injectedServices;

        //private ClickUtil bkClUtl = new ClickUtil();  
        public NavigatingBaseViewModel(InjectedServices services)
        {
            this.BackCommand = new AsyncCommand(OnBackCommand);
            injectedServices = services;
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        /// <summary>
        /// Indicates whether page will have Navigation bar with Back button
        /// </summary>
        public bool CanGoBack => injectedServices != null ? injectedServices.NavigationService.CanNavigateBack : false;

        /// <summary>
        /// Android: Disables the device hardware back button
        /// </summary>
        public bool DisableDeviceBackButton { get; set; }

        public AsyncCommand BackCommand { get; set; }

        /// <summary>
        /// A shortcut of <see cref="IPageNavigationService.Navigate">Service.NavigationService.Navigate()</see>
        /// </summary>
        //[LogMethods]
        public async Task Navigate(string name, INavigationParameters parameters = null, bool useModalNavigation = false, bool animated = true, bool wrapIntoNav = false)
        {
            if (injectedServices.NavigationService != null)
            {
                //notify that we are going to navigate
                //Service.EventAggregator.GetEvent<OnStartNaavigatingEvent>().Publish(name);

                //await OnStartNavigatingTo(name, parameters);
                //do navigate
                await injectedServices.NavigationService.Navigate(name, parameters, useModalNavigation, animated, wrapIntoNav);
            }
        }

        protected virtual Task OnStartNavigatingTo(string name, INavigationParameters parameters)
        {
            return Task.CompletedTask;
        }

        //[LogMethods]
        public async Task SkipAndNavigate(int skipCount, string route, INavigationParameters parameters = null)
        {
            var skip = string.Empty;
            for (int i = 0; i < skipCount; i++)
            {
                skip += "../";
            }
            route = $"{skip}{route}";

            await Navigate(route, parameters);
        }

        //[LogMethods]
        public async Task NavigateAndMakeRoot(string name, INavigationParameters parameters = null, bool useModalNavigation = false, bool animated = true)
        {
            var newRoot = $"/NavigationPage/{name}";
            await injectedServices.NavigationService.Navigate(newRoot, parameters, useModalNavigation, animated);
        }

        //[LogMethods]
        public async Task NavigateToRoot(INavigationParameters parameters)
        {
            await injectedServices.NavigationService.NavigateToRoot(parameters);
        }

        public NavigatingBaseViewModel GetCurrentPageViewModel()
        {
            return injectedServices.NavigationService.GetCurrentPageModel() as NavigatingBaseViewModel;
        }

        //[LogMethods]
        public Task NavigateBack(INavigationParameters navParams = null)
        {
            //if (bkClUtl.IsOneClickEvent())
            //{
                return Navigate("../", navParams);
            //}
            //else
            //{
            //    //Sometime user can unintentionally click twice on back button (double click), which can cause crash (trying to navigate back from root page)                
            //    Service.LoggingService.Log($"NavigatingBaseViewModel.NavigateBack() CANCELLED to avoid double click issue, which can cause double pop navigation.");
            //    return Task.CompletedTask;
            //}
        }

        public async Task BackToRootAndNavigate(string name, INavigationParameters parameters = null)
        {
            var navStack = injectedServices.NavigationService.GetNavStackModels().Select(s => s.ToString().Split('.').Last()).ToList();
            string currentNavStack = string.Empty;
            //generate debug string
            if (navStack.Count > 1)
            {
                currentNavStack = string.Join("/", navStack);
            }
            else
            {
                currentNavStack = navStack.FirstOrDefault().ToString();
            }
            //generate final uri
            //Find out how much we should go back to get root page
            var popCount = navStack.Count - 1;
            StringBuilder popPageUri = new StringBuilder();
            for (int i = 0; i < popCount; i++)
            {
                popPageUri.Append("../");
            }

            var resultUri = $"{popPageUri}{name}";
            injectedServices.LoggingService.Log($"BackToRootAndNavigate(): Current navigation stack: /{currentNavStack}, pop count: {popCount}, resultUri: {resultUri}");
            await Navigate($"{popPageUri}{name}", parameters);
        }

        /// <summary>
        /// Tries to pop to existing page or navigates to new page
        /// </summary>
        /// <typeparam name="T">Page for navigation</typeparam>
        /// <param name="pageId">will compare this pageId with INavPageId.PageId of the searched page</param>
        public async Task BackToOrNavigate<T>(string pageId, INavigationParameters parameters = null)
        {
            var pageVmName = typeof(T).Name;
            var navStack = injectedServices.NavigationService.GetNavStackModels();
            var currentNavStack = string.Join("/", navStack);
            var searchedItem = navStack.FirstOrDefault(s => s is T); //&& s is INavPageId navPage && navPage.PageId == pageId);
            if (searchedItem != null)
            {
                var pageIndex = navStack.IndexOf(searchedItem);

                injectedServices.LoggingService.Log($"GoToOrNavigate(): Current page index in the navigation stack: {pageIndex}, T:{typeof(T)}, Navigation Stack: {currentNavStack}");
                StringBuilder popPageUri = new StringBuilder();
                for (int i = navStack.Count; i < pageIndex; i++)
                {
                    popPageUri.Append("../");
                }

                var resultUri = $"{popPageUri}{pageVmName}";
                injectedServices.LoggingService.Log($"GoToOrNavigate(): Result uri: {resultUri}");
                await Navigate(resultUri, parameters);
            }
            else
            {
                injectedServices.LoggingService.Log($"GoToOrNavigate(): Can not find searched page, pageId: {pageId}, T:{typeof(T)}");

                await Navigate(pageVmName, parameters);
            }
        }

        public void DoDeviceBackCommand()
        {
            injectedServices.LoggingService.Log($"{this.GetType().Name}.DoDeviceBackCommand() (from base)");

            if (DisableDeviceBackButton)
            {
                injectedServices.LoggingService.Log($"Cancel {this.GetType().Name}.DoDeviceBackCommand(): Ignore back command because this page is set to cancel any device back button.");
            }
            else
            {
                BackCommand?.Execute();
            }
        }

        protected void GetParameter<T>(INavigationParameters parameters, string key, ref T value)
        {
            if (parameters.ContainsKey(key))
                value = parameters.GetValue<T>(key);
        }

        protected void GetParameter<T>(INavigationParameters parameters, string key, Action<T> setter)
        {
            if (parameters.ContainsKey(key))
                setter(parameters.GetValue<T>(key));
        }

        protected T GetParameter<T>(INavigationParameters parameters, string key)
        {
            if (parameters.ContainsKey(key))
                return parameters.GetValue<T>(key);

            return default(T);
        }

        protected virtual async Task OnBackCommand()
        {
            injectedServices.LoggingService.Log($"{this.GetType().Name}.OnBackCommand() (from base)");
            await this.NavigateBack();
        }
    }
}

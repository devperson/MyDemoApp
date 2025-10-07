using Android.Content;
using Android.Runtime;
using Android.Util;
using BestApp.Abstraction.General.UI.Navigation;
using BestApp.X.Droid.Pages.Base;
using BestApp.X.Droid.Utils;
using Common.Abstrtactions;
using KYChat.Controls.Navigation;
using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentManager = AndroidX.Fragment.App.FragmentManager;
using INavigationParameters = BestApp.Abstraction.General.UI.Navigation.INavigationParameters;
using NavigationParameters = BestApp.ViewModels.NavigationParameters;

namespace BestApp.X.Droid
{
    //[LogMethods]
    public class PageNavigationFrameLayout : FrameLayout , IPageNavigationService
    {
        public PageNavigationFrameLayout(Context context) : base(context)
        {
        }

        public PageNavigationFrameLayout(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public PageNavigationFrameLayout(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public PageNavigationFrameLayout(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected PageNavigationFrameLayout(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }
        
        private bool _disposed;
        private int animationDuration = 250;
        private MainActivity mainActivity;
        private FragmentManager FragmentManager => mainActivity.SupportFragmentManager;
        internal readonly List<LifecyclePage> navStack = new List<LifecyclePage>();
        internal LifecyclePage currentPage;       

        public bool CanNavigateBack
        {
            get
            {
                return navStack.Count > 1;
            }
        }

        private ILoggingService _logger;
        public ILoggingService Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = ContainerLocator.Container.Resolve<ILoggingService>();
                }

                return _logger;
            }
        }

        public void SetActivity(MainActivity activity)
        {
            this.mainActivity = activity;
        }

        

        public async Task Navigate(string url, INavigationParameters parameters = null, bool useModalNavigation = false, bool animated = true, bool wrapIntoNav = false)
        {
            try
            {
                if (parameters == null)
                {
                    parameters = new NavigationParameters();
                }

                var navInfo = UrlNavigationHelper.Parse(url);

                if (navInfo.isPush)
                {
                    await this.OnPushAsync(url, parameters, animated);
                }
                else if (navInfo.isPop)
                {
                    await this.OnPopAsync(parameters);
                }
                else if (navInfo.isMultiPop)
                {
                    await this.OnMultiPopAsync(url, parameters, animated);
                }
                else if (navInfo.isMultiPopAndPush)
                {
                    await this.OnMultiPopAndPush(url, parameters, animated);
                }
                else if (navInfo.isPushAsRoot)
                {
                    await this.OnPushRootAsync(url, parameters, animated);
                }
                else if (navInfo.isMultiPushAsRoot)
                {
                    await this.OnMultiPushRootAsync(url, parameters, animated);
                }
                else
                {
                    throw new NotImplementedException("Navigation case is not implemented.");
                }
            }
            catch (Exception ex)
            {
                Logger.TrackError(ex);
                PrintCurrentStack();
            }
        }

        private void PrintCurrentStack()
        {
            var currentStack = GetNavStackModels();
            var currentUri = string.Join('/', currentStack);
            Logger.Log($"{nameof(PageNavigationFrameLayout)}: current stack: {currentUri}");
        }

        public async Task NavigateToRoot()
        {
            try
            {
                await this.OnPopToRootAsync();
            }
            catch (Exception ex)
            {
                Logger.TrackError(ex);
            }
        }

        private async Task OnPushAsync(string vmName, INavigationParameters parameters, bool animated)
        {
            //create new page
            var oldPage = currentPage;
            var newPage = currentPage = NavRegisterar.CreatePage(vmName, parameters);

            //save new page in local stack list
            newPage.pushNavAnimated = animated;
            navStack.Add(newPage);

            //push new page to ui stack
            var pushTransaction = FragmentManager.BeginTransaction();
            if (animated)
            {
                pushTransaction.SetCustomAnimations(Resource.Animation.slide_right_in, Resource.Animation.slide_right_out);
            }
            pushTransaction.Add(Id, newPage);
            pushTransaction.CommitAllowingStateLoss();

            //call viewmodel lifecycle methods
            oldPage.ViewModel.OnNavigatedFrom(null);
            newPage.ViewModel.OnNavigatedTo(parameters);

            //hide keyboard if open 
            Context.HideKeyboard(this);

            if (animated)
            {
                await Task.Delay(animationDuration);
            }

            //hide current page 
            var hideTransaction = FragmentManager.BeginTransaction();
            hideTransaction.Hide(oldPage);
            hideTransaction.CommitAllowingStateLoss();
        }

        private async Task OnPopAsync(INavigationParameters parameters)
        {
            if (navStack.Count == 1)
            {
                return;
            }

            var popPage = currentPage;
            var animated = popPage.pushNavAnimated;
            //hide poped page
            var hideTransaction = FragmentManager.BeginTransaction();

            if (animated)
            {
                hideTransaction.SetCustomAnimations(Resource.Animation.slide_right_in, Resource.Animation.slide_right_out);
            }

            hideTransaction.Hide(popPage);
            hideTransaction.CommitAllowingStateLoss();

            //remove from local stack list
            navStack.Remove(popPage);

            //show beneath page
            var toShowPage = navStack.Last();
            var showTransaction = FragmentManager.BeginTransaction();
            showTransaction.Show(toShowPage);
            showTransaction.CommitAllowingStateLoss();

            //call viewmodel lifecycle methods
            currentPage = toShowPage;
            popPage.ViewModel.OnNavigatedFrom(null);
            currentPage.ViewModel.OnNavigatedTo(parameters);

            //hide keyboard if open 
            Context.HideKeyboard(this);

            //if navigation is animated then wait for compilation
            if (animated)
            {
                await Task.Delay(animationDuration);
            }

            //remove poped page
            var removeTransaction = FragmentManager.BeginTransaction();
            removeTransaction.Remove(popPage);
            removeTransaction.CommitAllowingStateLoss();
        }

        private async Task OnMultiPopAsync(string url, INavigationParameters parameters, bool animated)
        {            
            var pagesToRemove = new List<LifecyclePage>();
            var splitedCount = url.Split('/').Length - 1;            
            for (int i = 0; i < splitedCount; i++)
            {
                var pageToRemove = navStack.LastOrDefault();                
                if(pagesToRemove == null)
                {
                    //this can happen if user somehow removed this page for example: tapped device back while app removes this page, or double tap
                    Logger.LogWarning($"{nameof(PageNavigationFrameLayout)}: Canceling OnMultiPopAsync() because pageToRemove is null");
                    return;
                }
                navStack.Remove(pageToRemove);
                pagesToRemove.Add(pageToRemove);
            }

            var hideTransaction = FragmentManager.BeginTransaction();
            if (animated)
            {
                hideTransaction.SetCustomAnimations(Resource.Animation.slide_right_in, Resource.Animation.slide_right_out);
            }
            hideTransaction.Hide(currentPage);

            //first show home page
            currentPage = navStack.Last();
            var showTransaction = FragmentManager.BeginTransaction();
            showTransaction.Show(currentPage);
            showTransaction.CommitAllowingStateLoss();

            //then start pop animation
            hideTransaction.CommitAllowingStateLoss();

          
            //call viewmodel lifecycle methods
            currentPage.ViewModel.OnNavigatedTo(parameters);

            //hide keyboard if open 
            Context.HideKeyboard(this);


            if (animated)
            {
                await Task.Delay(animationDuration);
            }
            //removed pages after navigating to destination
            var removeTransaction = FragmentManager.BeginTransaction();
            foreach (var page in pagesToRemove)
            {
                removeTransaction.Remove(page);
            }
            removeTransaction.CommitAllowingStateLoss();           
        }

        private async Task OnMultiPopAndPush(string url, INavigationParameters parameters, bool animated)
        {
            //push new page to ui stack
            var pushTransaction = FragmentManager.BeginTransaction();
            if (animated)
            {
                pushTransaction.SetCustomAnimations(Resource.Animation.slide_right_in, Resource.Animation.slide_right_out);
            }

            var vmName = url.Replace("../", string.Empty);

            currentPage = NavRegisterar.CreatePage(vmName, parameters);
            navStack.Add(currentPage);
            pushTransaction.Add(Id, currentPage);

            pushTransaction.CommitAllowingStateLoss();

            //call viewmodel lifecycle methods
            currentPage.ViewModel.OnNavigatedTo(parameters);

            //hide keyboard if open 
            Context.HideKeyboard(this);

            //removed pages after navigating to destination
            var removeTransaction = FragmentManager.BeginTransaction();
            var splitedCount = url.Split('/').Length - 1;

            for (int i = 1; i <= splitedCount; i++)
            {
                var pageToRemove = navStack.Last(p => p != currentPage);
                navStack.Remove(pageToRemove);
                removeTransaction.Remove(pageToRemove);
            }

            if (animated)
            {
                await Task.Delay(animationDuration);
            }
            removeTransaction.CommitAllowingStateLoss();
        }

        private async Task OnPushRootAsync(string url, INavigationParameters parameters, bool animated)
        {           
            //create page and save it to local stack list
            var vmName = url.Replace("/", string.Empty).Replace("NavigationPage", "");
            currentPage = NavRegisterar.CreatePage(vmName, parameters);
            navStack.Add(currentPage);

            //remove other pages except currentPage, it will become root page
            var pagesToRemove = navStack.Where(p => p != currentPage).ToList();
            //clear local stack list
            navStack.RemoveAll(pagesToRemove.Contains);

            //add page to ui stack
            var pushTransaction = FragmentManager.BeginTransaction();
            if (animated)
            {
                pushTransaction.SetCustomAnimations(Resource.Animation.slide_right_in, Resource.Animation.slide_right_out);
            }
            pushTransaction.Add(Id, currentPage);
            pushTransaction.CommitAllowingStateLoss();

            //call viewmodel lifecycle methods
            currentPage.ViewModel.OnNavigatedTo(parameters);

            //hide keyboard if open 
            Context.HideKeyboard(this);

            //if navigation is animated then wait for compilation
            if (animated)
            {
                await Task.Delay(animationDuration);
            }

            
            var removeTransaction = FragmentManager.BeginTransaction();
            foreach (var page in pagesToRemove)
            {
                removeTransaction.Remove(page);
            }
            removeTransaction.CommitAllowingStateLoss();
        }

        private async Task OnMultiPushRootAsync(string url, INavigationParameters parameters, bool animated)
        {
            //remove existing pages
            var pagesToRemove = navStack.ToList();
            //clear local stack list
            navStack.Clear();

            //create page and save it to local stack list
            var cleanUrl = url.Replace("/NavigationPage", string.Empty);

            var vmPages = cleanUrl.Split("/").Where(s => !string.IsNullOrEmpty(s)).ToList();
            var pushTransaction = FragmentManager.BeginTransaction();
            if (animated)
            {
                pushTransaction.SetCustomAnimations(Resource.Animation.slide_right_in, Resource.Animation.slide_right_out);
            }

            foreach (var vmName in vmPages)
            {
                var page = NavRegisterar.CreatePage(vmName, parameters);
                //add page to ui stack
                page.pushNavAnimated = animated;
                navStack.Add(page);

                if(vmName == vmPages.Last())
                {
                    currentPage = page;                  
                    pushTransaction.Add(Id, currentPage);
                }
                else
                {
                    pushTransaction.Add(Id, page);
                    pushTransaction.Hide(page);
                }                
            }
         
            pushTransaction.CommitAllowingStateLoss();

            //call viewmodel lifecycle methods
            currentPage.ViewModel.OnNavigatedTo(parameters);

            //hide keyboard if open 
            Context.HideKeyboard(this);

            //if navigation is animated then wait for compilation
            if (animated)
            {
                await Task.Delay(animationDuration);
            }

            if (pagesToRemove.Any())
            {
                //remove other pages except currentPage, it will become root page           
                var removeTransaction = FragmentManager.BeginTransaction();
                foreach (var page in pagesToRemove)
                {
                    removeTransaction.Remove(page);
                }
                removeTransaction.CommitAllowingStateLoss();
            }
        }

        private async Task OnPopToRootAsync()
        {
            if (navStack.Count <= 1)
            {
                return;
            }
            else if (navStack.Count == 2)
            {
                await this.OnPopAsync(new NavigationParameters());
            }
            else
            {
                var rootPage = navStack.First();
                //show root page
                var showTransaction = FragmentManager.BeginTransaction();
                showTransaction.Show(rootPage);
                showTransaction.CommitAllowingStateLoss();

                //call viewmodel lifecycle methods
                rootPage.ViewModel.OnNavigatedTo(new NavigationParameters());

                var pagesToRemove = new List<LifecyclePage>();                
                var popAnimTransaction = FragmentManager.BeginTransaction();
                while (navStack.Count > 1)
                {
                    var pageToHide = navStack.Last();
                    navStack.Remove(pageToHide);
                    pagesToRemove.Add(pageToHide);

                    if (pageToHide == currentPage)
                    {
                        //hide current page with animation
                        popAnimTransaction.SetCustomAnimations(Resource.Animation.slide_right_in, Resource.Animation.slide_right_out);
                        popAnimTransaction.Hide(pageToHide);
                    }                                       
                }
                
                popAnimTransaction.CommitAllowingStateLoss();

                currentPage = rootPage;
                currentPage.ViewModel.OnNavigatedTo(new NavigationParameters());

                //hide keyboard if open 
                Context.HideKeyboard(this);

                await Task.Delay(animationDuration);

                var removeTransaction = FragmentManager.BeginTransaction();
                foreach (var page in pagesToRemove)
                {
                    removeTransaction.Remove(page);
                }
                removeTransaction.CommitAllowingStateLoss();
            }
        }

        public Task CloseModal(INavigationParameters parameters = null)
        {
            throw new NotImplementedException();
        }

        public object GetCurrentPageModel()
        {
            var page = navStack.LastOrDefault();
            return page?.ViewModel;
        }

        public object GetRootPageModel()
        {
            var page = navStack.FirstOrDefault();
            return page?.ViewModel;
        }

        public LifecyclePage GetCurrentPage()
        {
            var page = navStack.LastOrDefault();
            return page;
        }

        public bool HasPageInNavigation(string page)
        {
            throw new NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _disposed = true;

            if (!this.FragmentManager.IsDestroyed)
            {
                var trans = this.FragmentManager.BeginTransaction();

                foreach (var fragment in navStack)
                {
                    trans.Remove(fragment as Fragment);
                }
                trans.CommitAllowingStateLoss();
                this.FragmentManager.ExecutePendingTransactions();
            }

            base.Dispose(disposing);
        }

        public List<object> GetNavStackModels()
        {
            var viewModels = navStack.Select(x => x.ViewModel as object).ToList();
            return viewModels;
        }

        //protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        //{
        //    base.OnLayout(changed, left, top, right, bottom);

        //    var height = this.Height;
        //    Console.WriteLine($"Height of nav page = {height}");
        //}
    }
}
